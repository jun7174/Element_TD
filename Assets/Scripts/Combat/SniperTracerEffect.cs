// Assets/Scripts/Combat/SniperTracerEffect.cs

using UnityEngine;

namespace ElementTD
{
    // 저격형 타워의 즉시 명중(히트스캔)을 시각적으로 보여주는 짧은 선이다.
    // 발사 지점에서 명중 지점까지 순간적으로 선을 그렸다가 아주 짧은 시간 뒤 사라진다.
    [RequireComponent(typeof(LineRenderer))]
    public class SniperTracerEffect : MonoBehaviour
    {
        [Tooltip("선이 화면에 보이는 시간이다. 단위는 초이다.")]
        [SerializeField]
        private float _lifetimeSeconds = 0.1f;

        [Tooltip("트레이서 선의 굵기이다.")]
        [SerializeField]
        private float _lineWidth = 0.05f;

        private LineRenderer _lineRenderer;
        private float _elapsedTime;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
        }

        // 시작점(타워 위치)과 끝점(명중 위치)을 지정해서 선을 그린다.
        public void Show(Vector3 startPosition, Vector3 endPosition)
        {
            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);
            SoundManager.instance.PlaySFX(ESfx.SNIPER_ATK);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _lifetimeSeconds)
            {
                Destroy(gameObject);
            }
        }
    }
}
