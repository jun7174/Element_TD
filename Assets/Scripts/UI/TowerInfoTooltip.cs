// Assets/Scripts/UI/TowerInfoTooltip.cs

using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 선택된 타워의 이름, 강화 상태, 현재 받고 있는 원소 효과를 보여준다.
    // 항상 선택된 타워 바로 위에 화면 좌표로 위치가 갱신된다.
    public class TowerInfoTooltip : MonoBehaviour
    {
        [SerializeField]
        private TowerSelectionController _selectionController;

        [SerializeField]
        private GameObject _panelRoot;

        [SerializeField]
        private TextMeshProUGUI _infoText;

        [Tooltip("타워 위치보다 얼마나 위쪽(픽셀)에 표시할지이다. 액션패널보다 더 위에 뜨도록 큰 값을 쓴다.")]
        [SerializeField]
        private float _verticalOffset = 140f;

        private RectTransform _panelRectTransform;

        private void Awake()
        {
            _panelRectTransform = _panelRoot.GetComponent<RectTransform>();
        }

        private void Update()
        {
            TowerBase selectedTower = _selectionController.SelectedTower;

            if (selectedTower == null)
            {
                _panelRoot.SetActive(false);
                return;
            }

            _panelRoot.SetActive(true);
            _infoText.text = BuildInfoText(selectedTower);
            WorldToScreenUIPositioner.PositionAboveWorldPoint(_panelRectTransform, selectedTower.transform.position, _verticalOffset);
        }

        private string BuildInfoText(TowerBase tower)
        {
            string upgradeLabel = GetUpgradeStateLabel(tower.UpgradeState);
            string effectText = GetCurrentEffectText(tower);

            return tower.TowerData.TowerName + "\n"
                + "강화 상태 " + upgradeLabel + "\n"
                + effectText;
        }

        private string GetCurrentEffectText(TowerBase tower)
        {
            ElementEffectSO effect = tower.GetCurrentElementEffect();

            if (effect == null)
            {
                return "원소 효과 없음";
            }

            if (effect is StatModifierEffectSO statModifierEffect)
            {
                string statLabel = GetStatLabel(statModifierEffect.TargetStat);
                return statLabel + " 증가 " + statModifierEffect.ModifierValue;
            }

            return effect.EffectName;
        }

        private string GetStatLabel(TargetStat stat)
        {
            switch (stat)
            {
                case TargetStat.AttackSpeed:
                    return "공격속도";
                case TargetStat.Damage:
                    return "데미지";
                case TargetStat.ArmorPenetration:
                    return "방어관통";
                case TargetStat.Range:
                    return "사거리";
                case TargetStat.CritChance:
                    return "치명타 확률";
                case TargetStat.CoreValue:
                    return "핵심 수치";
                default:
                    return stat.ToString();
            }
        }

        private string GetUpgradeStateLabel(TowerUpgradeState state)
        {
            switch (state)
            {
                case TowerUpgradeState.Base:
                    return "기본";
                case TowerUpgradeState.First:
                    return "1강";
                case TowerUpgradeState.Second:
                    return "2강";
                default:
                    return state.ToString();
            }
        }
    }
}
