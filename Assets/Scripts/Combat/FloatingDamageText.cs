// Assets/Scripts/Combat/FloatingDamageText.cs

using TMPro;
using UnityEngine;

namespace ElementTD
{
    // 데미지 숫자 하나가 위로 떠오르다 사라지는 것을 담당한다.
    // 지금은 생성될 때마다 새로 만들고 소멸 시간이 지나면 파괴하는 단순한 방식이며,
    // 추후 오브젝트 풀링으로 최적화할 예정이다.
    public class FloatingDamageText : MonoBehaviour
    {
        [Tooltip("텍스트가 위로 떠오르는 속도이다.")]
        [SerializeField]
        private float _riseSpeed = 1f;

        [Tooltip("텍스트가 사라지기까지 걸리는 시간이다. 단위는 초이다.")]
        [SerializeField]
        private float _lifetimeSeconds = 0.8f;

        [SerializeField]
        private TextMeshPro _text;

        private float _elapsedTime;

        private void Awake()
        {
            _text.alignment = TextAlignmentOptions.Center;

            // 몬스터 스프라이트 등 다른 오브젝트에 가려지지 않도록 정렬 순서를 항상 높게 고정한다.
            Renderer textRenderer = _text.GetComponent<Renderer>();

            if (textRenderer != null)
            {
                textRenderer.sortingOrder = 100;
            }
        }

        // 생성 직후 호출해서 표시할 데미지 값, 색상, 치명타 여부를 설정한다.
        public void Setup(float damageAmount, Color color, bool isCriticalHit)
        {
            _text.text = Mathf.RoundToInt(damageAmount).ToString();
            _text.color = color;
            _text.fontStyle = isCriticalHit ? FontStyles.Bold : FontStyles.Normal;
        }

        private void Update()
        {
            transform.position += Vector3.up * _riseSpeed * Time.deltaTime;
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _lifetimeSeconds)
            {
                Destroy(gameObject);
            }
        }
    }
}
