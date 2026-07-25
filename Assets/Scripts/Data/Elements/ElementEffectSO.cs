// Assets/Scripts/Data/Elements/ElementEffectSO.cs

using UnityEngine;

namespace ElementTD
{
    // 원소 효과 하나를 나타내는 추상 데이터 규격이다.
    // 타입별 효과 슬롯에는 이 클래스를 상속한 구체적인 효과 데이터가 들어간다.
    // 실제 효과를 적용하는 로직은 타워와 몬스터 관련 클래스가 정의되는 다음 단계에서 추가한다.
    public abstract class ElementEffectSO : ScriptableObject
    {
        public string EffectName;

        [TextArea]
        public string EffectDescription;
    }
}
