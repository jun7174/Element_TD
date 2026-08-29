// Assets/Scripts/UI/WorldToScreenUIPositioner.cs

using UnityEngine;

namespace ElementTD
{
    // 월드 좌표를 화면 좌표로 변환해서 UI 패널을 그 위치로 옮기는 공용 유틸리티이다.
    // Screen Space - Overlay 캔버스 기준으로 동작하며, 패널의 피벗을 기준으로
    // 화면 밖으로 나가지 않도록 위치를 보정한다.
    public static class WorldToScreenUIPositioner
    {
        // worldPosition을 화면 좌표로 변환한 뒤 verticalOffset(픽셀 단위)만큼 위로 올리고,
        // panelRectTransform이 화면 밖으로 나가지 않도록 보정해서 그 위치에 배치한다.
        public static void PositionAboveWorldPoint(RectTransform panelRectTransform, Vector3 worldPosition, float verticalOffset)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition.y += verticalOffset;

            Rect rect = panelRectTransform.rect;
            Vector2 pivot = panelRectTransform.pivot;

            float minX = rect.width * pivot.x;
            float maxX = Screen.width - rect.width * (1f - pivot.x);
            float minY = rect.height * pivot.y;
            float maxY = Screen.height - rect.height * (1f - pivot.y);

            float clampedX = Mathf.Clamp(screenPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(screenPosition.y, minY, maxY);

            panelRectTransform.position = new Vector3(clampedX, clampedY, panelRectTransform.position.z);
        }
    }
}
