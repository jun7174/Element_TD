// Assets/Scripts/Combat/HomingProjectile.cs

using UnityEngine;

namespace ElementTD
{
    // 지정된 대상을 향해 계속 유도되는 투사체이다. 기본공격과 연사공격 타워가 사용한다.
    // AllowInterception이 켜져 있으면, 비행 중 원래 대상이 아닌 다른 몬스터가
    // 가까이 있을 때 그 몬스터가 대신 맞는 가로채기가 일어난다.
    public class HomingProjectile : MonoBehaviour
    {
        private const float HitDistance = 0.15f;
        private const float InterceptionRadius = 0.3f;

        [SerializeField]
        private float _speed = 8f;

        [Tooltip("켜져 있으면 비행 중 다른 몬스터가 가로챌 수 있다. 연사공격 타워용 프리팹에서 켠다.")]
        [SerializeField]
        private bool _allowInterception;

        private MonsterBase _target;
        private AttackPayload _payload;
        private GameObject _hitEffectPrefab;

        // 발사 시점에 호출해서 추적할 대상, 데미지 정보, 명중 시 재생할 이펙트를 설정한다.
        public void Launch(MonsterBase target, AttackPayload payload, GameObject hitEffectPrefab)
        {
            _target = target;
            _payload = payload;
            _hitEffectPrefab = hitEffectPrefab;
        }

        private void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            if (_allowInterception)
            {
                MonsterBase interceptor = FindInterceptor();

                if (interceptor != null)
                {
                    SpawnHitEffect(interceptor.transform.position);
                    _payload.ApplyTo(interceptor);
                    Destroy(gameObject);
                    return;
                }
            }

            MoveTowardTarget();
        }

        private void MoveTowardTarget()
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            float distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);

            if (distanceToTarget <= HitDistance)
            {
                SpawnHitEffect(_target.transform.position);
                _payload.ApplyTo(_target);
                Destroy(gameObject);
            }
        }

        private void SpawnHitEffect(Vector3 position)
        {
            //SoundManager.instance.PlaySFX(ESfx.BASIC_ATK);
            if (_hitEffectPrefab != null)
            {
                Instantiate(_hitEffectPrefab, position, Quaternion.identity);
            }
        }

        // 원래 대상이 아닌 몬스터 중, 지금 투사체 위치 가까이에 있는 몬스터를 찾는다.
        private MonsterBase FindInterceptor()
        {
            foreach (MonsterBase monster in MonsterBase.ActiveMonsters)
            {
                if (monster == _target)
                {
                    continue;
                }

                float distance = Vector2.Distance(transform.position, monster.transform.position);

                if (distance <= InterceptionRadius)
                {
                    return monster;
                }
            }

            return null;
        }
    }
}
