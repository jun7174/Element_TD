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

        private void OnDisable()
        {
            // 이 오라 자신이 판매되거나 파괴될 때, 지금까지 걸어둔 기여분을 전부 제거한다.
            // 이걸 안 하면 오라의 근원이 사라져도 이미 걸린 효과가 영구히 남는 버그가 생긴다.
            foreach (TowerBase tower in _buffedTowers)
            {
                if (tower != null)
                {
                    tower.RemoveBuff(this);
                }
            }

            _buffedTowers.Clear();

            foreach (MonsterBase monster in _debuffedMonsters)
            {
                if (monster != null)
                {
                    monster.RemoveSpeedDebuff(this);
                }
            }

            _debuffedMonsters.Clear();
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
            float currentAmount = _towerBase.GetUpgradedCoreValue();

            // 범위 안 대상에게 매 프레임 최신 수치를 다시 걸어준다.
            // ApplyBuff는 이 오라(this)를 키로 값을 덮어쓸 뿐이라, 반복 호출해도 항목이 늘어나지 않고
            // 강화로 수치가 바뀌면 다음 프레임부터 바로 반영된다.
            foreach (TowerBase tower in targetsInRange)
            {
                tower.ApplyBuff(this, currentAmount);

                if (!_buffedTowers.Contains(tower))
                {
                    _buffedTowers.Add(tower);
                }
            }

            for (int index = _buffedTowers.Count - 1; index >= 0; index--)
            {
                TowerBase buffedTower = _buffedTowers[index];

                if (!targetsInRange.Contains(buffedTower))
                {
                    buffedTower.RemoveBuff(this);
                    _buffedTowers.RemoveAt(index);
                }
            }
        }

        private void UpdateDebuff()
        {
            List<MonsterBase> targetsInRange = FindMonstersInRange();
            float currentAmount = _towerBase.GetUpgradedCoreValue();

            foreach (MonsterBase monster in targetsInRange)
            {
                monster.ApplySpeedDebuff(this, currentAmount);

                if (!_debuffedMonsters.Contains(monster))
                {
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
                    debuffedMonster.RemoveSpeedDebuff(this);
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
