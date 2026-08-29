// Assets/Scripts/Stages/WaveSpawner.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 스테이지 데이터의 웨이브 순서를 읽어서 몬스터를 순차적으로 스폰한다.
    // 보스 웨이브도 별도 분기 없이, 웨이브 구성 데이터에 보스 몬스터가 들어있는 것으로 처리된다.
    public class WaveSpawner : MonoBehaviour
    {
        [Tooltip("모든 몬스터 종류가 공유하는 단일 프리팹이다. 실제 데이터는 스폰 시점에 주입된다.")]
        [SerializeField]
        private MonsterBase _monsterPrefab;

        private int _currentWaveIndex = -1;
        private bool _skipRequested;

        // 지금 진행 중인 웨이브의 인덱스이다. 아직 시작 전이면 -1이다.
        public int CurrentWaveIndex => _currentWaveIndex;

        // 현재 실행 중인 스테이지 데이터이다. NextWavePreviewUI 등 외부에서 웨이브 구성을 조회할 때 사용한다.
        public StageDataSO CurrentStageData { get; private set; }

        // 마지막 웨이브까지 스폰이 전부 끝났는지 여부이다. 클리어 판정에 사용한다.
        public bool IsSpawningComplete { get; private set; }

        public void StartStage(StageDataSO stageData, StageReferences stageReferences)
        {
            StartCoroutine(RunWaves(stageData, stageReferences));
        }

        // 웨이브 사이 대기시간을 건너뛴다. 대기 중이 아닐 때 호출해도 안전하게 무시된다.
        public void SkipWaveDelay()
        {
            _skipRequested = true;
        }

        private IEnumerator RunWaves(StageDataSO stageData, StageReferences stageReferences)
        {
            CurrentStageData = stageData;
            IsSpawningComplete = false;

            for (int waveIndex = 0; waveIndex < stageData.WaveSequence.Count; waveIndex++)
            {
                if (waveIndex > 0)
                {
                    yield return WaitForNextWave(stageData.TimeBetweenWaves);
                }

                _currentWaveIndex = waveIndex;
                WaveData wave = stageData.WaveSequence[waveIndex];
                float waveMultiplier = CalculateWaveMultiplier(stageData, waveIndex);

                foreach (WaveMonsterEntry entry in wave.MonsterEntries)
                {
                    for (int spawnedCount = 0; spawnedCount < entry.Count; spawnedCount++)
                    {
                        SpawnMonster(entry.Monster, waveMultiplier, stageReferences.Waypoints);
                        yield return new WaitForSeconds(stageData.SpawnInterval);
                    }
                }
            }

            IsSpawningComplete = true;
        }

        // 스테이지 배율에 웨이브 진행에 따른 추가 배율을 곱한 최종 배율을 계산한다.
        // 웨이브 번호가 커질수록(0부터 시작) 몬스터가 조금씩 더 강해지며, 스테이지가 바뀌면 0부터 다시 계산된다.
        private float CalculateWaveMultiplier(StageDataSO stageData, int waveIndex)
        {
            float waveBonus = 1f + waveIndex * stageData.WaveScalingRate;
            return stageData.DifficultyMultiplier * waveBonus;
        }

        // 다음 웨이브까지 대기한다. 대기 중 SkipWaveDelay가 호출되면 즉시 종료한다.
        // 대기가 시작되기 전에 미리 눌린 스킵 요청도 반영되도록, 대기 시작 시점이 아니라 종료 시점에 초기화한다.
        private IEnumerator WaitForNextWave(float delay)
        {
            float elapsedTime = 0f;

            while (elapsedTime < delay && !_skipRequested)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _skipRequested = false;
        }

        private void SpawnMonster(MonsterDataSO monsterData, float difficultyMultiplier, List<Transform> waypoints)
        {
            Transform startPoint = waypoints[0];
            MonsterBase monster = Instantiate(_monsterPrefab, startPoint.position, Quaternion.identity);

            monster.Initialize(monsterData, difficultyMultiplier);
            monster.SetPath(waypoints);
        }
    }
}
