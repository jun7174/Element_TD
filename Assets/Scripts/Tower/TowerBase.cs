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
        private float _buffMultiplier = 1f;

        public TowerDataSO TowerData => _towerData;
        public float BuffMultiplier => _buffMultiplier;

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

        // 지휘타워 오라로부터 공격력 버프를 받는다.
        public void ApplyBuff(float amount)
        {
            _buffMultiplier = 1f + amount;
        }

        // 버프 범위를 벗어나면 호출되어 버프를 해제한다.
        public void RemoveBuff()
        {
            _buffMultiplier = 1f;
        }
    }
}
