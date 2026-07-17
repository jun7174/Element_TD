# Phase 0 스크립트 설명 문서

기준 문서: `ElementTD_Design_Document.md`, `ElementTD_Script_Implementation_Order.md`
목적: Phase 0에서 작성한 데이터 계층 스크립트 9개가 각각 무슨 역할을 하는지 설명

---

## 전체 구조 한눈에 보기

Phase 0은 로직이 없는 순수 데이터 계층이다. 아래 아홉 개 스크립트는 크게 세 그룹으로 나뉜다.

- 열거형(enum) 3개: `ElementType`, `TowerTier`, `AttackPatternType`
- 스크립터블 오브젝트(SO) 5개: `ElementEffectSO`, `ElementDataSO`, `TowerUpgradeDataSO`, `TowerDataSO`, `MonsterDataSO`, `StageDataSO`
  (`ElementEffectSO`는 추상 클래스라 직접 에셋으로 만들 수는 없고, 나머지 다섯 개만 실제 에셋으로 생성 가능하다)

이 아홉 개는 이후 Phase 1부터 작성할 모든 로직 스크립트가 참조하는 기반 데이터다. 즉 타워가 어떤 스탯을 가지는지, 몬스터가 어떤 원소에 약한지, 스테이지가 어떻게 구성되는지를 전부 이 SO 에셋들이 들고 있고, 나중에 작성할 로직 스크립트는 이 값을 읽어서 계산만 한다.

---

## 1. ElementType.cs

**역할**: 원소 다섯 종류와 무속성 상태를 정의하는 열거형이다.

**값**: `None`, `Water`, `Fire`, `Grass`, `Dark`, `Light`

**쓰이는 곳**: `ElementDataSO`가 자신이 어떤 원소인지 식별할 때, `MonsterDataSO`의 저항 목록이 원소별 배율을 매길 때, 타일이 자신의 원소를 저장할 때(추후 `ElementTile.cs`에서 사용 예정) 전부 이 열거형을 참조한다.

`None`을 따로 둔 이유는 설계 문서 3.4항의 "무속성 타일 위에서는 무속성 데미지, 모든 몬스터에게 100 퍼센트 고정 배율"이라는 규칙을 표현하기 위해서다. 원소가 다섯 개뿐이라고 해서 다섯 개만 정의하면, 무속성 타일을 나타낼 방법이 없어진다.

---

## 2. TowerTier.cs

**역할**: 타워가 속하는 세 계층을 정의하는 열거형이다.

**값**: `Combat`(전투형), `Support`(지원형), `Economy`(경제형)

**쓰이는 곳**: `TowerDataSO`가 자신이 어느 계층인지 저장할 때 사용한다. 이후 로직 단계에서, 타워가 원소 타일 위에 설 때 `ElementDataSO`의 세 슬롯(`CombatEffect`, `SupportEffect`, `EconomyEffect`) 중 이 계층 값과 일치하는 슬롯만 조회하게 된다.

친화 태그 방식이 폐기되고 계층 기반 슬롯 구조로 바뀌었기 때문에, 이 열거형이 사실상 원소 효과를 결정하는 핵심 기준값이 된다.

---

## 3. AttackPatternType.cs

**역할**: 전투형 타워의 공격 방식을 정의하는 열거형이다.

**값**: `Basic`(기본공격), `RapidFire`(연사공격), `Splash`(범위공격), `Sniper`(저격형)

**쓰이는 곳**: `TowerDataSO`의 `AttackPattern` 필드에서 사용한다. 지원형과 경제형 타워에는 의미가 없는 값이라, 해당 타워 인스턴스에서는 사용하지 않고 기본값 상태로 둔다.

이 열거형은 원래 Phase 0 계획 목록에는 없었지만, `TowerDataSO`가 설계 문서에 명시된 "공격 패턴 종류" 필드를 가지려면 반드시 필요해서 추가로 작성했다.

---

## 4. ElementEffectSO.cs

**역할**: 원소 효과 하나를 나타내는 추상 데이터 규격이다. 직접 에셋으로 만들 수 없고, 이를 상속한 구체적인 효과 클래스만 실제 에셋이 된다.

**필드**:
- `EffectName`: 효과를 식별하는 이름
- `EffectDescription`: 효과에 대한 설명 문구 (여러 줄 입력 가능)

**쓰이는 곳**: `ElementDataSO`의 세 슬롯이 전부 이 타입을 참조한다.

