// Assets/Scripts/Monsters/MonsterBase.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 몬스터의 이동, 체력, 원소 저항 적용, 누출 판정을 담당한다.
    // 몬스터 데이터와 난이도 배율은 스폰 시점에 Initialize를 통해 외부에서 주입받는다.
    // 이렇게 하면 몬스터 프리팹 하나로 모든 원형과 원소 변종을 전부 처리할 수 있다.
    public class MonsterBase : MonoBehaviour
    {
        private const float ArrivalThreshold = 0.1f;

        private static readonly List<MonsterBase> s_activeMonsters = new List<MonsterBase>();

        public static IReadOnlyList<MonsterBase> ActiveMonsters => s_activeMonsters;

        private MonsterDataSO _monsterData;
        private float _difficultyMultiplier = 1f;
        private float _maxHealth;
        private float _currentHealth;
        private readonly Dictionary<AuraEffect, float> _speedDebuffContributions = new Dictionary<AuraEffect, float>();

        [Tooltip("MonsterDataSO.UseAutomaticResistances가 켜진 몬스터의 저항/약점을 계산할 때 사용하는 규칙이다.")]
        [SerializeField]
        private MonsterElementRulesSO _elementRules;

        [SerializeField]
        private MonsterSpriteColorApplier _spriteColorApplier;

        private List<Transform> _path;
        private int _currentWaypointIndex;

        // 난이도 배율이 반영된 처치 골드이다. 경제 시스템이 연결되면 이 값을 사용한다.
        public int GoldReward => Mathf.RoundToInt(_monsterData.GoldReward * _difficultyMultiplier);

        // 현재 체력을 최대 체력으로 나눈 비율이다. 체력바 표시 등 외부에서 사용한다.
        public float HealthRatio => _maxHealth > 0f ? _currentHealth / _maxHealth : 0f;

        // 이 몬스터의 방어력이다. 데미지 계산 시 방어관통 비율만큼 무시된 뒤 고정 수치로 차감된다.
        public float Defense => _monsterData.Defense;

        // 걸려있는 모든 감속 오라의 기여분을 곱해서 누적한 최종 배율이다.
        // 곱셈으로 누적하면 아무리 많이 겹쳐도 수학적으로 0 이하가 될 수 없다.
        private float SpeedMultiplier
        {
            get
            {
                float multiplier = 1f;

                foreach (float amount in _speedDebuffContributions.Values)
                {
                    multiplier *= 1f - amount;
                }

                return multiplier;
            }
        }

        private void OnEnable()
        {
            s_activeMonsters.Add(this);
        }

        private void OnDisable()
        {
            s_activeMonsters.Remove(this);
        }

        private void Update()
        {
            MoveAlongPath();
        }

        // 스폰 시점에 몬스터 데이터와 난이도 배율을 주입받는다.
        // 체력은 기본 체력에 난이도 배율을 곱한 값으로 초기화되고, 스프라이트 색도 이 시점에 같이 적용된다.
        public void Initialize(MonsterDataSO monsterData, float difficultyMultiplier)
        {
            _monsterData = monsterData;
            _difficultyMultiplier = difficultyMultiplier;
            _maxHealth = _monsterData.BaseHealth * _difficultyMultiplier;
            _currentHealth = _maxHealth;

            if (_spriteColorApplier != null)
            {
                _spriteColorApplier.ApplyColor(_monsterData.Element);
            }
        }

        // 이동 경로를 설정한다. 목록의 첫 번째 지점부터 순서대로 통과한다.
        public void SetPath(List<Transform> path)
        {
            _path = path;
            _currentWaypointIndex = 0;
        }

        private void MoveAlongPath()
        {
            if (_path == null || _currentWaypointIndex >= _path.Count)
            {
                return;
            }

            Transform currentWaypoint = _path[_currentWaypointIndex];
            Vector3 direction = (currentWaypoint.position - transform.position).normalized;
            float currentSpeed = _monsterData.BaseMoveSpeed * SpeedMultiplier;
            transform.position += direction * currentSpeed * Time.deltaTime;

            float distanceToWaypoint = Vector2.Distance(transform.position, currentWaypoint.position);

            if (distanceToWaypoint <= ArrivalThreshold)
            {
                _currentWaypointIndex++;

                if (_currentWaypointIndex >= _path.Count)
                {
                    ReachEndOfPath();
                }
            }
        }

        private void ReachEndOfPath()
        {
            int scaledDamage = Mathf.RoundToInt(_monsterData.DamageToPlayer * _difficultyMultiplier);
            PlayerHealth.TakeDamage(scaledDamage);
            Destroy(gameObject);
        }

        // 원소 공격을 받았을 때 적용되는 배율을 반환한다.
        // UseAutomaticResistances가 켜져 있으면 MonsterElementRulesSO의 규칙으로 계산하고,
        // 꺼져 있으면 수동으로 채운 저항 목록을 사용한다 (목록에 없는 원소는 일반 배율 1).
        public float GetResistanceMultiplier(ElementType attackElement)
        {
            if (_monsterData.UseAutomaticResistances)
            {
                return _elementRules.GetMultiplier(_monsterData.Element, attackElement);
            }

            List<ElementResistanceEntry> resistances = _monsterData.ElementResistances;

            foreach (ElementResistanceEntry entry in resistances)
            {
                if (entry.Element == attackElement)
                {
                    return entry.DamageMultiplier;
                }
            }

            return 1f;
        }

        // source 오라가 이 몬스터에게 거는 이동속도 감소 수치를 갱신한다.
        // 매 프레임 다시 호출해도 같은 오라의 항목은 하나만 유지되며 값만 최신으로 덮어써진다.
        public void ApplySpeedDebuff(AuraEffect source, float amount)
        {
            _speedDebuffContributions[source] = amount;
        }

        // source 오라의 감속 기여분을 제거한다. 다른 오라의 기여분은 영향받지 않는다.
        public void RemoveSpeedDebuff(AuraEffect source)
        {
            _speedDebuffContributions.Remove(source);
        }

        // 데미지를 받아 체력을 감소시킨다.
        // 체력이 0 이하가 되면 처치 골드를 지급하고 몬스터를 제거한다.
        public void TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;

            if (_currentHealth <= 0f)
            {
                EconomyManager.AddGold(GoldReward);
                Debug.Log(_monsterData.name + " 처치됨, 골드 획득 " + GoldReward);
                Destroy(gameObject);
            }
        }
    }
}
