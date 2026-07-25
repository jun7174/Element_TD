// Assets/Scripts/Data/Towers/TargetingStrategySO.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 사거리 안 몬스터 후보 목록을 받아서 실제로 공격할 대상 하나를 고르는 규격이다.
    // 구체적인 선정 방식은 이를 상속한 클래스가 정한다.
    public abstract class TargetingStrategySO : ScriptableObject
    {
        public abstract MonsterBase SelectTarget(Vector2 towerPosition, List<MonsterBase> candidates);
    }
}
