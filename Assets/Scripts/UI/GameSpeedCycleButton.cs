// Assets/Scripts/UI/GameSpeedCycleButton.cs

using System;
using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 배속 순환 버튼 하나의 동작과 표시 문구를 담당한다.
    // 누를 때마다 1배속, 2배속, 4배속을 순서대로 돌아가며 적용한다.
    public class GameSpeedCycleButton : MonoBehaviour
    {
        private static readonly float[] SpeedSteps = { 1f, 2f, 4f };

        [SerializeField]
        private TextMeshProUGUI _buttonLabel;

        private void Update()
        {
            _buttonLabel.text = GameSpeedManager.CurrentSpeed + "배속";
        }

        public void OnSpeedButtonClicked()
        {
            SoundManager.instance.PlaySFX(ESfx.BUTTON_CLICK);
            int currentIndex = Array.IndexOf(SpeedSteps, GameSpeedManager.CurrentSpeed);
            int nextIndex = (currentIndex + 1) % SpeedSteps.Length;
            GameSpeedManager.SetSpeed(SpeedSteps[nextIndex]);
        }
    }
}
