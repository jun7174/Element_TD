// Assets/Scripts/UI/TowerShopButton.cs

using UnityEngine;

namespace ElementTD
{
    // 상점 버튼 하나가 눌리면 해당 타워의 구매 대기 상태를 시작한다.
    // 타워 7종마다 이 컴포넌트를 하나씩 붙이고, TowerPrefab만 다르게 지정해서 재사용한다.
    // 유니티 에디터에서 Button 컴포넌트의 OnClick에 OnBuyButtonClicked를 연결해서 사용한다.
    public class TowerShopButton : MonoBehaviour
    {
        [SerializeField]
        private TowerSelectionController _selectionController;

        [SerializeField]
        private TowerBase _towerPrefab;

        public void OnBuyButtonClicked()
        {
            SoundManager.instance.PlaySFX(ESfx.BUTTON_CLICK);
            _selectionController.BeginPurchase(_towerPrefab);
        }
    }
}
