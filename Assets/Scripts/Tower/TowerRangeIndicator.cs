// Assets/Scripts/Towers/TowerRangeIndicator.cs

using UnityEngine;

namespace ElementTD
{
    // 타워의 사거리 또는 효과 반경을 원으로 시각화한다.
    // 전투형은 원소 효과가 반영된 유효 사거리를, 지원형은 효과 반경을 보여준다.
    // 경제형 타워에는 이 컴포넌트를 붙이지 않는다.
    [RequireComponent(typeof(TowerBase))]
    [RequireComponent(typeof(LineRenderer))]
    public class TowerRangeIndicator : MonoBehaviour
    {
        [SerializeField]
        private RangeIndicatorSettingsSO _settings;

        private TowerBase _towerBase;
        private TowerCombatController _combatController;
        private LineRenderer _lineRenderer;
        private bool _isActiveDisplay;

        private void Awake()
        {
            _towerBase = GetComponent<TowerBase>();
            _combatController = GetComponent<TowerCombatController>();
            _lineRenderer = GetComponent<LineRenderer>();
            RangeCircleDrawer.Configure(_lineRenderer, _settings);
        }

        private void Update()
        {
            float radius = GetDisplayRadius();

            if (radius <= 0f)
            {
                _lineRenderer.enabled = false;
                return;
            }

            _lineRenderer.enabled = !(_settings.HideWhenIdle && !_isActiveDisplay);

            if (!_lineRenderer.enabled)
            {
                return;
            }

            ApplyAlpha();
            RangeCircleDrawer.Draw(_lineRenderer, radius);
        }

        // 선택 중이거나 배치 미리보기 중임을 알려서 진하게 표시되게 한다.
        public void SetActiveDisplay(bool isActive)
        {
            _isActiveDisplay = isActive;
        }

        private float GetDisplayRadius()
        {
            if (_towerBase.TowerData.Type == TowerType.Combat && _combatController != null)
            {
                return _combatController.GetEffectiveRange();
            }

            if (_towerBase.TowerData.Type == TowerType.Support)
            {
                return _towerBase.TowerData.EffectRadius;
            }

            return 0f;
        }

        private void ApplyAlpha()
        {
            float alpha = _isActiveDisplay ? _settings.ActiveAlpha : _settings.IdleAlpha;
            Color lineColor = _settings.LineColor;
            lineColor.a = alpha;

            _lineRenderer.startColor = lineColor;
            _lineRenderer.endColor = lineColor;
        }
    }
}
