// Assets/Scripts/Towers/TowerBase.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 타워의 배치와 현재 위치의 타일 원소 조회를 담당하고,
    // 원소 시너지 조회 결과를 전투, 오라, 인컴 등 다른 컴포넌트에 제공한다.
    // 공격 실행 자체는 이 클래스의 책임이 아니며 TowerCombatController가 담당한다.
    public class TowerBase : MonoBehaviour
    {
        private static readonly List<TowerBase> s_activeTowers = new List<TowerBase>();

        public static IReadOnlyList<TowerBase> ActiveTowers => s_activeTowers;

        [SerializeField]
        private TowerDataSO _towerData;

        [Tooltip("게임에 존재하는 모든 원소 데이터 에셋 목록이다. 지금 단계에서는 인스펙터에서 직접 채워 넣는다.")]
        [SerializeField]
        private List<ElementDataSO> _elementDataList;

        private ElementSynergyEvaluator _synergyEvaluator;
        private readonly Dictionary<AuraEffect, float> _buffContributions = new Dictionary<AuraEffect, float>();
        private TowerUpgradeState _upgradeState = TowerUpgradeState.Base;

        public TowerDataSO TowerData => _towerData;
        public TowerUpgradeState UpgradeState => _upgradeState;

        // 지금 이 타워에 걸린 모든 버프 오라의 기여분을 더한 최종 배율이다.
        public float BuffMultiplier
        {
            get
            {
                float totalBonus = 0f;

                foreach (float amount in _buffContributions.Values)
                {
                    totalBonus += amount;
                }

                return 1f + totalBonus;
            }
        }

        private void Awake()
        {
            _synergyEvaluator = new ElementSynergyEvaluator(_elementDataList);
        }

        private void OnEnable()
        {
            s_activeTowers.Add(this);
        }

        private void OnDisable()
        {
            s_activeTowers.Remove(this);
        }

        // 타워를 주어진 위치로 이동시켜 배치한다.
        public void PlaceAt(Vector2 position)
        {
            transform.position = position;
        }

        // 현재 서 있는 타일의 원소를 조회한다.
        // 타일을 찾지 못하면 무속성을 반환한다.
        public ElementType GetCurrentElement()
        {
            return ElementTileQuery.GetElementAt(transform.position);
        }

        // 현재 서 있는 타일의 원소와 이 타워의 계층에 맞는 효과를 조회한다.
        // 원소가 없거나 일치하는 효과가 없으면 null을 반환한다.
        public ElementEffectSO GetCurrentElementEffect()
        {
            ElementType currentElement = GetCurrentElement();
            return _synergyEvaluator.GetEffect(currentElement, _towerData.Type);
        }

        // source 오라가 이 타워에게 거는 버프 수치를 갱신한다.
        // 매 프레임 다시 호출해도 같은 오라의 항목은 하나만 유지되며 값만 최신으로 덮어써진다.
        public void ApplyBuff(AuraEffect source, float amount)
        {
            _buffContributions[source] = amount;
        }

        // source 오라의 버프 기여분을 제거한다. 다른 오라의 기여분은 영향받지 않는다.
        public void RemoveBuff(AuraEffect source)
        {
            _buffContributions.Remove(source);
        }

        // TowerUpgradeController가 강화 성공 시 호출해서 강화 상태를 변경한다.
        public void SetUpgradeState(TowerUpgradeState newState)
        {
            _upgradeState = newState;
        }

        // 이 타워 계층의 핵심 수치에 강화 배율을 곱한 값을 반환한다.
        // 전투형은 데미지, 지원형은 효과량, 경제형은 골드 생산량이 핵심 수치이다.
        public float GetUpgradedCoreValue()
        {
            float baseValue = GetBaseCoreValue();
            float upgradeMultiplier = GetUpgradeMultiplier();
            return baseValue * upgradeMultiplier;
        }

        private float GetBaseCoreValue()
        {
            switch (_towerData.Type)
            {
                case TowerType.Combat:
                    return _towerData.BaseDamage;
                case TowerType.Support:
                    return _towerData.EffectAmount;
                case TowerType.Economy:
                    return _towerData.GoldPerSecond;
                default:
                    return 0f;
            }
        }

        private float GetUpgradeMultiplier()
        {
            if (_upgradeState == TowerUpgradeState.First)
            {
                return 1f + _towerData.UpgradeData.FirstUpgrade.CoreValueIncreaseRate;
            }

            if (_upgradeState == TowerUpgradeState.Second)
            {
                return 1f + _towerData.UpgradeData.SecondUpgrade.CoreValueIncreaseRate;
            }

            return 1f;
        }
    }
}
