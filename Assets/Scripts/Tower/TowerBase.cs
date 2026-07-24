// Assets/Scripts/Tower/TowerBase.cs

using System.Collections.Generic;
using UnityEngine;

namespace ElementTD
{
    // 타워의 공격, 원소, 설치를 담당한다.
    public class TowerBase : MonoBehaviour
    {
        [SerializeField]
        private TowerDataSO _towerData;

        [SerializeField]
        private List<ElementDataSO> _elementDataList;

        private TowerTier towerTier;

        public MonsterBase TestMonster;
        public ElementType TestElement = ElementType.None;

        private void Awake()
        {
            // TODO : 원소 목록 담아둘 스크립트 추가 예정

        }

        private void Start()
        {
            
        }
        //타워 데미지 테스트 함수
        [ContextMenu("Test_Atk")]
        void AtkTest()
        {
            Attack(TestMonster);
        }

        //타워 업그레이드 테스트 함수
        [ContextMenu("Test_TierUpgrade")]

        void TierTest()
        {
            switch (towerTier)
            {
                case (TowerTier.One):
                    towerTier = TowerTier.Two;
                    break;
                case (TowerTier.Two):
                    towerTier = TowerTier.Three;
                    break;
                case (TowerTier.Three):
                    towerTier = TowerTier.One;
                    break;
                default:
                    break;
            }

        }

            public void PlaceAt(Vector2 position)
        {
            transform.position = position;
        }

        public ElementType GetCurrentElement()
        {
            //TODO : 추후 타워 아래 타일의 원소 조사함수로 교체
            return TestElement;
        }

        // 몬스터 타격
        public void Attack(MonsterBase monster)
        {
            float damage = DamageCalculator(monster, GetCurrentElement());

        }

        //몬스터 타격시 원소 상성과 포탑의 데미지 계산하여 최종데미지 계산
        public float DamageCalculator(MonsterBase monster, ElementType towerElementType)
        {
            float baseDamage = _towerData.BaseDamage;
            float elementMultiplier = monster.GetResistanceMultiplier(towerElementType);

            float combatEffectMultiplier = 1f;
            switch (towerTier)
            {
                case (TowerTier.Two):
                    combatEffectMultiplier = 1f + _towerData.UpgradeData.FirstUpgrade.CoreValueIncreaseRate;
                    break;
                case (TowerTier.Three):
                    combatEffectMultiplier = 1f + _towerData.UpgradeData.FirstUpgrade.CoreValueIncreaseRate + _towerData.UpgradeData.SecondUpgrade.CoreValueIncreaseRate;
                    break;
                default:
                    combatEffectMultiplier = 1f;
                    break;
            }

            Debug.Log(combatEffectMultiplier + "타워배수");
            Debug.Log(elementMultiplier + "원소배수");
            Debug.Log(elementMultiplier * combatEffectMultiplier + "최종배수");

            return 0;
        }
    }
}