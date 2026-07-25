// Assets/Scripts/Data/Elements/TargetStat.cs

namespace ElementTD
{
    // 원소 효과가 어떤 스탯을 대상으로 하는지 나타낸다.
    // 전투형 타워의 개별 스탯과, 지원형과 경제형 타워가 공유하는 핵심 수치를 모두 포함한다.
    public enum TargetStat
    {
        AttackSpeed,
        Damage,
        ArmorPenetration,
        Range,
        CritChance,
        CoreValue
    }
}
