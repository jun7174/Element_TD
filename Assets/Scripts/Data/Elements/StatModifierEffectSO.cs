// Assets/Scripts/Data/Elements/StatModifierEffectSO.cs

using UnityEngine;

namespace ElementTD
{
    // 특정 스탯을 정해진 값만큼 증가시키는 원소 효과이다.
    // TargetStat 필드로 어떤 스탯을 대상으로 하는지 정하고, ModifierValue 필드로 증가량을 정한다.
    // 지원형과 경제형 타워는 CoreValue를 대상 스탯으로 사용한다.
    [CreateAssetMenu(fileName = "NewStatModifierEffect", menuName = "ElementTD/Stat Modifier Effect")]
    public class StatModifierEffectSO : ElementEffectSO
    {
        public TargetStat TargetStat;

        [Tooltip("증가율이다. 0.1은 10 퍼센트 증가를 의미한다.")]
        public float ModifierValue;
    }
}
