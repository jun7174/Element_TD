// Assets/Scripts/Data/Stages/StageDataSO.cs

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    [Serializable]
    public class WaveMonsterEntry
    {
        public MonsterDataSO Monster;
        public int Count;
    }

    [Serializable]
    public class WaveData
    {
        [Tooltip("이 웨이브에 등장하는 몬스터 종류와 수량 목록이다.")]
        public List<WaveMonsterEntry> MonsterEntries;
    }

    [CreateAssetMenu(fileName = "NewStageData", menuName = "ElementTD/Stage Data")]
    public class StageDataSO : ScriptableObject
    {
        [Tooltip("이 스테이지의 고정 구조가 담긴 프리팹이다. 타일맵과 웨이포인트를 자식으로 포함한다.")]
        public GameObject StagePrefab;

        [Range(0f, 1f)]
        [Tooltip("설치 가능 타일 중 원소가 부여되는 비율이다. 0.3에서 0.4 사이 값을 사용한다.")]
        public float ElementTileRatio;

        [Tooltip("고정 웨이브 순서이다. 총 열 개의 웨이브를 담는다.")]
        public List<WaveData> WaveSequence;

        [Tooltip("몬스터의 체력, 누출 피해, 처치 골드에 곱해지는 난이도 배율이다.")]
        public float DifficultyMultiplier;

        public int StartingGold;

        [Tooltip("같은 웨이브 안에서 몬스터가 하나씩 스폰되는 간격이다. 단위는 초이다.")]
        public float SpawnInterval;
    }
}
