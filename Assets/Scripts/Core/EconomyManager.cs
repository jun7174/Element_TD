// Assets/Scripts/Core/EconomyManager.cs

using UnityEngine;

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

        // 골드를 더하거나(양수) 뺀다(음수). 결과가 0 미만으로 내려가지 않게 제한한다.
        public static void AddGold(int amount)
        {
            CurrentGold = Mathf.Max(0, CurrentGold + amount);
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
