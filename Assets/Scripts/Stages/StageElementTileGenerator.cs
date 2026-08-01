// Assets/Scripts/Stages/StageElementTileGenerator.cs

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    // 설치 가능 구역 타일맵을 기준으로, 그 중 일부 비율만큼 원소 오버레이 타일을 랜덤하게 배치한다.
    // 스테이지에 입장할 때마다 호출되어 기존 배치를 지우고 다시 생성한다.
    public class StageElementTileGenerator : MonoBehaviour
    {
        [Tooltip("배치에 사용할 다섯 원소 타일 에셋 목록이다.")]
        [SerializeField]
        private List<ElementTileBase> _elementTileAssets;

        public void Generate(StageReferences stageReferences, float elementTileRatio)
        {
            Tilemap buildableTilemap = stageReferences.BuildableAreaTilemap;
            Tilemap overlayTilemap = stageReferences.ElementOverlayTilemap;

            overlayTilemap.ClearAllTiles();

            List<Vector3Int> buildableCells = FindBuildableCells(buildableTilemap);
            List<Vector3Int> selectedCells = SelectRandomCells(buildableCells, elementTileRatio);

            foreach (Vector3Int cellPosition in selectedCells)
            {
                ElementTileBase randomTile = _elementTileAssets[Random.Range(0, _elementTileAssets.Count)];
                overlayTilemap.SetTile(cellPosition, randomTile);
            }
        }

        private List<Vector3Int> FindBuildableCells(Tilemap buildableTilemap)
        {
            List<Vector3Int> cells = new List<Vector3Int>();
            BoundsInt bounds = buildableTilemap.cellBounds;

            foreach (Vector3Int cellPosition in bounds.allPositionsWithin)
            {
                if (buildableTilemap.HasTile(cellPosition))
                {
                    cells.Add(cellPosition);
                }
            }

            return cells;
        }

        private List<Vector3Int> SelectRandomCells(List<Vector3Int> candidateCells, float ratio)
        {
            List<Vector3Int> shuffledCells = new List<Vector3Int>(candidateCells);
            ShuffleList(shuffledCells);

            int selectCount = Mathf.RoundToInt(candidateCells.Count * ratio);
            int clampedCount = Mathf.Min(selectCount, shuffledCells.Count);

            return shuffledCells.GetRange(0, clampedCount);
        }

        private void ShuffleList(List<Vector3Int> list)
        {
            for (int index = list.Count - 1; index > 0; index--)
            {
                int randomIndex = Random.Range(0, index + 1);
                Vector3Int temp = list[index];
                list[index] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}
