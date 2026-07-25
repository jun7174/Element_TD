using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    public class TileMapManager : MonoBehaviour
    {
        [SerializeField]
        CustomTileBase[] customTileBase;

        [SerializeField]
        Sprite[] sprites;

        [SerializeField]
        CustomTileBase selectTile;

        public Tilemap tilemap;
        

        private void Start()
        {
            //tilemap = GetComponent<Tilemap>(); TODO : 추후 삭제
        }

        public void SpriteSet(Vector3 Pos)
        {
            Debug.Log("Set진입");
            Vector3Int cellPosititon = tilemap.WorldToCell(Pos);
            tilemap.SetTile(cellPosititon, customTileBase[0]);
            Debug.Log(tilemap.GetTile(cellPosititon));
            selectTile = (CustomTileBase)tilemap.GetTile(cellPosititon);
            ElementType type = selectTile.GetElementType();

            Debug.Log(selectTile);
            Debug.Log(type);

            switch (type)
            {
                case ElementType.Fire:
                    tilemap.SetColor(cellPosititon, Color.red);
                    break;
                case ElementType.Grass:
                    tilemap.SetColor(cellPosititon, Color.green);
                    break;
                case ElementType.Water:
                    tilemap.SetColor(cellPosititon, Color.blue);
                    break;
                case ElementType.Dark:
                    tilemap.SetColor(cellPosititon, Color.black);
                    break;
                case ElementType.Light:
                    tilemap.SetColor(cellPosititon, Color.yellow);
                    break;
                default:
                    break;
            }
            

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                SpriteSet(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

        }
    }
}