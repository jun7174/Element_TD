// Assets/Scripts/Combat/SniperTracerSpawner.cs

using UnityEngine;

namespace ElementTD
{
    // 저격형 타워의 명중 트레이서 효과 생성을 요청받는다.
    // 씬에 이 컴포넌트를 하나 두면, 어디서든 정적 메서드로 트레이서 생성을 요청할 수 있다.
    public class SniperTracerSpawner : MonoBehaviour
    {
        private static SniperTracerSpawner s_instance;

        [SerializeField]
        private SniperTracerEffect _tracerPrefab;

        private void Awake()
        {
            s_instance = this;
        }

        // 시작점에서 끝점까지 트레이서 선을 생성한다.
        public static void Spawn(Vector3 startPosition, Vector3 endPosition)
        {
            if (s_instance == null)
            {
                return;
            }

            SniperTracerEffect instance = Instantiate(s_instance._tracerPrefab);
            instance.Show(startPosition, endPosition);
        }
    }
}
