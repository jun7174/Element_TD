// Assets/Scripts/Tiles/ElementTile.cs

using UnityEngine;

namespace ElementTD
{
    // 타일 하나가 어떤 원소를 가지고 있는지 보유하고, 이를 조회할 수 있는 API를 제공한다.
    // 이 컴포넌트가 붙은 오브젝트에는 트리거로 설정된 콜라이더가 있어야 하며,
    // 콜라이더의 범위가 곧 이 타일이 차지하는 영역이 된다.
    // 지금은 타일 하나마다 별도의 오브젝트를 두는 단순한 방식이며,
    // 실제 타일맵과 결합하는 방식은 스테이지 시스템을 작성하는 단계에서 다시 검토한다.
    [RequireComponent(typeof(Collider2D))]
    public class ElementTile : MonoBehaviour
    {
        [SerializeField]
        private ElementType _element;

        public ElementType Element => _element;

        // 주어진 위치에 겹쳐 있는 타일을 찾아서 반환한다.
        // 겹치는 타일이 없으면 null을 반환한다.
        public static ElementTile FindTileAt(Vector2 position)
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(position);

            if (hitCollider == null)
            {
                return null;
            }

            return hitCollider.GetComponent<ElementTile>();
        }
    }
}
