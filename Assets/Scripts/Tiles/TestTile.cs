using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTile : MonoBehaviour
{
    private Tilemap tilemap;
    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void MakeDot(Vector3 Pos)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(Pos);
        tilemap.SetTile(cellPosition, null);
    }
}
