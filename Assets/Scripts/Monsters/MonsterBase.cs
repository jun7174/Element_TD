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
        private float _speedMultiplier = 1f;

        private List<Transform> _path;
        private int _currentWaypointIndex;

        // 난이도 배율이 반영된 처치 골드이다. 경제 시스템이 연결되면 이 값을 사용한다.
        public int GoldReward => Mathf.RoundToInt(_monsterData.GoldReward * _difficultyMultiplier);

        // 현재 체력을 최대 체력으로 나눈 비율이다. 체력바 표시 등 외부에서 사용한다.
        public float HealthRatio => _maxHealth > 0f ? _currentHealth / _maxHealth : 0f;

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
        // 체력은 기본 체력에 난이도 배율을 곱한 값으로 초기화된다.
        public void Initialize(MonsterDataSO monsterData, float difficultyMultiplier)
        {
            _monsterData = monsterData;
            _difficultyMultiplier = difficultyMultiplier;
            _maxHealth = _monsterData.BaseHealth * _difficultyMultiplier;
            _currentHealth = _maxHealth;
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
            float currentSpeed = _monsterData.BaseMoveSpeed * _speedMultiplier;
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
        // 저항 목록에 해당 원소 항목이 없으면 일반 배율인 1을 반환한다.
        public float GetResistanceMultiplier(ElementType attackElement)
        {
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

        // 구속타워 오라로부터 이동속도 감소 디버프를 받는다.
        public void ApplySpeedDebuff(float amount)
        {
            _speedMultiplier = 1f - amount;
        }

        // 디버프 범위를 벗어나면 호출되어 이동속도 감소를 해제한다.
        public void RemoveSpeedDebuff()
        {
            _speedMultiplier = 1f;
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
