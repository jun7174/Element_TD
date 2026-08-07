// Assets/Scripts/Towers/RangeCircleDrawer.cs

using UnityEngine;

namespace ElementTD
{
    // LineRenderer에 원을 그리는 공용 로직만 담당한다.
    // TowerRangeIndicator와 PlacementRangePreview가 함께 사용해서 원 그리기 코드가 중복되지 않게 한다.
    public static class RangeCircleDrawer
    {
        public const int SegmentCount = 48;

        // LineRenderer의 도형 설정, 머티리얼, 선 굵기를 한 번에 구성한다.
        // 머티리얼을 지정하지 않으면 유니티가 마젠타색 에러 표시로 렌더링하므로,
        // settings에 URP 호환 머티리얼이 반드시 지정되어 있어야 한다.
        public static void Configure(LineRenderer lineRenderer, RangeIndicatorSettingsSO settings)
        {
            lineRenderer.loop = true;
            lineRenderer.positionCount = SegmentCount;
            lineRenderer.useWorldSpace = false;
            lineRenderer.material = settings.LineMaterial;
            lineRenderer.startWidth = settings.LineWidth;
            lineRenderer.endWidth = settings.LineWidth;
        }

        public static void Draw(LineRenderer lineRenderer, float radius)
        {
            for (int index = 0; index < SegmentCount; index++)
            {
                float angle = index * Mathf.PI * 2f / SegmentCount;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                lineRenderer.SetPosition(index, new Vector3(x, y, 0f));
            }
        }
    }
}
