// Assets/Scripts/Combat/DamageCalculator.cs

using UnityEngine;

namespace ElementTD
{
    // 기본 데미지에 전투형 슬롯 효과 배율과 몬스터 원소 배율을 곱하고,
    // 몬스터의 방어력(방어관통 비율만큼 무시됨)을 뺀 최종 데미지를 계산하는 역할만 담당한다.
    public static class DamageCalculator
    {
        // 치명타가 발동했을 때 적용되는 데미지 배율이다.
        public const float CriticalHitMultiplier = 2f;

        // 방어력 계산 후에도 보장되는 최소 데미지이다.
        // 추후 0이나 다른 값으로 바꾸고 싶으면 이 값만 수정하면 된다.
        public const float MinimumDamage = 1f;

        // monsterDefense는 몬스터의 방어력이고, armorPenetrationRatio는 그 방어력 중 몇 퍼센트를 무시하는지이다.
        // 예를 들어 방어력 6에 armorPenetrationRatio가 0.5이면, 실제로 깎이는 데미지는 방어력 3만큼이다.
        public static float CalculateFinalDamage(float baseDamage, float combatEffectMultiplier, float monsterResistanceMultiplier, float monsterDefense, float armorPenetrationRatio)
        {
            float rawDamage = baseDamage * combatEffectMultiplier * monsterResistanceMultiplier;
            float effectiveDefense = monsterDefense * (1f - armorPenetrationRatio);

            return Mathf.Max(MinimumDamage, rawDamage - effectiveDefense);
        }
    }
}
