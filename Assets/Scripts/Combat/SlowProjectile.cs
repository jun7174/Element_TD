// Assets/Scripts/Combat/SlowProjectile.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 발사 시점에 정해진 고정 좌표를 향해 날아가는 느린 투사체이다. 범위공격 타워가 사용한다.
    // 목표 몬스터를 추적하지 않으므로, 도착할 때까지 몬스터가 이동하면 빗나갈 수 있다.
    public class SlowProjectile : MonoBehaviour
    {
        private const float HitDistance = 0.15f;

        [SerializeField]
        private float _speed = 3f;

        private Vector3 _destination;
        private float _splashRadius;
        private AttackPayload _payload;
        private GameObject _hitEffectPrefab;

        // 발사 시점에 호출해서 목표 좌표, 스플래시 반경, 데미지 정보, 명중 시 재생할 이펙트를 설정한다.
        public void Launch(Vector3 destination, float splashRadius, AttackPayload payload, GameObject hitEffectPrefab)
        {
            _destination = destination;
            _splashRadius = splashRadius;
            _payload = payload;
            _hitEffectPrefab = hitEffectPrefab;
        }

        private void Update()
        {
            Vector3 direction = (_destination - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            float distanceToDestination = Vector2.Distance(transform.position, _destination);

            if (distanceToDestination <= HitDistance)
            {
                Explode();
            }
        }

        private void Explode()
        {
            // 데미지를 적용하다가 몬스터가 죽으면 ActiveMonsters 목록에서 자기 자신을 즉시 제거한다.
            // 그 목록을 그대로 순회하면서 동시에 바꾸면 예외가 발생하므로,
            // 반경 안 대상을 먼저 별도 목록에 담아둔 뒤, 그 별도 목록을 순회하며 데미지를 적용한다.
            List<MonsterBase> monstersInRange = new List<MonsterBase>();

            foreach (MonsterBase monster in MonsterBase.ActiveMonsters)
            {
                float distance = Vector2.Distance(transform.position, monster.transform.position);

                if (distance <= _splashRadius)
                {
                    monstersInRange.Add(monster);
                }
            }

            // 맞은 몬스터 수와 무관하게, 폭발 중심에 이펙트를 딱 한 번만 생성한다.
            if (_hitEffectPrefab != null)
            {
                Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);
            }

            foreach (MonsterBase monster in monstersInRange)
            {
                _payload.ApplyTo(monster);
            }

            Destroy(gameObject);
        }
    }
}
