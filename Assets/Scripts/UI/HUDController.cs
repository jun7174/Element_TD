// Assets/Scripts/UI/HUDController.cs

using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 골드, 생명력, 진행 시간, 웨이브 진행도를 화면에 실시간으로 표시한다.
    public class HUDController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _goldText;

        [SerializeField]
        private TextMeshProUGUI _healthText;

        [SerializeField]
        private TextMeshProUGUI _timeText;

        [SerializeField]
        private TextMeshProUGUI _waveText;

        [SerializeField]
        private WaveSpawner _waveSpawner;

        private void Update()
        {
            _goldText.text = "" + EconomyManager.CurrentGold;
            _healthText.text = ""+PlayerHealth.CurrentHealth;
            _timeText.text = FormatTime(GameTimer.ElapsedSeconds);
            _waveText.text = BuildWaveProgressText();
        }

        private string FormatTime(float totalSeconds)
        {
            int minutes = Mathf.FloorToInt(totalSeconds / 60f);
            int seconds = Mathf.FloorToInt(totalSeconds % 60f);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private string BuildWaveProgressText()
        {
            StageDataSO stageData = _waveSpawner.CurrentStageData;

            if (stageData == null)
            {
                return "웨이브 대기 중";
            }

            int currentWaveNumber = _waveSpawner.CurrentWaveIndex + 1;
            int totalWaveCount = stageData.WaveSequence.Count;

            return currentWaveNumber + " / " + totalWaveCount + " 웨이브";
        }
    }
}
