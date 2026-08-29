// Assets/Scripts/UI/StartMenuController.cs

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElementTD
{
    // 시작 화면의 세 버튼(시작하기, 설정, 종료)의 동작을 담당한다.
    // 설정 버튼은 지금은 여닫기만 가능한 빈 패널을 띄우며, 실제 설정 항목(사운드 조절 등)은 추후 채워질 예정이다.
    public class StartMenuController : MonoBehaviour
    {
        [Tooltip("시작하기 버튼을 눌렀을 때 로드할 게임 씬의 이름이다.")]
        [SerializeField]
        private string _gameSceneName = "GameScene";

        [Tooltip("설정 버튼을 눌렀을 때 열리는 패널이다. 씬에서 기본적으로 비활성화 상태로 두어야 한다.")]
        [SerializeField]
        private GameObject _settingsPanel;

        public void OnStartButtonClicked()
        {
            SceneManager.LoadScene(_gameSceneName);
        }

        // 설정 패널을 연다. 지금은 여닫기 기능만 있고, 실제 설정 항목은 추후 추가된다.
        public void OnSettingsButtonClicked()
        {
            _settingsPanel.SetActive(true);
        }

        // 설정 패널 안의 닫기 버튼이 호출한다.
        public void OnSettingsCloseButtonClicked()
        {
            _settingsPanel.SetActive(false);
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
