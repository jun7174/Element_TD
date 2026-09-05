// Assets/Scripts/Towers/TowerCombatController.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 전투형 타워의 자동 타겟팅과 공격 실행을 담당한다.
    // 사거리 안 몬스터를 찾아 타겟팅 전략으로 대상을 고르고, 공격속도 주기에 맞춰 공격한다.
    [RequireComponent(typeof(TowerBase))]
    public class TowerCombatController : MonoBehaviour
    {
        private TowerBase _towerBase;
        private float _cooldownRemaining;

        private void Awake()
        {
            _towerBase = GetComponent<TowerBase>();
        }

        private void Update()
        {
            _cooldownRemaining -= Time.deltaTime;

            if (_cooldownRemaining > 0f)
            {
                return;
            }

            TryAttack();
        }

        private void TryAttack()
        {
            TowerDataSO towerData = _towerBase.TowerData;
            ElementEffectSO currentEffect = _towerBase.GetCurrentElementEffect();

            float effectiveRange = CalculateEffectiveRange(towerData, currentEffect);
            List<MonsterBase> candidates = FindCandidatesInRange(effectiveRange);

            if (candidates.Count == 0)
            {
                return;
            }

            MonsterBase target = towerData.TargetingStrategy.SelectTarget(transform.position, candidates);

            if (target == null)
            {
                return;
            }

            PerformAttack(target, towerData, currentEffect);

            float effectiveAttackSpeed = CalculateEffectiveAttackSpeed(towerData, currentEffect);
            _cooldownRemaining = 1f / effectiveAttackSpeed;
        }

        private List<MonsterBase> FindCandidatesInRange(float range)
        {
            List<MonsterBase> candidates = new List<MonsterBase>();

            foreach (MonsterBase monster in MonsterBase.ActiveMonsters)
            {
                float distance = Vector2.Distance(transform.position, monster.transform.position);

                if (distance <= range)
                {
                    candidates.Add(monster);
                }
            }

            return candidates;
        }

        // 발사 시점에 확정되는 데미지 정보(AttackPayload)를 만들고, 공격 패턴에 맞는 방식으로 발사한다.
        private void PerformAttack(MonsterBase target, TowerDataSO towerData, ElementEffectSO currentEffect)
        {
            AttackPayload payload = BuildAttackPayload(towerData, currentEffect);
            LaunchAttack(target, towerData, payload);
        }

        // 강화 수치, 원소 효과 배율, 버프 배율, 치명타 여부, 방어관통 비율을 전부 반영해서 데미지 정보를 확정한다.
        // 이후 대상이 바뀌거나(가로채기) 시간이 지나도 이 값 자체는 변하지 않는다.
        private AttackPayload BuildAttackPayload(TowerDataSO towerData, ElementEffectSO currentEffect)
        {
            float damageMultiplier = 1f;
            float critChance = 0f;
            float armorPenetrationRatio = 0f;

            if (currentEffect is StatModifierEffectSO statModifierEffect)
            {
                if (statModifierEffect.TargetStat == TargetStat.Damage)
                {
                    damageMultiplier = 1f + statModifierEffect.ModifierValue;
                }
                else if (statModifierEffect.TargetStat == TargetStat.CritChance)
                {
                    critChance = statModifierEffect.ModifierValue;
                }
                else if (statModifierEffect.TargetStat == TargetStat.ArmorPenetration)
                {
                    armorPenetrationRatio = statModifierEffect.ModifierValue;
                }
            }

            bool isCriticalHit = Random.value < critChance;

            if (isCriticalHit)
            {
                damageMultiplier *= DamageCalculator.CriticalHitMultiplier;
            }

            float totalMultiplier = damageMultiplier * _towerBase.BuffMultiplier;
            float finalBaseDamage = _towerBase.GetUpgradedCoreValue() * totalMultiplier;
            ElementType currentElement = _towerBase.GetCurrentElement();

            return new AttackPayload(finalBaseDamage, currentElement, isCriticalHit, armorPenetrationRatio);
        }

        // 공격 패턴에 따라 즉시 명중(저격), 고정 좌표로 날아가는 투사체(범위), 유도 투사체(기본, 연사) 중 하나로 발사한다.
        private void LaunchAttack(MonsterBase target, TowerDataSO towerData, AttackPayload payload)
        {
            if (towerData.AttackPattern == AttackPatternType.Sniper)
            {
                SniperTracerSpawner.Spawn(transform.position, target.transform.position);

                if (towerData.HitEffectPrefab != null)
                {
                    Instantiate(towerData.HitEffectPrefab, target.transform.position, Quaternion.identity);
                }

                payload.ApplyTo(target);
                return;
            }

            if (towerData.AttackPattern == AttackPatternType.Splash)
            {
                GameObject projectileObject = Instantiate(towerData.ProjectilePrefab, transform.position, Quaternion.identity);
                SlowProjectile slowProjectile = projectileObject.GetComponent<SlowProjectile>();
                slowProjectile.Launch(target.transform.position, towerData.SplashRadius, payload, towerData.HitEffectPrefab);
                return;
            }

            GameObject homingObject = Instantiate(towerData.ProjectilePrefab, transform.position, Quaternion.identity);
            HomingProjectile homingProjectile = homingObject.GetComponent<HomingProjectile>();
            homingProjectile.Launch(target, payload, towerData.HitEffectPrefab);
        }

        // 지금 이 타워의 유효 사거리를 반환한다. 원소 효과로 사거리가 늘어난 상태라면 그 값이 반영된다.
        // TowerRangeIndicator 등 외부에서 사거리를 표시할 때 사용한다.
        public float GetEffectiveRange()
        {
            TowerDataSO towerData = _towerBase.TowerData;
            ElementEffectSO currentEffect = _towerBase.GetCurrentElementEffect();
            return CalculateEffectiveRange(towerData, currentEffect);
        }

        private float CalculateEffectiveRange(TowerDataSO towerData, ElementEffectSO currentEffect)
        {
            if (currentEffect is StatModifierEffectSO statModifierEffect && statModifierEffect.TargetStat == TargetStat.Range)
            {
                return towerData.AttackRange * (1f + statModifierEffect.ModifierValue);
            }

            return towerData.AttackRange;
        }

        private float CalculateEffectiveAttackSpeed(TowerDataSO towerData, ElementEffectSO currentEffect)
        {
            if (currentEffect is StatModifierEffectSO statModifierEffect && statModifierEffect.TargetStat == TargetStat.AttackSpeed)
            {
                return towerData.AttackSpeed * (1f + statModifierEffect.ModifierValue);
            }

            return towerData.AttackSpeed;
        }
    }
}
