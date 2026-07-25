// Assets/Scripts/Data/Towers/TowerUpgradeState.cs

namespace ElementTD
{
    // 타워 인스턴스가 지금 몇 강 상태인지 나타낸다.
    // 강화 단계별 수치 자체는 TowerUpgradeLevelData가 담당하고,
    // 이 열거형은 실행 중인 타워가 지금 어느 상태인지만 나타낸다.
    public enum TowerUpgradeState
    {
        Base,
        First,
        Second
    }
}
