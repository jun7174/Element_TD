using UnityEngine;
using UnityEngine.Tilemaps;


    public class Bricks : MonoBehaviour
    {
        public Tilemap tilemap;
        //public CustomTileBase customTileBase;
        //public TileMapManager tileMapManager;
        private void Start()
        {
            tilemap = GetComponent<Tilemap>();
        }

        public void MakeDot(Vector3 Pos)
        {
            Debug.Log("DotDot");
            Vector3Int cellPosititon = tilemap.WorldToCell(Pos);

            //tilemap.SetTile(cellPosititon, null);
        }
    }
