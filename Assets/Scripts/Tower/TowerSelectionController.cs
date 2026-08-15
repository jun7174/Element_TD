// Assets/Scripts/Towers/TowerSelectionController.cs

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    // 마우스 클릭으로 타워를 선택하거나, 대기 중인 구매 또는 재배치를 확정한다.
    // 왼쪽 클릭으로 확정하고, 오른쪽 클릭으로 대기 중인 동작을 취소한다.
    public class TowerSelectionController : MonoBehaviour
    {
        private const float ClickSelectRadius = 0.3f;

        [SerializeField]
        private TowerTradeController _tradeController;

        [SerializeField]
        private TowerRelocationController _relocationController;

        private Tilemap _buildableAreaTilemap;

        [SerializeField]
        private PlacementRangePreview _placementPreview;

        private TowerBase _selectedTower;
        private TowerBase _pendingPurchasePrefab;
        private bool _isPendingRelocation;

        public TowerBase SelectedTower => _selectedTower;

        // StageManager가 스테이지 로드 직후 호출해서, 설치 가능 여부 판정에 쓸 타일맵을 전달한다.
        // 스테이지 프리팹은 런타임에 인스턴스화되므로, 이 참조는 인스펙터에서 미리 연결할 수 없다.
        public void SetBuildableAreaTilemap(Tilemap buildableAreaTilemap)
        {
            _buildableAreaTilemap = buildableAreaTilemap;
        }

        private void Update()
        {
            if (_pendingPurchasePrefab != null)
            {
                UpdatePurchasePreview();
            }
            else if (_isPendingRelocation)
            {
                UpdateRelocationPreview();
            }

            if (IsPointerOverUI())
            {
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleLeftClick();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                CancelPendingAction();
            }
        }

        // 지금 마우스 포인터가 UI 요소(버튼 등) 위에 있는지 확인한다.
        // UI 위에서 일어난 클릭은 월드 클릭(선택, 배치, 재배치)으로 처리하지 않는다.
        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        // 상점 버튼이 눌렸을 때 호출한다. 다음 왼쪽 클릭에서 그 타워를 배치한다.
        public void BeginPurchase(TowerBase towerPrefab)
        {
            ClearSelection();
            _pendingPurchasePrefab = towerPrefab;
        }

        // 재배치 버튼이 눌렸을 때 호출한다. 다음 왼쪽 클릭 위치로 선택된 타워를 옮긴다.
        public void BeginRelocation()
        {
            if (_selectedTower == null)
            {
                return;
            }

            _isPendingRelocation = true;
        }

        public void ClearSelection()
        {
            SetIndicatorActive(_selectedTower, false);
            _selectedTower = null;
        }

        private void HandleLeftClick()
        {
            Vector2 worldPosition = GetMouseWorldPosition();

            if (_pendingPurchasePrefab != null)
            {
                ConfirmPurchase(worldPosition);
                return;
            }

            if (_isPendingRelocation)
            {
                ConfirmRelocation(worldPosition);
                return;
            }

            TowerBase clickedTower = FindTowerNear(worldPosition);

            if (clickedTower != null)
            {
                Select(clickedTower);
            }
            else
            {
                ClearSelection();
            }
        }

        private void CancelPendingAction()
        {
            _pendingPurchasePrefab = null;
            _isPendingRelocation = false;
            _placementPreview.Hide();
        }

        private void ConfirmPurchase(Vector2 worldPosition)
        {
            if (!IsBuildable(worldPosition))
            {
                Debug.Log("설치 가능한 칸이 아니다");
                return;
            }

            _tradeController.TryPurchaseAndPlace(_pendingPurchasePrefab, worldPosition);
            _pendingPurchasePrefab = null;
            _placementPreview.Hide();
        }

        private void ConfirmRelocation(Vector2 worldPosition)
        {
            if (!IsBuildable(worldPosition))
            {
                Debug.Log("설치 가능한 칸이 아니다");
                return;
            }

            _relocationController.TryRelocate(_selectedTower, worldPosition);
            _isPendingRelocation = false;
            _placementPreview.Hide();
        }

        private bool IsBuildable(Vector2 worldPosition)
        {
            Vector3Int cellPosition = _buildableAreaTilemap.WorldToCell(worldPosition);
            return _buildableAreaTilemap.HasTile(cellPosition);
        }

        private void UpdatePurchasePreview()
        {
            Vector2 worldPosition = GetMouseWorldPosition();
            float radius = GetTowerDisplayRadius(_pendingPurchasePrefab.TowerData);
            _placementPreview.Show(worldPosition, radius);
        }

        private void UpdateRelocationPreview()
        {
            Vector2 worldPosition = GetMouseWorldPosition();
            float radius = GetTowerDisplayRadius(_selectedTower.TowerData);
            _placementPreview.Show(worldPosition, radius);
        }

        private float GetTowerDisplayRadius(TowerDataSO towerData)
        {
            if (towerData.Type == TowerType.Combat)
            {
                return towerData.AttackRange;
            }

            if (towerData.Type == TowerType.Support)
            {
                return towerData.EffectRadius;
            }

            return 0f;
        }

        private TowerBase FindTowerNear(Vector2 worldPosition)
        {
            foreach (TowerBase tower in TowerBase.ActiveTowers)
            {
                float distance = Vector2.Distance(worldPosition, tower.transform.position);

                if (distance <= ClickSelectRadius)
                {
                    return tower;
                }
            }

            return null;
        }

        private void Select(TowerBase tower)
        {
            SetIndicatorActive(_selectedTower, false);
            _selectedTower = tower;
            SetIndicatorActive(_selectedTower, true);
        }

        private void SetIndicatorActive(TowerBase tower, bool isActive)
        {
            if (tower == null)
            {
                return;
            }

            TowerRangeIndicator indicator = tower.GetComponent<TowerRangeIndicator>();

            if (indicator != null)
            {
                indicator.SetActiveDisplay(isActive);
            }
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector3 screenPosition = Mouse.current.position.ReadValue();
            screenPosition.z = -Camera.main.transform.position.z;

            return Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }
}
