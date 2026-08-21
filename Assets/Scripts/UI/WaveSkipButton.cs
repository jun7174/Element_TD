// Assets/Scripts/UI/WaveSkipButton.cs

using UnityEngine;

namespace ElementTD
{
    // 웨이브 사이 대기시간을 건너뛰는 버튼의 동작을 담당한다.
    public class WaveSkipButton : MonoBehaviour
    {
        [SerializeField]
        private WaveSpawner _waveSpawner;

        public void OnSkipButtonClicked()
        {
            _waveSpawner.SkipWaveDelay();
        }
    }
}
