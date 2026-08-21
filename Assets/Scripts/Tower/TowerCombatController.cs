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

        private void PerformAttack(MonsterBase target, TowerDataSO towerData, ElementEffectSO currentEffect)
        {
            float damageMultiplier = 1f;
            float critChance = 0f;

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
            }

            bool isCriticalHit = Random.value < critChance;

            if (isCriticalHit)
            {
                damageMultiplier *= DamageCalculator.CriticalHitMultiplier;
            }

            ApplyDamageTo(target, towerData, damageMultiplier, isCriticalHit);

            if (towerData.AttackPattern == AttackPatternType.Splash)
            {
                ApplySplashDamage(target, towerData, damageMultiplier, isCriticalHit);
            }
        }

        private void ApplyDamageTo(MonsterBase target, TowerDataSO towerData, float damageMultiplier, bool isCriticalHit)
        {
            ElementType currentElement = _towerBase.GetCurrentElement();
            float monsterResistanceMultiplier = target.GetResistanceMultiplier(currentElement);
            float totalMultiplier = damageMultiplier * _towerBase.BuffMultiplier;
            float baseDamage = _towerBase.GetUpgradedCoreValue();
            float finalDamage = DamageCalculator.CalculateFinalDamage(baseDamage, totalMultiplier, monsterResistanceMultiplier);

            target.TakeDamage(finalDamage);
            FloatingDamageTextSpawner.Spawn(target.transform.position, finalDamage, currentElement, isCriticalHit);
        }

        private void ApplySplashDamage(MonsterBase primaryTarget, TowerDataSO towerData, float damageMultiplier, bool isCriticalHit)
        {
            foreach (MonsterBase monster in MonsterBase.ActiveMonsters)
            {
                if (monster == primaryTarget)
                {
                    continue;
                }

                float distance = Vector2.Distance(primaryTarget.transform.position, monster.transform.position);

                if (distance <= towerData.SplashRadius)
                {
                    ApplyDamageTo(monster, towerData, damageMultiplier, isCriticalHit);
                }
            }
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
