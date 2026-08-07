// Assets/Scripts/Towers/TowerTradeController.cs

using UnityEngine;

namespace ElementTD
{
    // 타워 설치(구매)와 판매를 담당한다.
    // TowerBase는 돈에 대해 알 필요가 없도록, 거래 절차를 별도 클래스로 분리했다.
    public class TowerTradeController : MonoBehaviour
    {
        [Tooltip("판매 시 설치비 대비 환급 비율이다. 0.85는 85 퍼센트를 의미한다.")]
        [SerializeField]
        private float _sellRefundRatio = 0.85f;

        // 설치비를 확인하고 차감한 뒤 타워를 생성해서 배치한다.
        // 골드가 부족하면 아무것도 생성하지 않고 null을 반환한다.
        public TowerBase TryPurchaseAndPlace(TowerBase towerPrefab, Vector2 position)
        {
            int cost = towerPrefab.TowerData.InstallCost;

            if (!EconomyManager.TrySpendGold(cost))
            {
                Debug.Log("골드가 부족해서 설치할 수 없다");
                return null;
            }

            TowerBase newTower = Instantiate(towerPrefab);
            newTower.PlaceAt(position);
            return newTower;
        }

        // 타워를 판매한다. 설치비의 일정 비율만큼 골드를 돌려주고 타워를 제거한다.
        public void SellTower(TowerBase tower)
        {
            int refund = Mathf.RoundToInt(tower.TowerData.InstallCost * _sellRefundRatio);
            EconomyManager.AddGold(refund);
            Destroy(tower.gameObject);
        }
    }
}
