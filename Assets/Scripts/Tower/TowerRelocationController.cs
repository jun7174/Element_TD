// Assets/Scripts/Towers/TowerRelocationController.cs

using UnityEngine;

namespace ElementTD
{
    // 설치된 타워를 다른 위치로 옮기는 것을 담당한다.
    // 비용은 설치비의 20 퍼센트이고, 강화 상태는 그대로 유지된다.
    public class TowerRelocationController : MonoBehaviour
    {
        private const float RelocationCostRatio = 0.2f;

        // 재배치를 시도한다. 골드가 부족하면 실패하고 false를 반환한다.
        public bool TryRelocate(TowerBase tower, Vector2 newPosition)
        {
            int cost = Mathf.RoundToInt(tower.TowerData.InstallCost * RelocationCostRatio);

            if (!EconomyManager.TrySpendGold(cost))
            {
                Debug.Log("골드가 부족해서 재배치할 수 없다");
                return false;
            }

            tower.PlaceAt(newPosition);
            return true;
        }
    }
}
