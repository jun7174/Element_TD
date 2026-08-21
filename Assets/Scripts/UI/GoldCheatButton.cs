// Assets/Scripts/UI/GoldCheatButton.cs

using UnityEngine;

namespace ElementTD
{
    // 골드를 즉시 증감시키는 치트 버튼 하나의 동작을 담당한다.
    // 증감량이 다른 버튼마다 이 컴포넌트를 하나씩 붙이고 Amount 값만 다르게 지정한다.
    // 포트폴리오 테스트 편의용이며, 실제 배포 시 제거 대상이다.
    public class GoldCheatButton : MonoBehaviour
    {
        [SerializeField]
        private int _amount = 100;

        public void OnCheatButtonClicked()
        {
            EconomyManager.AddGold(_amount);
        }
    }
}
