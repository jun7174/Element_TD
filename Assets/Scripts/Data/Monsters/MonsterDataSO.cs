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
        public float BaseHealth;
        public float BaseMoveSpeed;

        [Tooltip("다섯 개 원소 각각에 대한 데미지 배율 목록이다. 다섯 항목이 모두 채워져야 한다.")]
        public List<ElementResistanceEntry> ElementResistances;

        public int GoldReward;
        public int DamageToPlayer;

        [Tooltip("슬로우, 스턴 등 상태이상에 면역인지 여부이다.")]
        public bool IsCrowdControlImmune;

        [Tooltip("보스 전용 연출과 체력바를 사용할지 여부이다.")]
        public bool IsBoss;
    }
}
