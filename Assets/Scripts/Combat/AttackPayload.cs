// Assets/Scripts/Combat/AttackPayload.cs

namespace ElementTD
{
    // 공격 발사 시점에 확정되는 데미지 정보를 담는다.
    // 발사 이후에는 값이 바뀌지 않으며, 명중 시점에 대상 몬스터의 저항 배율과 방어력만 추가로 계산해서 적용한다.
    public struct AttackPayload
    {
        public float BaseDamage;
        public ElementType Element;
        public bool IsCriticalHit;
        public float ArmorPenetrationRatio;

        public AttackPayload(float baseDamage, ElementType element, bool isCriticalHit, float armorPenetrationRatio)
        {
            BaseDamage = baseDamage;
            Element = element;
            IsCriticalHit = isCriticalHit;
            ArmorPenetrationRatio = armorPenetrationRatio;
        }

        // 대상 몬스터에게 최종 데미지를 계산해서 적용하고, 플로팅 데미지 텍스트를 생성한다.
        public void ApplyTo(MonsterBase target)
        {
            float monsterResistanceMultiplier = target.GetResistanceMultiplier(Element);
            float finalDamage = DamageCalculator.CalculateFinalDamage(BaseDamage, 1f, monsterResistanceMultiplier, target.Defense, ArmorPenetrationRatio);

            target.TakeDamage(finalDamage);
            FloatingDamageTextSpawner.Spawn(target.transform.position, finalDamage, Element, IsCriticalHit);
        }
    }
}
