// Assets/Scripts/Towers/IncomeGenerator.cs

using UnityEngine;

namespace ElementTD
{
    // 경제형 타워가 일정 주기로 골드를 생산하는 것을 담당한다.
    // 골드를 실제로 저장하는 경제 시스템은 아직 없으므로, 지금은 생산량을 콘솔에 출력만 한다.
    [RequireComponent(typeof(TowerBase))]
    public class IncomeGenerator : MonoBehaviour
    {
        private const float TickInterval = 1f;

        private TowerBase _towerBase;
        private float _tickTimer;

        private void Awake()
        {
            _towerBase = GetComponent<TowerBase>();
        }

        private void Update()
        {
            _tickTimer += Time.deltaTime;

            if (_tickTimer < TickInterval)
            {
                return;
            }

            _tickTimer -= TickInterval;
            GenerateGold();
        }

        private void GenerateGold()
        {
            float goldAmount = CalculateGoldAmount();

            Debug.Log(_towerBase.TowerData.TowerName + " 골드 생산 " + goldAmount);
        }

        private float CalculateGoldAmount()
        {
            TowerDataSO towerData = _towerBase.TowerData;
            ElementEffectSO currentEffect = _towerBase.GetCurrentElementEffect();

            if (currentEffect is StatModifierEffectSO statModifierEffect && statModifierEffect.TargetStat == TargetStat.CoreValue)
            {
                return towerData.GoldPerSecond * (1f + statModifierEffect.ModifierValue);
            }

            return towerData.GoldPerSecond;
        }
    }
}
