// Assets/Scripts/Data/Towers/TowerUpgradeDataSO.cs

using System;
using UnityEngine;

namespace ElementTD
{
    [Serializable]
    public class TowerUpgradeLevelData
    {
        [Tooltip("기본 핵심 수치 대비 증가율이다. 0.3은 30 퍼센트 증가를 의미한다.")]
        public float CoreValueIncreaseRate;

        [Tooltip("설치 비용 대비 강화 비용 비율이다. 0.6은 설치비의 60 퍼센트를 의미한다.")]
        public float CostRatio;
    }

    [CreateAssetMenu(fileName = "NewTowerUpgradeData", menuName = "ElementTD/Tower Upgrade Data")]
    public class TowerUpgradeDataSO : ScriptableObject
    {
        [Tooltip("1강 적용 시 사용하는 증가율과 비용 비율이다.")]
        public TowerUpgradeLevelData FirstUpgrade;

        [Tooltip("2강 적용 시 사용하는 증가율과 비용 비율이다.")]
        public TowerUpgradeLevelData SecondUpgrade;
    }
}
