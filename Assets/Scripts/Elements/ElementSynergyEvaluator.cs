// Assets/Scripts/Elements/ElementSynergyEvaluator.cs

using System.Collections.Generic;

namespace ElementTD
{
    // 계층과 원소 조합을 받아서 해당하는 원소 효과를 조회하는 역할만 담당한다.
    // 효과를 실제로 적용하는 계산은 이 클래스가 아니라 이 결과를 사용하는 쪽에서 처리한다.
    public class ElementSynergyEvaluator
    {
        private readonly List<ElementDataSO> _elementDataList;

        public ElementSynergyEvaluator(List<ElementDataSO> elementDataList)
        {
            _elementDataList = elementDataList;
        }

        // 주어진 원소와 계층에 해당하는 효과를 찾아서 반환한다.
        // 일치하는 원소 데이터가 없거나 원소가 없는 경우에는 null을 반환한다.
        public ElementEffectSO GetEffect(ElementType element, TowerType type)
        {
            if (element == ElementType.None)
            {
                return null;
            }

            ElementDataSO matchedElementData = FindElementData(element);

            if (matchedElementData == null)
            {
                return null;
            }

            switch (type)
            {
                case TowerType.Combat:
                    return matchedElementData.CombatEffect;
                case TowerType.Support:
                    return matchedElementData.SupportEffect;
                case TowerType.Economy:
                    return matchedElementData.EconomyEffect;
                default:
                    return null;
            }
        }

        private ElementDataSO FindElementData(ElementType element)
        {
            foreach (ElementDataSO elementData in _elementDataList)
            {
                if (elementData.Element == element)
                {
                    return elementData;
                }
            }

            return null;
        }
    }
}