지금은 이름과 설명만 담긴 껍데기 상태다. 실제로 "공격속도를 얼마나 올리는지", "몬스터에게 어떤 방식으로 적용하는지" 같은 적용 로직은 타워와 몬스터 클래스가 만들어지는 Phase 1 이후에 이 클래스를 상속한 구체 클래스(예를 들어 공격속도 증가 효과, 범용 보너스 효과)를 만들면서 채워 넣을 예정이다. 지금 로직까지 넣으면 아직 존재하지도 않는 타워 클래스를 참조해야 해서, 일부러 비워두었다.

이 구조 덕분에 나중에 새로운 종류의 효과(예를 들어 도트 데미지, 스턴)가 필요해져도 이 추상 클래스와 이를 사용하는 코드는 건드리지 않고, 상속받은 새 클래스만 추가하면 된다. 설계 문서 15항의 OCP 원칙이 여기서 실현된다.

---

## 5. ElementDataSO.cs

**역할**: 원소 하나(예를 들어 Water)에 대한 데이터를 담는다. 다섯 원소 각각 이 스크립트로 만든 에셋이 하나씩, 총 다섯 개 존재하게 된다.

**필드**:
- `Element`: 이 에셋이 어떤 원소를 나타내는지
- `CombatEffect`: 전투형 타워가 이 원소 타일 위에 있을 때 받는 효과
- `SupportEffect`: 지원형 타워가 이 원소 타일 위에 있을 때 받는 효과
- `EconomyEffect`: 경제형 타워가 이 원소 타일 위에 있을 때 받는 효과

**쓰이는 곳**: 타워가 타일 위에 설 때, 그 타일의 원소에 해당하는 `ElementDataSO`를 찾아서 타워 자신의 계층에 맞는 슬롯을 조회하게 된다 (이 조회 로직은 Phase 1의 `ElementSynergyEvaluator.cs`가 담당할 예정).

설계 문서 5.2항(A안)에 따라, 다섯 원소의 `SupportEffect`와 `EconomyEffect`는 지금 단계에서는 전부 같은 하나의 효과 에셋을 공유해서 참조하게 될 예정이다. `CombatEffect`만 원소마다 서로 다른 에셋을 참조한다.

---

## 6. TowerUpgradeDataSO.cs

**역할**: 타워 강화 1단계, 2단계 각각의 수치 증가율과 비용 비율을 담는다.

**구성 요소**:
- `TowerUpgradeLevelData` (직렬화 가능한 보조 클래스): `CoreValueIncreaseRate`(핵심 수치 증가율), `CostRatio`(설치비 대비 강화 비용 비율) 두 값을 가진다
- `TowerUpgradeDataSO` 본체: `FirstUpgrade`(1강 데이터), `SecondUpgrade`(2강 데이터) 두 개의 `TowerUpgradeLevelData`를 가진다

**쓰이는 곳**: `TowerDataSO`가 자신의 `UpgradeData` 필드로 이 에셋을 참조한다.

설계 문서 4.3항에서 강화는 항상 정확히 2단계로 고정되어 있으므로, 임의 개수의 강화 단계를 담을 수 있는 목록 형태 대신 `FirstUpgrade`와 `SecondUpgrade`라는 이름의 필드 두 개로 고정해서 만들었다. 나중에 강화 단계가 3단계로 늘어나는 일이 생기지 않는 한, 이 구조가 가장 단순하다.

---

## 7. TowerDataSO.cs

**역할**: 타워 하나의 기본 스탯을 담는다. 전투형, 지원형, 경제형 타워 전부 이 하나의 스크립트로 만든 에셋이 된다.

**공통 필드**:
- `TowerName`: 표시 이름
- `Tier`: 소속 계층
- `InstallCost`: 설치 비용
- `UpgradeData`: 연결된 강화 데이터 에셋

**전투형 전용 필드** (전투형 타워일 때만 값을 채운다):
- `BaseDamage`, `AttackSpeed`, `AttackRange`: 기본 데미지, 공격속도, 사거리
- `AttackPattern`: 공격 패턴 종류
- `SplashRadius`: 범위공격 타워에서만 사용하는 스플래시 반경

**지원형 전용 필드**:
- `EffectRadius`: 오라 반경
- `EffectAmount`: 버프 또는 디버프 효과량

