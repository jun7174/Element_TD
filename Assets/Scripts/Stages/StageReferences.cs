// Assets/Scripts/Stages/StageReferences.cs

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    // 스테이지 프리팹 루트에 붙어서, 이 스테이지의 타일맵과 웨이포인트 참조를 한곳에 모아둔다.
    // StageManager가 스테이지를 인스턴스화한 뒤 이 컴포넌트를 통해 필요한 참조를 가져온다.
    public class StageReferences : MonoBehaviour
    {
        [Tooltip("설치 가능 여부만 판정하는 타일맵이다. 타일이 있으면 설치 가능한 칸이다.")]
        [SerializeField]
        private Tilemap _buildableAreaTilemap;

        [Tooltip("원소 데이터를 담은 커스텀 타일이 배치되는 오버레이 타일맵이다.")]
        [SerializeField]
        private Tilemap _elementOverlayTilemap;

        [Tooltip("몬스터가 순서대로 통과할 경유 지점 목록이다.")]
        [SerializeField]
        private List<Transform> _waypoints;

        public Tilemap BuildableAreaTilemap => _buildableAreaTilemap;
        public Tilemap ElementOverlayTilemap => _elementOverlayTilemap;
        public List<Transform> Waypoints => _waypoints;
    }
}
