// Assets/Scripts/Test/TestEconomyTrigger.cs

using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementTD
{
    // Phase 4 검증용 임시 테스트 스크립트이다.
    // 왼쪽 클릭으로 구매 위치, 오른쪽 클릭으로 재배치 위치를 지정하고,
    // 숫자 키로 구매, 강화, 재배치, 판매를 각각 트리거한다.
    // 실제 UI가 완성되면 삭제한다.
    public class TestEconomyTrigger : MonoBehaviour
    {
        [SerializeField]
        private TowerTradeController _tradeController;

        [SerializeField]
        private TowerUpgradeController _upgradeController;

        [SerializeField]
        private TowerRelocationController _relocationController;

        [SerializeField]
        private TowerBase _towerPrefabToPurchase;

        [Tooltip("왼쪽 클릭 시 자동으로 갱신된다. 직접 입력해도 다음 클릭 전까지는 유지된다.")]
        [SerializeField]
        private Vector2 _purchasePosition;

        [Tooltip("오른쪽 클릭 시 자동으로 갱신된다. 직접 입력해도 다음 클릭 전까지는 유지된다.")]
        [SerializeField]
        private Vector2 _relocationPosition;

        private TowerBase _lastPurchasedTower;

        private void Update()
        {
            UpdatePositionsFromMouse();

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _lastPurchasedTower = _tradeController.TryPurchaseAndPlace(_towerPrefabToPurchase, _purchasePosition);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame && _lastPurchasedTower != null)
            {
                _upgradeController.TryUpgrade(_lastPurchasedTower);
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame && _lastPurchasedTower != null)
            {
                _relocationController.TryRelocate(_lastPurchasedTower, _relocationPosition);
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame && _lastPurchasedTower != null)
            {
                _tradeController.SellTower(_lastPurchasedTower);
                _lastPurchasedTower = null;
            }
        }

        // 왼쪽 클릭은 구매 위치, 오른쪽 클릭은 재배치 위치를 마우스가 가리키는 좌표로 갱신한다.
        private void UpdatePositionsFromMouse()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _purchasePosition = GetMouseWorldPosition();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _relocationPosition = GetMouseWorldPosition();
            }
        }

        // 마우스 화면 좌표를 월드 좌표로 변환한다.
        // 카메라와 오브젝트 사이의 거리를 그대로 넘겨줘서, 2D 오소그래픽 카메라 기준으로 정확한 평면 위치를 구한다.
        private Vector2 GetMouseWorldPosition()
        {
            Vector3 screenPosition = Mouse.current.position.ReadValue();
            screenPosition.z = -Camera.main.transform.position.z;

            return Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }
}
