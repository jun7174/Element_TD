using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    public class ElementTile : MonoBehaviour
    {
        public Tilemap tilemap;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) MakeDot();
        }
        void MakeDot()
        {
            // 마우스 클릭 위치의 그리드 좌표 계산
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            // 해당 위치에 타일이 있는지 확인
            TileBase tileBase = tilemap.GetTile(cellPosition);

            // 타일이 커스텀 타일인지 확인 및 데이터 읽기
            if (tileBase != null && tileBase is CustomTile customTile)
            {
                Debug.Log($"타일 속성: {customTile.GetTileElementType()}");
                customTile.ElementChange();
            }

        }

    }
}