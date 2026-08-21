// Assets/Scripts/Combat/FloatingDamageTextSpawner.cs

using UnityEngine;

namespace ElementTD
{
    // 몬스터가 데미지를 받을 때 위로 떠오르는 데미지 텍스트를 생성한다.
    // 씬에 이 컴포넌트를 하나 두면, 어디서든 정적 메서드로 텍스트 생성을 요청할 수 있다.
    public class FloatingDamageTextSpawner : MonoBehaviour
    {
        private const float SpawnHeightOffset = 0.3f;

        private static FloatingDamageTextSpawner s_instance;

        [SerializeField]
        private FloatingDamageText _floatingTextPrefab;

        [SerializeField]
        private ElementColorPaletteSO _colorPalette;

        private void Awake()
        {
            s_instance = this;
        }

        // 주어진 위치보다 살짝 위쪽에 데미지 텍스트를 생성한다. element에 따라 색상이 정해진다.
        public static void Spawn(Vector3 worldPosition, float damageAmount, ElementType element, bool isCriticalHit)
        {
            if (s_instance == null)
            {
                return;
            }

            Vector3 spawnPosition = worldPosition + Vector3.up * SpawnHeightOffset;
            Color color = s_instance._colorPalette.GetColor(element);
            FloatingDamageText instance = Instantiate(s_instance._floatingTextPrefab, spawnPosition, Quaternion.identity);
            instance.Setup(damageAmount, color, isCriticalHit);
        }
    }
}
