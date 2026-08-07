// Assets/Scripts/Core/EconomyManager.cs

namespace ElementTD
{
    // 게임 전체의 골드를 관리한다.
    // 골드 획득, 차감, 지불 가능 여부 확인을 담당한다.
    public static class EconomyManager
    {
        public static int CurrentGold;

        public static void Initialize(int startingGold)
        {
            CurrentGold = startingGold;
        }

        public static void AddGold(int amount)
        {
            CurrentGold += amount;
        }

        // 비용을 지불할 수 있으면 차감하고 true를 반환한다.
        // 부족하면 아무것도 하지 않고 false를 반환한다.
        public static bool TrySpendGold(int amount)
        {
            if (CurrentGold < amount)
            {
                return false;
            }

            CurrentGold -= amount;
            return true;
        }
    }
}
