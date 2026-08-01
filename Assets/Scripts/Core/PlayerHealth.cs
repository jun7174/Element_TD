// Assets/Scripts/Core/PlayerHealth.cs

using UnityEngine;

namespace ElementTD
{
    // 플레이어 생명력을 관리한다.
    // 지금은 차감과 로그 출력만 담당하고, 실제 UI 표시와 게임오버 처리는 이후 단계에서 연결한다.
    public static class PlayerHealth
    {
        public static int CurrentHealth = 100;

        public static void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            Debug.Log("플레이어 생명력 감소, 남은 생명력 " + CurrentHealth);

            if (CurrentHealth <= 0)
            {
                Debug.Log("게임 오버");
            }
        }
    }
}