**경제형 전용 필드**:
- `GoldPerSecond`: 초당 골드 생산량

**쓰이는 곳**: Phase 1의 `TowerBase.cs`가 이 데이터를 참조해서 실제 타워 오브젝트의 동작 기준값으로 사용하게 된다.

계층마다 실제로 쓰는 필드가 다르지만, 하나의 클래스로 합쳐두었다. 설계 문서의 데이터 구조표에서도 `TowerDataSO` 하나로 정의되어 있었기 때문에 그 결정을 그대로 따랐다. 인스펙터에서 헤더로 구간을 나눠서, 어떤 필드가 어느 계층 전용인지 한눈에 보이게 했다.

---

## 8. MonsterDataSO.cs

**역할**: 몬스터 하나의 스탯을 담는다. 원형 세 종류(일반형, 빠른형, 탱키형)의 다섯 원소 변종에 보스형까지 전부 이 하나의 스크립트로 만든 에셋이 된다.

**구성 요소**:
- `ElementResistanceEntry` (직렬화 가능한 보조 클래스): `Element`, `DamageMultiplier` 두 값을 가진다
- `MonsterDataSO` 본체: `BaseHealth`, `BaseMoveSpeed`, `ElementResistances`(다섯 개의 `ElementResistanceEntry` 목록), `GoldReward`, `DamageToPlayer`, `IsCrowdControlImmune`, `IsBoss`

**쓰이는 곳**: Phase 1의 `MonsterBase.cs`가 이동, 체력, 원소 저항 계산에 이 데이터를 사용하게 된다.

설계 논의에서 확정한 대로, 원형을 구분하는 별도 필드(예를 들어 `archetypeType`)는 넣지 않았다. 일반형, 빠른형, 탱키형이라는 구분은 코드에 존재하지 않고, 그냥 `BaseHealth`와 `BaseMoveSpeed` 값을 다르게 채운 서로 다른 에셋일 뿐이다.

`ElementResistances`는 다섯 항목이 전부 채워져야 정상 작동하지만, 이를 강제하는 검사 로직은 Phase 0에 포함하지 않았다. 로직 없이 필드만 정의하는 단계라는 원칙 때문이며, 검사 로직이 필요하면 이후 단계에서 추가할 수 있다.

---

## 9. StageDataSO.cs

**역할**: 스테이지 하나의 구성 정보를 담는다. 스테이지 세 개가 전부 이 하나의 스크립트로 만든 에셋이 된다.

**구성 요소**:
- `WaveMonsterEntry` (직렬화 가능한 보조 클래스): `Monster`, `Count` 두 값을 가진다
- `WaveData` (직렬화 가능한 보조 클래스): `MonsterEntries`(해당 웨이브에 등장하는 몬스터 종류와 수량 목록)를 가진다
- `StageDataSO` 본체: `TilemapReference`(고정 타일맵 프리팹), `ElementTileRatio`(원소 타일 비율), `WaveSequence`(열 개의 `WaveData` 목록), `DifficultyMultiplier`(난이도 배율), `StartingGold`(시작 골드)

**쓰이는 곳**: Phase 3의 `StageManager.cs`와 `WaveSpawner.cs`가 이 데이터를 읽어서 스테이지를 로드하고 웨이브를 진행하게 된다.

`ElementTileRatio`에는 인스펙터에서 0에서 1 사이 값만 입력하도록 범위 제한을 걸어두었다. 이는 설계 문서 8.4항의 0.3에서 0.4 사이 비율이라는 규칙을 실수로 벗어난 값이 입력되지 않도록 돕기 위함이다.

---

## 스크립트 간 참조 관계 요약

```
TowerDataSO ---- UpgradeData 참조 ----> TowerUpgradeDataSO
TowerDataSO ---- Tier 필드 사용 ------> TowerTier (열거형)
TowerDataSO ---- AttackPattern 사용 --> AttackPatternType (열거형)

ElementDataSO ---- Element 필드 사용 --> ElementType (열거형)
ElementDataSO ---- 세 슬롯 참조 ------> ElementEffectSO (추상 클래스)

MonsterDataSO ---- ElementResistances 항목의 Element 사용 --> ElementType (열거형)

StageDataSO ---- WaveSequence 안의 Monster 참조 --> MonsterDataSO
```

이 관계도가 곧 Phase 1부터 작성할 로직 스크립트들이 참조하게 될 기반 구조다.
