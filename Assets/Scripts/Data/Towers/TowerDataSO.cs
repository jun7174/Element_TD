// Assets/Scripts/Data/Towers/TowerDataSO.cs

using UnityEngine;

namespace ElementTD
{
    [CreateAssetMenu(fileName = "NewTowerData", menuName = "ElementTD/Tower Data")]
    public class TowerDataSO : ScriptableObject
    {
        [Header("공통 정보")]
        public string TowerName;
        public TowerType Type;
        public int InstallCost;
        public TowerUpgradeDataSO UpgradeData;

        [Header("전투형 전용 필드")]
        [Tooltip("전투형 타워에만 사용한다. 지원형과 경제형 타워는 기본값을 그대로 둔다.")]
        public float BaseDamage;
        public float AttackSpeed;
        public float AttackRange;
        public AttackPatternType AttackPattern;
        public TargetingStrategySO TargetingStrategy;

        [Tooltip("범위공격 타워에만 사용하며, 그 외 전투형 타워는 0으로 둔다.")]
        public float SplashRadius;

        [Header("지원형 전용 필드")]
        [Tooltip("지원형 타워에만 사용한다. 버프타워는 공격력 증가량, 디버프타워는 이동속도 감소량으로 사용한다.")]
        public float EffectRadius;
        public float EffectAmount;

        [Header("경제형 전용 필드")]
        [Tooltip("경제형 타워에만 사용한다.")]
        public float GoldPerSecond;
    }
}
