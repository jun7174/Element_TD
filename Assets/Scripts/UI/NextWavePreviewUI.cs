// Assets/Scripts/UI/NextWavePreviewUI.cs

using System.Text;
using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 다음 웨이브에 등장할 몬스터의 종류와 수량을 미리 보여준다.
    public class NextWavePreviewUI : MonoBehaviour
    {
        [SerializeField]
        private WaveSpawner _waveSpawner;

        [SerializeField]
        private TextMeshProUGUI _previewText;

        private void Update()
        {
            UpdatePreviewText();
        }

        private void UpdatePreviewText()
        {
            StageDataSO stageData = _waveSpawner.CurrentStageData;

            if (stageData == null)
            {
                _previewText.text = string.Empty;
                return;
            }

            int nextWaveIndex = _waveSpawner.CurrentWaveIndex + 1;

            if (nextWaveIndex >= stageData.WaveSequence.Count)
            {
                _previewText.text = "다음 웨이브 없음";
                return;
            }

            _previewText.text = BuildPreviewText(stageData.WaveSequence[nextWaveIndex]);
        }

        private string BuildPreviewText(WaveData nextWave)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("다음 웨이브\n");

            foreach (WaveMonsterEntry entry in nextWave.MonsterEntries)
            {
                builder.Append(entry.Monster.name);
                builder.Append(" x");
                builder.Append(entry.Count);
                builder.Append("\n");
            }

            return builder.ToString();
        }
    }
}
