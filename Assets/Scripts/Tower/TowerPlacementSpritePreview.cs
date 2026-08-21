// Assets/Scripts/Towers/TowerPlacementSpritePreview.cs

using UnityEngine;

namespace ElementTD
{
    // 타워 구매 또는 재배치 대기 상태에서, 마우스가 가리키는 타일 위치에 타워 스프라이트를 미리 보여준다.
    // 실제 타워 프리팹을 생성하지 않고 스프라이트만 표시해서, 정적 레지스트리(TowerBase.ActiveTowers)에
    // 아직 구매하지 않은 타워가 등록되어 다른 시스템(오라 버프 등)에 영향을 주는 부작용을 막는다.
    // 설치 가능한 칸이면 원래 색에 반투명, 불가능한 칸이면 빨간색으로 표시한다.
    [RequireComponent(typeof(SpriteRenderer))]
    public class TowerPlacementSpritePreview : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField]
        private float _alpha = 0.5f;

        [SerializeField]
        private Color _invalidColor = Color.red;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        // 미리보기를 켜고 주어진 위치에 스프라이트를 표시한다.
        // isValid가 true이면 원래 색에 반투명, false이면 빨간색으로 표시한다.
        public void Show(Vector2 worldPosition, Sprite sprite, bool isValid)
        {
            transform.position = worldPosition;
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = sprite;

            Color displayColor = isValid ? Color.white : _invalidColor;
            displayColor.a = _alpha;
            _spriteRenderer.color = displayColor;
        }

        public void Hide()
        {
            _spriteRenderer.enabled = false;
        }
    }
}
