// Assets/Scripts/Core/GameTimer.cs

using UnityEngine;

namespace ElementTD
{
    // 배속 등 게임 속도 조절과 무관하게 흐르는 실제 플레이 시간을 관리한다.
    // Time.unscaledDeltaTime을 누적하므로 Time.timeScale이 바뀌어도 영향을 받지 않는다.
    // 씬에 이 컴포넌트를 하나 두면 매 프레임 시간을 누적하고, 값은 정적으로 어디서든 조회 가능하다.
    public class GameTimer : MonoBehaviour
    {
        public static float ElapsedSeconds { get; private set; }

        private void Update()
        {
            if (GameSpeedManager.IsPaused)
            {
                return;
            }

            ElapsedSeconds += Time.unscaledDeltaTime;
        }

        public static void ResetTimer()
        {
            ElapsedSeconds = 0f;
        }
    }
}
