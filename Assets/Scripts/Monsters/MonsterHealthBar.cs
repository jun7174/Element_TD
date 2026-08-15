// Assets/Scripts/Monsters/MonsterHealthBar.cs

using UnityEngine;

namespace ElementTD
{
    // 몬스터 위에 체력바를 표시한다.
    // 체력이 가득 찬 상태에서는 숨기고, 한 번이라도 깎이면 나타난다.
    // 채움 바는 크기와 위치를 함께 조정해서, 스프라이트의 피벗 설정과 무관하게
    // 왼쪽은 고정된 채 오른쪽부터 줄어드는 것처럼 보이게 만든다.
    [RequireComponent(typeof(MonsterBase))]
    public class MonsterHealthBar : MonoBehaviour
    {
        [Tooltip("체력 비율만큼 가로 크기가 줄어드는 채움 스프라이트이다.")]
        [SerializeField]
        private SpriteRenderer _fillBar;

        [Tooltip("항상 고정된 크기로 표시되는 배경 스프라이트이다.")]
        [SerializeField]
        private SpriteRenderer _backgroundBar;

        private MonsterBase _monster;
        private Vector3 _fillBarBaseLocalPosition;
        private float _fillBarBaseScaleX;
        private float _fillBarFullWidth;

        private void Awake()
        {
            _monster = GetComponent<MonsterBase>();

            Transform fillTransform = _fillBar.transform;
            _fillBarBaseLocalPosition = fillTransform.localPosition;
            _fillBarBaseScaleX = fillTransform.localScale.x;
            _fillBarFullWidth = _fillBar.sprite.bounds.size.x * _fillBarBaseScaleX;
        }

        private void Update()
        {
            float healthRatio = _monster.HealthRatio;
            bool isDamaged = healthRatio < 1f;

            SetVisible(isDamaged);

            if (isDamaged)
            {
                UpdateFillBar(healthRatio);
            }
        }

        private void SetVisible(bool isVisible)
        {
            _fillBar.enabled = isVisible;
            _backgroundBar.enabled = isVisible;
        }

        // 채움 바의 가로 크기를 체력 비율만큼 줄이고, 왼쪽 끝이 고정되어 보이도록 위치도 함께 보정한다.
        private void UpdateFillBar(float healthRatio)
        {
            Transform fillTransform = _fillBar.transform;

            Vector3 scale = fillTransform.localScale;
            scale.x = _fillBarBaseScaleX * healthRatio;
            fillTransform.localScale = scale;

            float offsetX = (_fillBarFullWidth / 2f) * (healthRatio - 1f);
            Vector3 position = _fillBarBaseLocalPosition;
            position.x += offsetX;
            fillTransform.localPosition = position;
        }
    }
}

