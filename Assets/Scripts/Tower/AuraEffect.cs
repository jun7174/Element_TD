// Assets/Scripts/Towers/AuraEffect.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 반경 안의 대상에게 상시 버프 또는 디버프를 적용하는 지원형 타워 공용 컴포넌트이다.
    // 버프 모드에서는 반경 안 전투형 타워에게 공격력 버프를 걸고,
    // 디버프 모드에서는 반경 안 몬스터에게 이동속도 감소를 건다.
    [RequireComponent(typeof(TowerBase))]
    public class AuraEffect : MonoBehaviour
    {
        [SerializeField]
        private AuraMode _mode;

        private TowerBase _towerBase;
        private readonly List<TowerBase> _buffedTowers = new List<TowerBase>();
        private readonly List<MonsterBase> _debuffedMonsters = new List<MonsterBase>();

        private void Awake()
        {
            _towerBase = GetComponent<TowerBase>();
        }

        private void Update()
        {
            if (_mode == AuraMode.Buff)
            {
                UpdateBuff();
            }
            else
            {
                UpdateDebuff();
            }
        }

        private void UpdateBuff()
        {
            List<TowerBase> targetsInRange = FindTowersInRange();

            foreach (TowerBase tower in targetsInRange)
            {
                if (!_buffedTowers.Contains(tower))
                {
                    tower.ApplyBuff(_towerBase.GetUpgradedCoreValue());
                    _buffedTowers.Add(tower);
                }
            }

            for (int index = _buffedTowers.Count - 1; index >= 0; index--)
            {
                TowerBase buffedTower = _buffedTowers[index];

                if (!targetsInRange.Contains(buffedTower))
                {
                    buffedTower.RemoveBuff();
                    _buffedTowers.RemoveAt(index);
                }
            }
        }

        private void UpdateDebuff()
        {
            List<MonsterBase> targetsInRange = FindMonstersInRange();

            foreach (MonsterBase monster in targetsInRange)
            {
                if (!_debuffedMonsters.Contains(monster))
                {
                    monster.ApplySpeedDebuff(_towerBase.GetUpgradedCoreValue());
                    _debuffedMonsters.Add(monster);
                }
            }

            for (int index = _debuffedMonsters.Count - 1; index >= 0; index--)
            {
                MonsterBase debuffedMonster = _debuffedMonsters[index];

                if (debuffedMonster == null)
                {
                    _debuffedMonsters.RemoveAt(index);
                    continue;
                }

                if (!targetsInRange.Contains(debuffedMonster))
                {
                    debuffedMonster.RemoveSpeedDebuff();
                    _debuffedMonsters.RemoveAt(index);
                }
            }
        }

        private List<TowerBase> FindTowersInRange()
        {
            List<TowerBase> towersInRange = new List<TowerBase>();

            foreach (TowerBase tower in TowerBase.ActiveTowers)
            {
                if (tower == _towerBase)
                {
                    continue;
                }

                float distance = Vector2.Distance(transform.position, tower.transform.position);

                if (distance <= _towerBase.TowerData.EffectRadius)
                {
                    towersInRange.Add(tower);
                }
            }

            return towersInRange;
        }

        private List<MonsterBase> FindMonstersInRange()
        {
            List<MonsterBase> monstersInRange = new List<MonsterBase>();

            foreach (MonsterBase monster in MonsterBase.ActiveMonsters)
            {
                float distance = Vector2.Distance(transform.position, monster.transform.position);

                if (distance <= _towerBase.TowerData.EffectRadius)
                {
                    monstersInRange.Add(monster);
                }
            }

            return monstersInRange;
        }
    }
}
