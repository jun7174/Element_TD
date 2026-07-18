// Assets/Scripts/Data/Towers/AttackPatternType.cs

namespace ElementTD
{
    // 전투형 타워의 공격 방식을 나타낸다.
    // 전투형 타워에만 유효하며, 지원형과 경제형 타워는 이 값을 사용하지 않는다.
    public enum AttackPatternType
    {
        Basic, // 기본 공격
        RapidFire, // 연사 공격
        Splash, // 범위 공격
        Sniper // 저격 공격
    }
}
