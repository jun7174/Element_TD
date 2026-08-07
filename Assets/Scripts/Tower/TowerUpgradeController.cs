// Assets/Scripts/Towers/TowerUpgradeController.cs

using UnityEngine;

namespace ElementTD
{
    // 타워를 다음 강화 단계로 올리는 것을 담당한다.
    // 비용을 확인하고 차감한 뒤, 대상 타워의 강화 상태를 변경한다.
    public class TowerUpgradeController : MonoBehaviour
    {
        // 강화를 시도한다. 이미 최대 강화 상태이거나 골드가 부족하면 실패하고 false를 반환한다.
        public bool TryUpgrade(TowerBase tower)
        {
            TowerUpgradeState nextState = GetNextState(tower.UpgradeState);

            if (nextState == tower.UpgradeState)
            {
                Debug.Log("이미 최대 강화 상태이다");
                return false;
            }

            TowerUpgradeLevelData levelData = GetLevelData(tower.TowerData, nextState);
            int cost = Mathf.RoundToInt(tower.TowerData.InstallCost * levelData.CostRatio);

            if (!EconomyManager.TrySpendGold(cost))
            {
                Debug.Log("골드가 부족해서 강화할 수 없다");
                return false;
            }

            tower.SetUpgradeState(nextState);
            Debug.Log(tower.TowerData.TowerName + " 강화 완료, 상태 " + nextState);
            return true;
        }

        private TowerUpgradeState GetNextState(TowerUpgradeState currentState)
        {
            if (currentState == TowerUpgradeState.Base)
            {
                return TowerUpgradeState.First;
            }

            if (currentState == TowerUpgradeState.First)
            {
                return TowerUpgradeState.Second;
            }

            return TowerUpgradeState.Second;
        }

        private TowerUpgradeLevelData GetLevelData(TowerDataSO towerData, TowerUpgradeState state)
        {
            if (state == TowerUpgradeState.First)
            {
                return towerData.UpgradeData.FirstUpgrade;
            }

            return towerData.UpgradeData.SecondUpgrade;
        }
    }
}
