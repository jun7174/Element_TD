// Assets/Scripts/Tiles/ElementTileBase.cs

using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    // 원소 오버레이 타일맵에 배치되는 커스텀 타일이다.
    // Tile을 상속해서 기존 타일 기능(스프라이트 표시)은 그대로 쓰면서 원소 정보를 추가로 담는다.
    [CreateAssetMenu(fileName = "NewElementTile", menuName = "ElementTD/Element Tile")]
    public class ElementTileBase : Tile
    {
        public ElementType Element;
    }
}
