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

            AssignElementTiles(overlayTilemap, selectedCells);
        }

        // 선택된 칸 앞쪽에 5원소를 하나씩 겹치지 않게 먼저 배정하고(선택된 칸이 5개 미만이면 그 개수만큼만),
        // 남는 칸은 기존처럼 완전히 랜덤한 원소를 배정한다.
        // selectedCells 자체가 이미 셔플되어 있으므로, "앞쪽 칸"이 매번 다른 위치가 된다.
        private void AssignElementTiles(Tilemap overlayTilemap, List<Vector3Int> selectedCells)
        {
            List<ElementTileBase> shuffledElements = new List<ElementTileBase>(_elementTileAssets);
            ShuffleList(shuffledElements);

            int guaranteedCount = Mathf.Min(selectedCells.Count, shuffledElements.Count);

            // 임시 디버그 로그: 실제 배정 대상 칸 수와 원소 종류 수를 확인한다. 확인 후 삭제할 것.
            Debug.Log("[원소배치 디버그] 배정 대상 칸 수: " + selectedCells.Count + ", 원소 종류 수: " + _elementTileAssets.Count + ", 보장 배정 수: " + guaranteedCount);

            for (int index = 0; index < selectedCells.Count; index++)
            {
                ElementTileBase tile = index < guaranteedCount
                    ? shuffledElements[index]
                    : _elementTileAssets[Random.Range(0, _elementTileAssets.Count)];

                overlayTilemap.SetTile(selectedCells[index], tile);

                // 임시 디버그 로그: 보장 배정 구간(index < guaranteedCount)에서 어떤 원소가 배정됐는지 확인한다. 확인 후 삭제할 것.
                if (index < guaranteedCount)
                {
                    Debug.Log("[원소배치 디버그] 보장 배정 " + index + "번째 칸 " + selectedCells[index] + " → " + tile.name);
                }
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

        private void ShuffleList<T>(List<T> list)
        {
            for (int index = list.Count - 1; index > 0; index--)
            {
                int randomIndex = Random.Range(0, index + 1);
                T temp = list[index];
                list[index] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}