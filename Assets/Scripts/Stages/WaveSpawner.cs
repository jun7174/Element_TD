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

        // 지금 진행 중인 웨이브의 인덱스이다. 아직 시작 전이면 -1이다.
        public int CurrentWaveIndex => _currentWaveIndex;

        // 현재 실행 중인 스테이지 데이터이다. NextWavePreviewUI 등 외부에서 웨이브 구성을 조회할 때 사용한다.
        public StageDataSO CurrentStageData { get; private set; }

        public void StartStage(StageDataSO stageData, StageReferences stageReferences)
        {
            StartCoroutine(RunWaves(stageData, stageReferences));
        }

        private IEnumerator RunWaves(StageDataSO stageData, StageReferences stageReferences)
        {
            CurrentStageData = stageData;

            for (int waveIndex = 0; waveIndex < stageData.WaveSequence.Count; waveIndex++)
            {
                _currentWaveIndex = waveIndex;
                WaveData wave = stageData.WaveSequence[waveIndex];

                foreach (WaveMonsterEntry entry in wave.MonsterEntries)
                {
                    for (int spawnedCount = 0; spawnedCount < entry.Count; spawnedCount++)
                    {
                        SpawnMonster(entry.Monster, stageData.DifficultyMultiplier, stageReferences.Waypoints);
                        yield return new WaitForSeconds(stageData.SpawnInterval);
                    }
                }
            }
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
