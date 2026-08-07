// Assets/Scripts/Towers/PlacementRangePreview.cs

using UnityEngine;

namespace ElementTD
{
    // 타워 구매 또는 재배치 대기 상태에서 마우스 위치에 사거리 미리보기 원을 그린다.
    // 배치 전이라 원소 타일 효과는 반영하지 않고 기본 사거리 또는 효과 반경만 보여준다.
    [RequireComponent(typeof(LineRenderer))]
    public class PlacementRangePreview : MonoBehaviour
    {
        [SerializeField]
        private RangeIndicatorSettingsSO _settings;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            RangeCircleDrawer.Configure(_lineRenderer, _settings);
            _lineRenderer.enabled = false;
        }

        // 미리보기를 켜고 주어진 위치와 반경으로 원을 그린다.
        public void Show(Vector2 worldPosition, float radius)
        {
            if (radius <= 0f)
            {
                Hide();
                return;
            }

            transform.position = worldPosition;
            _lineRenderer.enabled = true;

            Color lineColor = _settings.LineColor;
            lineColor.a = _settings.ActiveAlpha;
            _lineRenderer.startColor = lineColor;
            _lineRenderer.endColor = lineColor;

            RangeCircleDrawer.Draw(_lineRenderer, radius);
        }

        public void Hide()
        {
            _lineRenderer.enabled = false;
        }
    }
}
