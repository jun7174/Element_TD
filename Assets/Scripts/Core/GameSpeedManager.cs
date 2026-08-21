// Assets/Scripts/Core/GameSpeedManager.cs

using UnityEngine;

namespace ElementTD
{
    // 게임 배속과 일시정지 상태를 중앙에서 관리한다.
    // Time.timeScale은 이 클래스만 직접 건드려서, 배속 기능과 일시정지 기능이
    // 서로의 값을 덮어쓰지 않게 한다.
    public static class GameSpeedManager
    {
        public static float CurrentSpeed { get; private set; } = 1f;
        public static bool IsPaused { get; private set; }

        public static void SetSpeed(float speed)
        {
            CurrentSpeed = speed;
            ApplyTimeScale();
        }

        // 스테이지 시작 시 호출해서 배속을 1배속으로 되돌린다.
        public static void ResetSpeed()
        {
            SetSpeed(1f);
        }

        public static void Pause()
        {
            IsPaused = true;
            ApplyTimeScale();
        }

        public static void Resume()
        {
            IsPaused = false;
            ApplyTimeScale();
        }

        public static void TogglePause()
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private static void ApplyTimeScale()
        {
            Time.timeScale = IsPaused ? 0f : CurrentSpeed;
        }
    }
}
