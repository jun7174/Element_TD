// Assets/Scripts/UI/TowerActionPanel.cs

using UnityEngine;
using UnityEngine.UI;

namespace ElementTD
{
    // 선택된 타워에 대해 강화, 판매, 재배치 버튼을 제공한다.
    // 항상 선택된 타워 바로 위에 화면 좌표로 위치가 갱신되며, 툴팁보다는 타워에 더 가깝게 표시된다.
    public class TowerActionPanel : MonoBehaviour
    {
        [SerializeField]
        private TowerSelectionController _selectionController;

        [SerializeField]
        private TowerUpgradeController _upgradeController;

        [SerializeField]
        private TowerTradeController _tradeController;

        [SerializeField]
        private GameObject _panelRoot;

        [SerializeField]
        private Button _upgradeButton;

        [SerializeField]
        private Button _sellButton;

        [SerializeField]
        private Button _relocateButton;

        [Tooltip("타워 위치보다 얼마나 위쪽(픽셀)에 표시할지이다. 툴팁보다 타워에 더 가깝도록 작은 값을 쓴다.")]
        [SerializeField]
        private float _verticalOffset = 60f;

        private RectTransform _panelRectTransform;

        private void Awake()
        {
            _panelRectTransform = _panelRoot.GetComponent<RectTransform>();
            _upgradeButton.onClick.AddListener(OnUpgradeClicked);
            _sellButton.onClick.AddListener(OnSellClicked);
            _relocateButton.onClick.AddListener(OnRelocateClicked);
        }

        private void Update()
        {
            TowerBase selectedTower = _selectionController.SelectedTower;
            _panelRoot.SetActive(selectedTower != null);

            if (selectedTower != null)
            {
                WorldToScreenUIPositioner.PositionAboveWorldPoint(_panelRectTransform, selectedTower.transform.position, _verticalOffset);
            }
        }

        private void OnUpgradeClicked()
        {
            TowerBase selectedTower = _selectionController.SelectedTower;

            if (selectedTower != null)
            {
                _upgradeController.TryUpgrade(selectedTower);
            }
        }

        private void OnSellClicked()
        {
            TowerBase selectedTower = _selectionController.SelectedTower;

            if (selectedTower != null)
            {
                _tradeController.SellTower(selectedTower);
                _selectionController.ClearSelection();
            }
        }

        private void OnRelocateClicked()
        {
            _selectionController.BeginRelocation();
        }
    }
}
