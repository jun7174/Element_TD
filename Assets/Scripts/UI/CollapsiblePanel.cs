// Assets/Scripts/UI/CollapsiblePanel.cs

using UnityEngine;

namespace ElementTD
{
    // 버튼 쪽에 붙여서, 지정한 Content 오브젝트를 접었다 폈다 하는 범용 토글 컴포넌트이다.
    // 이 컴포넌트 자신이 붙은 오브젝트가 아니라 별도로 지정한 Content를 켜고 끄기 때문에,
    // Content를 꺼도 이 컴포넌트는 계속 살아있어서 다시 펼치는 클릭을 받을 수 있다.
    // 상점 패널뿐 아니라, 같은 방식으로 접었다 펴고 싶은 다른 패널에도 그대로 재사용 가능하다.
    public class CollapsiblePanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _content;

        [Tooltip("씬 시작 시 펼쳐진 상태로 시작할지 여부이다.")]
        [SerializeField]
        private bool _startExpanded = true;

        private bool _isExpanded;

        private void Awake()
        {
            _isExpanded = _startExpanded;
            _content.SetActive(_isExpanded);
        }

        // 토글 버튼의 OnClick에 연결해서 사용한다.
        public void OnToggleButtonClicked()
        {
            _isExpanded = !_isExpanded;
            _content.SetActive(_isExpanded);
        }
    }
}
