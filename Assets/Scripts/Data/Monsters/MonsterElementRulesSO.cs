// Assets/Scripts/Data/Monsters/MonsterElementRulesSO.cs

using UnityEngine;

namespace ElementTD
{
    // 원소 하나를 받아서 그 원소 변종 몬스터의 저항/약점 원소와 배율을 자동으로 계산하는 규칙을 담는다.
    // Water, Fire, Grass는 삼각 순환 상성(Water가 Fire를 이기고, Fire가 Grass를 이기고, Grass가 Water를 이김)이고,
    // Light와 Dark는 서로 대립하며 각자 자기 원소 공격에 저항한다.
    // 원소 조합 자체는 게임 규칙이라 코드에 고정되어 있지만, 배율 수치는 여기서 직접 조절 가능하다.
    [CreateAssetMenu(fileName = "NewMonsterElementRules", menuName = "ElementTD/Monster Element Rules")]
    public class MonsterElementRulesSO : ScriptableObject
    {
        [Tooltip("약점 원소 공격을 받았을 때 적용되는 배율이다.")]
        public float WeaknessMultiplier = 1.5f;

        [Tooltip("저항 원소 공격을 받았을 때 적용되는 배율이다.")]
        public float ResistanceMultiplier = 0.5f;

        // element 변종 몬스터가 저항하는 공격 원소를 반환한다.
        private ElementType GetResistantAttackElement(ElementType element)
        {
            switch (element)
            {
                case ElementType.Water:
                    return ElementType.Fire;
                case ElementType.Fire:
                    return ElementType.Grass;
                case ElementType.Grass:
                    return ElementType.Water;
                case ElementType.Light:
                    return ElementType.Light;
                case ElementType.Dark:
                    return ElementType.Dark;
                default:
                    return ElementType.None;
            }
        }

        // element 변종 몬스터가 약점을 가지는 공격 원소를 반환한다.
        private ElementType GetWeakAttackElement(ElementType element)
        {
            switch (element)
            {
                case ElementType.Water:
                    return ElementType.Grass;
                case ElementType.Fire:
                    return ElementType.Water;
                case ElementType.Grass:
                    return ElementType.Fire;
                case ElementType.Light:
                    return ElementType.Dark;
                case ElementType.Dark:
                    return ElementType.Light;
                default:
                    return ElementType.None;
            }
        }

        // element 변종 몬스터가 attackElement 공격을 받았을 때 적용할 최종 배율을 반환한다.
        // 약점도 저항도 아니면 일반 배율인 1을 반환한다.
        public float GetMultiplier(ElementType element, ElementType attackElement)
        {
            if (attackElement == GetWeakAttackElement(element))
            {
                return WeaknessMultiplier;
            }

            if (attackElement == GetResistantAttackElement(element))
            {
                return ResistanceMultiplier;
            }

            return 1f;
        }
    }
}
