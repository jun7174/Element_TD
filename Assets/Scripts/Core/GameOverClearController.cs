// Assets/Scripts/Core/GameOverClearController.cs

using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 게임오버와 클리어 조건을 매 프레임 확인하고, 조건이 충족되면 일시정지하고 결과를 표시한다.
    // 클리어 조건: 마지막 웨이브까지 스폰이 끝났고 생존 몬스터가 없음
    // 게임오버 조건: 플레이어 생명력이 0 이하
    public class GameOverClearController : MonoBehaviour
    {
        [SerializeField]
        private WaveSpawner _waveSpawner;

        [SerializeField]
        private GameObject _resultPanel;

        [SerializeField]
        private TextMeshProUGUI _resultText;

        private bool _isGameEnded;

        // 스테이지 시작 시 StageManager가 호출해서 결과 표시 상태를 초기화한다.
        public void ResetGameEndState()
        {
            _isGameEnded = false;
            _resultPanel.SetActive(false);
        }

        private void Update()
        {
            if (_isGameEnded)
            {
                return;
            }

            if (PlayerHealth.CurrentHealth <= 0)
            {
                EndGame("게임오버");
                return;
            }

            if (IsClearConditionMet())
            {
                EndGame("클리어");
            }
        }

        private bool IsClearConditionMet()
        {
            return _waveSpawner.IsSpawningComplete && MonsterBase.ActiveMonsters.Count == 0;
        }

        private void EndGame(string resultMessage)
        {
            _isGameEnded = true;
            GameSpeedManager.Pause();
            _resultPanel.SetActive(true);
            _resultText.text = resultMessage;
        }
    }
}
