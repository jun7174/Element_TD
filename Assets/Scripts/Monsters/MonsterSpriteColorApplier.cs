// Assets/Scripts/Monsters/MonsterSpriteColorApplier.cs

using UnityEngine;

namespace ElementTD
{
    // 몬스터 데이터의 원소에 맞게 스프라이트 색을 입힌다.
    // MonsterBase.Initialize가 스폰 시점에 데이터를 주입받는 순간 같이 호출해서,
    // 색이 적용되는 타이밍이 항상 데이터 주입과 함께 확실히 보장되도록 한다.
    [RequireComponent(typeof(SpriteRenderer))]
    public class MonsterSpriteColorApplier : MonoBehaviour
    {
        [SerializeField]
        private ElementColorPaletteSO _colorPalette;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // 원소에 맞는 색을 스프라이트에 입힌다. 무속성(None)이면 팔레트의 NoneColor가 적용된다.
        public void ApplyColor(ElementType element)
        {
            _spriteRenderer.color = _colorPalette.GetColor(element);
        }
    }
}
