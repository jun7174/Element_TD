// Assets/Scripts/Data/Monsters/MonsterDataSO.cs

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    [Serializable]
    public class ElementResistanceEntry
    {
        public ElementType Element;

        [Tooltip("이 원소 공격을 받았을 때 적용되는 데미지 배율이다. 1.0은 일반, 1.5는 약점, 0.5는 저항을 의미한다.")]
        public float DamageMultiplier;
    }

    [CreateAssetMenu(fileName = "NewMonsterData", menuName = "ElementTD/Monster Data")]
    public class MonsterDataSO : ScriptableObject
    {
        [Tooltip("이 몬스터의 원소이다. 스프라이트 색상 표시와 자동 저항/약점 계산에 쓰인다. 원소 테마가 없으면 None으로 둔다.")]
        public ElementType Element;

        public float BaseHealth;
        public float BaseMoveSpeed;

        [Tooltip("켜져 있으면 Element 값을 기준으로 MonsterElementRulesSO의 규칙에 따라 저항/약점이 자동으로 계산된다. " +
            "꺼져 있으면 아래 ElementResistances 목록을 직접 사용한다 (보스처럼 규칙에 맞지 않는 예외용).")]
        public bool UseAutomaticResistances = true;

        [Tooltip("UseAutomaticResistances가 꺼져 있을 때만 사용하는 수동 저항/약점 목록이다.")]
        public List<ElementResistanceEntry> ElementResistances;

        public int GoldReward;
        public int DamageToPlayer;

        [Tooltip("방어력이다. 데미지 계산 시 원소 배율을 적용한 뒤 고정 수치로 차감된다. 채우지 않으면 0으로 방어력이 없는 상태이다.")]
        public float Defense;

        [Tooltip("슬로우, 스턴 등 상태이상에 면역인지 여부이다.")]
        public bool IsCrowdControlImmune;

        [Tooltip("보스 전용 연출과 체력바를 사용할지 여부이다.")]
        public bool IsBoss;
    }
}
