// Assets/Scripts/Data/Elements/ElementDataSO.cs

using UnityEngine;

namespace ElementTD
{
    [CreateAssetMenu(fileName = "NewElementData", menuName = "ElementTD/Element Data")]
    public class ElementDataSO : ScriptableObject
    {
        public ElementType Element;

        [Header("타입별 효과 슬롯")]
        [Tooltip("전투형 타워가 이 원소 타일 위에 있을 때 적용되는 효과이다.")]
        public ElementEffectSO CombatEffect;

        [Tooltip("지원형 타워가 이 원소 타일 위에 있을 때 적용되는 효과이다. 현재는 다섯 원소가 동일한 효과를 공유해서 참조한다.")]
        public ElementEffectSO SupportEffect;

        [Tooltip("경제형 타워가 이 원소 타일 위에 있을 때 적용되는 효과이다. 현재는 다섯 원소가 동일한 효과를 공유해서 참조한다.")]
        public ElementEffectSO EconomyEffect;
    }
}
