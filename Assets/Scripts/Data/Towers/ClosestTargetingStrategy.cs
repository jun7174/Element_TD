// Assets/Scripts/Data/Towers/ClosestTargetingStrategy.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 후보 몬스터 중 타워와 가장 가까운 몬스터를 선택하는 전략이다.
    [CreateAssetMenu(fileName = "NewClosestTargetingStrategy", menuName = "ElementTD/Targeting/Closest")]
    public class ClosestTargetingStrategy : TargetingStrategySO
    {
        public override MonsterBase SelectTarget(Vector2 towerPosition, List<MonsterBase> candidates)
        {
            MonsterBase closestMonster = null;
            float closestDistance = float.MaxValue;

            foreach (MonsterBase candidate in candidates)
            {
                float distance = Vector2.Distance(towerPosition, candidate.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMonster = candidate;
                }
            }

            return closestMonster;
        }
    }
}
