// Assets/Scripts/Data/Towers/RangeIndicatorSettingsSO.cs

using UnityEngine;

namespace ElementTD
{
    // 타워 사거리 표시의 공통 설정을 담는다.
    // 모든 타워의 사거리 표시 컴포넌트가 이 에셋 하나를 공유 참조해서,
    // 여기서 값을 바꾸면 전체 타워에 한 번에 반영된다.
    [CreateAssetMenu(fileName = "NewRangeIndicatorSettings", menuName = "ElementTD/Range Indicator Settings")]
    public class RangeIndicatorSettingsSO : ScriptableObject
    {
        [Tooltip("설치 완료된 평소 상태에서 사거리 표시를 아예 숨길지 여부이다. 체크 해제하면 옅게 계속 보인다.")]
        public bool HideWhenIdle;

        [Range(0f, 1f)]
        [Tooltip("선택 중이거나 배치 미리보기 중일 때의 불투명도이다.")]
        public float ActiveAlpha = 0.5f;

        [Range(0f, 1f)]
        [Tooltip("HideWhenIdle이 꺼져 있을 때, 평소 상태에서의 옅은 불투명도이다.")]
        public float IdleAlpha = 0.08f;

        public Color LineColor = Color.white;

        [Tooltip("사거리 원을 그릴 때 사용할 머티리얼이다. URP와 호환되는 셰이더(Sprites-Default 등)를 쓰는 머티리얼을 지정해야 한다. 비어있으면 마젠타색 에러 표시가 뜬다.")]
        public Material LineMaterial;

        [Tooltip("사거리 원 테두리의 굵기이다. 유니티 LineRenderer 기본값(1)은 타일 크기 기준으로 너무 두꺼우므로 작은 값을 권장한다.")]
        public float LineWidth = 0.05f;
    }
}
