using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    [CreateAssetMenu(fileName = "NewCustomTile", menuName = "2D/Tiles/Custom Tile")]
    public class CustomTile : Tile
    {
        [SerializeField]
        public ElementType elementType;

        // 타일맵에 타일이 배치될 때 호출되는 로직 (데이터 추가나 초기화 등)
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);

            // 필요에 따라 타일의 색상, 스프라이트 등을 동적으로 변경 가능
            switch (elementType)
            {
                case ElementType.Fire:
                    tileData.color = Color.red;
                    break;
                case ElementType.Water:
                    tileData.color = Color.blue;
                    break;
                case ElementType.Grass:
                    tileData.color = Color.green;
                    break;
                default:
                    break;
            }
            
        }

        void TileColor(ref TileData tileData)
        {
            switch (elementType)
            {
                case ElementType.Fire:
                    tileData.color = Color.red;
                    break;
                case ElementType.Water:
                    tileData.color = Color.blue;
                    break;
                case ElementType.Grass:
                    tileData.color = Color.green;
                    break;
                default:
                    break;
            }
        }
        
        public ElementType GetTileElementType()
        {
            return elementType;
        }

        public void ElementChange()
        {
            switch (elementType)
            {
                case ElementType.Water:
                    elementType = ElementType.Grass;
                    break;
                case ElementType.Grass:
                    elementType = ElementType.Fire;
                    break;
                case ElementType.Fire:
                    elementType = ElementType.Water;
                    break;
                case ElementType.Light:
                    elementType = ElementType.Dark;
                    break;
                case ElementType.Dark:
                    elementType = ElementType.Light;
                    break;
            }
            Debug.Log("속성 전환 : "+ elementType);

            
        }
    }
}