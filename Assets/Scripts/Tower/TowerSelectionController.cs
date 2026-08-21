// Assets/Scripts/Towers/TowerSelectionController.cs

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    // 마우스 클릭으로 타워를 선택하거나, 대기 중인 구매 또는 재배치를 확정한다.
    // 왼쪽 클릭으로 확정하고, 오른쪽 클릭이나 잘못된 칸 클릭(설치 불가 구역, 중복,
    // 재배치의 경우 현재 위치와 동일한 칸)으로 대기 중인 동작을 취소한다.
    // 확정된 위치는 항상 타일 중앙으로 스냅된다.
    public class TowerSelectionController : MonoBehaviour
    {
        private const float ClickSelectRadius = 0.3f;

        [SerializeField]
        private TowerTradeController _tradeController;

        [SerializeField]
        private TowerRelocationController _relocationController;

        [SerializeField]
        private PlacementRangePreview _placementPreview;

        [SerializeField]
        private TowerPlacementSpritePreview _spritePreview;

        private Tilemap _buildableAreaTilemap;
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
            _spritePreview.Hide();
        }

        private void ConfirmPurchase(Vector2 worldPosition)
        {
            Vector3Int cellPosition = _buildableAreaTilemap.WorldToCell(worldPosition);

            if (!IsValidPlacementCell(cellPosition, null))
            {
                Debug.Log("설치할 수 없는 칸이라 구매를 취소한다");
                CancelPendingAction();
                return;
            }

            Vector3 centerPosition = _buildableAreaTilemap.GetCellCenterWorld(cellPosition);
            _tradeController.TryPurchaseAndPlace(_pendingPurchasePrefab, centerPosition);
            _pendingPurchasePrefab = null;
            _placementPreview.Hide();
            _spritePreview.Hide();
        }

        private void ConfirmRelocation(Vector2 worldPosition)
        {
            Vector3Int cellPosition = _buildableAreaTilemap.WorldToCell(worldPosition);
            Vector3Int currentCell = _buildableAreaTilemap.WorldToCell(_selectedTower.transform.position);

            if (cellPosition == currentCell)
            {
                Debug.Log("현재 위치와 같은 칸이라 재배치를 취소한다");
                CancelPendingAction();
                return;
            }

            if (!IsValidPlacementCell(cellPosition, _selectedTower))
            {
                Debug.Log("재배치할 수 없는 칸이라 재배치를 취소한다");
                CancelPendingAction();
                return;
            }

            Vector3 centerPosition = _buildableAreaTilemap.GetCellCenterWorld(cellPosition);
            _relocationController.TryRelocate(_selectedTower, centerPosition);
            _isPendingRelocation = false;
            _placementPreview.Hide();
            _spritePreview.Hide();
        }

        // 설치 가능 구역이면서 다른 타워가 없는 칸인지 확인한다.
        // excludeTower로 넘긴 타워는 중복 판정에서 제외한다 (재배치 시 자기 자신 제외용).
        private bool IsValidPlacementCell(Vector3Int cellPosition, TowerBase excludeTower)
        {
            if (!_buildableAreaTilemap.HasTile(cellPosition))
            {
                return false;
            }

            if (IsCellOccupied(cellPosition, excludeTower))
            {
                return false;
            }

            return true;
        }

        private bool IsCellOccupied(Vector3Int cellPosition, TowerBase excludeTower)
        {
            foreach (TowerBase tower in TowerBase.ActiveTowers)
            {
                if (tower == excludeTower)
                {
                    continue;
                }

                Vector3Int towerCell = _buildableAreaTilemap.WorldToCell(tower.transform.position);

                if (towerCell == cellPosition)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdatePurchasePreview()
        {
            Vector2 worldPosition = GetMouseWorldPosition();
            Vector3Int cellPosition = _buildableAreaTilemap.WorldToCell(worldPosition);
            Vector3 centerPosition = _buildableAreaTilemap.GetCellCenterWorld(cellPosition);
            float radius = GetTowerDisplayRadius(_pendingPurchasePrefab.TowerData);
            bool isValid = IsValidPlacementCell(cellPosition, null);

            _placementPreview.Show(centerPosition, radius);
            _spritePreview.Show(centerPosition, GetTowerSprite(_pendingPurchasePrefab), isValid);
        }

        private void UpdateRelocationPreview()
        {
            Vector2 worldPosition = GetMouseWorldPosition();
            Vector3Int cellPosition = _buildableAreaTilemap.WorldToCell(worldPosition);
            Vector3 centerPosition = _buildableAreaTilemap.GetCellCenterWorld(cellPosition);
            Vector3Int currentCell = _buildableAreaTilemap.WorldToCell(_selectedTower.transform.position);
            float radius = GetTowerDisplayRadius(_selectedTower.TowerData);
            bool isValid = cellPosition != currentCell && IsValidPlacementCell(cellPosition, _selectedTower);

            _placementPreview.Show(centerPosition, radius);
            _spritePreview.Show(centerPosition, GetTowerSprite(_selectedTower), isValid);
        }

        private Sprite GetTowerSprite(TowerBase tower)
        {
            SpriteRenderer spriteRenderer = tower.GetComponent<SpriteRenderer>();
            return spriteRenderer != null ? spriteRenderer.sprite : null;
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
