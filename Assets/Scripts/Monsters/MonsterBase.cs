// Assets/Scripts/Monsters/MonsterBase.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 몬스터의 이동, 체력, 원소 저항 적용을 담당한다.
    // 이동은 지금 단계에서 단일 목표 지점을 향해 직선으로 이동하는 단순한 방식이며,
    // 실제 경로 이동 방식은 스테이지 시스템을 작성하는 단계에서 다시 검토한다.
    public class MonsterBase : MonoBehaviour
    {
        private static readonly List<MonsterBase> s_activeMonsters = new List<MonsterBase>();

        public static IReadOnlyList<MonsterBase> ActiveMonsters => s_activeMonsters;

        [SerializeField]
        private MonsterDataSO _monsterData;

        [SerializeField]
        private Transform _targetWaypoint;

        [SerializeField]
        private float _currentHealth;
        private float _speedMultiplier = 1f;

        private void Awake()
        {
            _currentHealth = _monsterData.BaseHealth;
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
            //                             TestMonster.cs로 임시 이동 테스트
            // MoveTowardWaypoint(); TODO : 추후 이동 알고리즘 교체후 추가 

        }

        private void MoveTowardWaypoint()
        {
            if (_targetWaypoint == null)
            {
                return;
            }

            Vector3 direction = (_targetWaypoint.position - transform.position).normalized;
            float currentSpeed = _monsterData.BaseMoveSpeed * _speedMultiplier;
            transform.position += direction * currentSpeed * Time.deltaTime;
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
        // 체력이 0 이하가 되면 몬스터를 제거한다.
        public void TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;

            if (_currentHealth <= 0f)
            {
                Debug.Log(_monsterData.name + " 처치됨");
                Destroy(gameObject);
            }
        }
    }
}
