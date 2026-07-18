// Assets/Scripts/Monsters/MonsterBase.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 몬스터의 이동, 체력, 원소 저항 적용을 담당한다.
    public class MonsterBase : MonoBehaviour
    {
        [SerializeField]
        private MonsterDataSO _monsterData;

        [SerializeField]
        private Transform _targetWaypoint;

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _monsterData.BaseHealth;
        }

        private void Update()
        {
            MoveTowardWaypoint();
        }

        private void MoveTowardWaypoint()
        {
            if (_targetWaypoint == null)
            {
                return;
            }

            Vector3 direction = (_targetWaypoint.position - transform.position).normalized;
            transform.position += direction * _monsterData.BaseMoveSpeed * Time.deltaTime;
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

            Debug.Log("저항 없음");
            return 1f;
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
