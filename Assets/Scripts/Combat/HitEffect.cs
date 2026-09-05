// Assets/Scripts/Combat/HitEffect.cs

using UnityEngine;

namespace ElementTD
{
    // 타워 종류별 타격 이펙트 애니메이션을 재생하고, 한 번 다 재생되면 자기 자신을 파괴한다.
    // 애니메이션 자체는 미리 만들어진 Animation Clip을 사용하며,
    // 이 스크립트는 재생 속도 조절과 재생 완료 감지만 담당한다.
    [RequireComponent(typeof(Animator))]
    public class HitEffect : MonoBehaviour
    {
        [Tooltip("애니메이션 재생 속도이다. 1이 기본 속도이고, 크면 빠르게, 작으면 느리게 재생된다.")]
        [SerializeField]
        private float _playbackSpeed = 1f;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.speed = _playbackSpeed;
        }

        private void Update()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
