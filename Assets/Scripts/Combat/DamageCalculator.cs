// Assets/Scripts/Combat/DamageCalculator.cs

namespace ElementTD
{
    // 기본 데미지에 전투형 슬롯 효과 배율과 몬스터 원소 배율을 곱해서
    // 최종 데미지를 계산하는 역할만 담당한다.
    public static class DamageCalculator
    {
        // 치명타가 발동했을 때 적용되는 데미지 배율이다.
        public const float CriticalHitMultiplier = 2f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseDamage"></param>
        /// <param name="combatEffectMultiplier"></param>
        /// <param name="monsterResistanceMultiplier"></param>
        /// <returns></returns>
        public static float CalculateFinalDamage(float baseDamage, float combatEffectMultiplier, float monsterResistanceMultiplier)
        {
            return baseDamage * combatEffectMultiplier * monsterResistanceMultiplier;
        }
    }
}
