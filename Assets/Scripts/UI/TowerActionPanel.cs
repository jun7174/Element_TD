// Assets/Scripts/UI/TowerActionPanel.cs

using UnityEngine;
using UnityEngine.UI;

namespace ElementTD
{
    // 선택된 타워에 대해 강화, 판매, 재배치 버튼을 제공한다.
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

        private void Awake()
        {
            _upgradeButton.onClick.AddListener(OnUpgradeClicked);
            _sellButton.onClick.AddListener(OnSellClicked);
            _relocateButton.onClick.AddListener(OnRelocateClicked);
        }

        private void Update()
        {
            _panelRoot.SetActive(_selectionController.SelectedTower != null);
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
