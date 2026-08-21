// Assets/Scripts/UI/PauseButton.cs

using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 일시정지 토글 버튼의 동작과 표시 문구를 담당한다.
    public class PauseButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _buttonLabel;

        private void Update()
        {
            _buttonLabel.text = GameSpeedManager.IsPaused ? "재개" : "일시정지";
        }

        public void OnPauseButtonClicked()
        {
            GameSpeedManager.TogglePause();
        }
    }
}
