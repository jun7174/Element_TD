// Assets/Scripts/Tiles/ElementTileQuery.cs

using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    // 현재 스테이지의 원소 오버레이 타일맵을 참조하고, 좌표로 원소를 조회하는 API를 제공한다.
    // StageManager가 스테이지를 로드할 때 CurrentOverlayTilemap을 설정한다.
    public static class ElementTileQuery
    {
        public static Tilemap CurrentOverlayTilemap;

        // 주어진 월드 좌표에 있는 타일의 원소를 반환한다.
        // 오버레이 타일맵이 설정되지 않았거나 그 좌표에 타일이 없으면 무속성을 반환한다.
        public static ElementType GetElementAt(Vector2 worldPosition)
        {
            if (CurrentOverlayTilemap == null)
            {
                return ElementType.None;
            }

            Vector3Int cellPosition = CurrentOverlayTilemap.WorldToCell(worldPosition);
            ElementTileBase tile = CurrentOverlayTilemap.GetTile<ElementTileBase>(cellPosition);

            if (tile == null)
            {
                return ElementType.None;
            }

            return tile.Element;
        }
    }
}
