// Assets/Scripts/Data/Elements/ElementColorPaletteSO.cs

using UnityEngine;

namespace ElementTD
{
    // 원소별 플로팅 데미지 텍스트 색상을 정의한다.
    // 하나의 에셋으로 관리해서, 나중에 색상을 바꾸고 싶을 때 코드 수정 없이 여기서만 조절하면 된다.
    [CreateAssetMenu(fileName = "NewElementColorPalette", menuName = "ElementTD/Element Color Palette")]
    public class ElementColorPaletteSO : ScriptableObject
    {
        public Color WaterColor = new Color(0.3f, 0.55f, 1f);
        public Color FireColor = new Color(1f, 0.5f, 0.1f);
        public Color GrassColor = new Color(0.3f, 0.8f, 0.3f);
        public Color DarkColor = new Color(0.6f, 0.25f, 0.85f);
        public Color LightColor = new Color(1f, 0.85f, 0.2f);
        public Color NoneColor = Color.gray;

        // 원소에 해당하는 색상을 반환한다. 무속성이면 NoneColor를 반환한다.
        public Color GetColor(ElementType element)
        {
            switch (element)
            {
                case ElementType.Water:
                    return WaterColor;
                case ElementType.Fire:
                    return FireColor;
                case ElementType.Grass:
                    return GrassColor;
                case ElementType.Dark:
                    return DarkColor;
                case ElementType.Light:
                    return LightColor;
                default:
                    return NoneColor;
            }
        }
    }
}
