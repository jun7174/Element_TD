# Phase 1 스크립트 설명 문서

기준 문서: `ElementTD_Design_Document.md`, `ElementTD_Script_Implementation_Order.md`
목적: Phase 1에서 작성한 최소 플레이 가능 루프 스크립트 7개가 각각 무슨 역할을 하는지 설명

---

## 전체 구조 한눈에 보기

Phase 1의 목표는 타워 하나, 몬스터 하나, 원소 하나만으로 핵심 판정 흐름(타일 원소 조회, 원소 효과 조회, 데미지 계산, 몬스터 원소 배율 적용)이 설계대로 정확히 맞물리는지 검증하는 것이다. 아래 일곱 개 스크립트는 두 그룹으로 나뉜다.

- Phase 0 계획에 있던 다섯 개: `ElementTile`, `TowerBase`, `ElementSynergyEvaluator`, `MonsterBase`, `DamageCalculator`
- 실제로 구현하다가 필요해서 추가한 두 개: `TargetStat`, `StatModifierEffectSO`

두 개가 추가된 이유는, 원소마다 서로 다른 스탯을 강화한다는 설계(공격속도, 데미지, 방어관통, 사거리, 치명타)를 실제로 표현하려면 `ElementEffectSO`를 상속한 구체적인 클래스가 필요했기 때문이다. Phase 0에서는 `ElementEffectSO`가 이름과 설명만 가진 빈 껍데기였는데, 이번에 처음으로 실제 내용을 채운 구현체가 생긴 것이다.

---

## 1. TargetStat.cs

**역할**: 원소 효과 하나가 정확히 어떤 스탯을 대상으로 하는지 나타내는 열거형이다.

**값**: `AttackSpeed`, `Damage`, `ArmorPenetration`, `Range`, `CritChance`, `CoreValue`

앞의 다섯 개는 전투형 타워의 개별 스탯이고(설계 문서 6.1항), `CoreValue`는 지원형과 경제형 타워가 공유하는 핵심 수치(버프량, 디버프량, 골드 생산량)를 뜻한다.

**쓰이는 곳**: `StatModifierEffectSO`가 자신이 어떤 스탯을 얼마나 바꾸는지 표현할 때 사용한다. `TowerBase.Attack()`이 이 값을 보고, 지금 조회된 효과가 데미지에 관여하는 효과인지 아닌지를 판단한다.

---

## 2. StatModifierEffectSO.cs

**역할**: `ElementEffectSO`를 상속한 첫 번째 구체 효과 클래스이다. 특정 스탯을 정해진 비율만큼 증가시키는 효과를 표현한다.

**필드**:
- `TargetStat`: 어떤 스탯을 대상으로 하는지
- `ModifierValue`: 증가율 (0.1이면 10 퍼센트 증가)

**쓰이는 곳**: 다섯 원소 각각의 전투형 슬롯에 이 클래스로 만든 서로 다른 에셋이 꽂히게 된다. 예를 들어 Water의 전투형 슬롯에는 `TargetStat`이 `AttackSpeed`인 에셋이, Fire의 전투형 슬롯에는 `TargetStat`이 `Damage`인 에셋이 들어간다. 지원형과 경제형이 현재 공유하는 범용 보너스도 이 클래스로 만들되, `TargetStat`을 `CoreValue`로 지정하면 된다.

이 클래스 하나로 다섯 원소의 전투형 효과와 범용 보너스를 전부 표현할 수 있어서, 원소마다 새로운 클래스를 만들 필요가 없다. 새로운 원소나 새로운 강화 대상이 필요해지면 `TargetStat`에 값 하나만 추가하면 되므로, 설계 문서 15항의 OCP 원칙에 맞는 구조다.

---

## 3. ElementTile.cs

**역할**: 타일 하나가 어떤 원소를 가지고 있는지 보유하고, 조회할 수 있는 API를 제공한다.

**필드**:
- `_element` (private): 이 타일의 원소. 인스펙터에서 직접 지정한다.

**API**:
- `Element` (읽기 전용 프로퍼티): 이 타일의 원소를 반환
- `FindTileAt(Vector2 position)` (정적 메서드): 주어진 위치에 겹쳐 있는 타일을 찾아서 반환. 콜라이더 겹침 판정을 이용한다.

**쓰이는 곳**: `TowerBase.GetCurrentElement()`가 자신의 위치로 이 메서드를 호출해서 지금 서 있는 타일의 원소를 알아낸다.

지금은 타일 하나마다 별도의 게임 오브젝트와 콜라이더를 두는 단순한 방식으로 만들었다. 실제 유니티 타일맵과 결합하는 방식(커스텀 타일 클래스나 좌표 기반 조회 테이블)은 스테이지 시스템을 작성하는 단계에서 다시 검토해야 한다. 지금 방식은 게임 오브젝트 하나만 만들면 바로 테스트가 가능해서 최소 루프 검증 목적에는 충분하지만, 타일 수가 많아지고 매 스테이지 입장마다 랜덤 생성을 해야 하는 시점에는 성능과 관리 편의성 양쪽에서 한계가 있다.

---

## 4. ElementSynergyEvaluator.cs

**역할**: 계층과 원소 조합을 받아서 해당하는 원소 효과를 조회하는 역할만 담당한다. 효과를 실제로 해석하고 적용하는 일은 이 클래스의 책임이 아니다.

**생성자**: 원소 데이터 목록(`List<ElementDataSO>`)을 받는다.

**API**:
- `GetEffect(ElementType element, TowerTier tier)`: 주어진 원소와 계층에 맞는 효과를 반환. 원소가 없거나(`None`) 일치하는 원소 데이터가 없으면 `null` 반환.

**쓰이는 곳**: `TowerBase.Attack()`이 공격할 때마다 이 메서드를 호출해서 지금 서 있는 타일의 원소에 맞는 효과를 가져온다.

이 클래스는 몬오비헤이비어가 아니라 순수한 일반 클래스로 만들었다. 프레임마다 갱신되거나 씬 생명주기를 따라야 할 이유가 없고, 오직 조회 계산만 하기 때문이다. 지금은 `TowerBase`가 원소 데이터 목록을 직접 인스펙터에서 채워서 이 클래스를 생성하는데, 이 목록을 어디서 가져오는지는 나중에 중앙 저장소 방식으로 바뀔 가능성이 있다. 그렇게 바뀌어도 이 클래스의 조회 로직 자체는 수정할 필요가 없다.

---

## 5. MonsterBase.cs

**역할**: 몬스터의 이동, 체력, 원소 저항 적용을 담당한다.

**필드**:
- `_monsterData` (private): 이 몬스터의 데이터
- `_targetWaypoint` (private): 이동 목표 지점
- `_currentHealth` (private): 현재 체력

**API**:
- `GetResistanceMultiplier(ElementType attackElement)`: 주어진 공격 원소에 대한 데미지 배율을 반환. 저항 목록에 없는 원소는 일반 배율인 1을 반환
- `TakeDamage(float damageAmount)`: 데미지를 받아 체력을 감소시키고, 체력이 0 이하가 되면 몬스터를 제거

**쓰이는 곳**: `TowerBase.Attack()`이 공격 대상으로 이 클래스의 인스턴스를 받아서 `GetResistanceMultiplier`와 `TakeDamage`를 호출한다.

이동은 지금 단계에서 단일 목표 지점을 향해 직선으로 이동하는 가장 단순한 방식으로 만들었다. 실제로는 여러 경유 지점을 순서대로 통과하고, 마지막 지점에 도달하면 플레이어 생명력을 깎는 누출 처리가 필요한데, 이 부분은 스테이지의 길 구조가 확정되는 단계에서 별도로 작성해야 한다.

---

## 6. DamageCalculator.cs

**역할**: 기본 데미지에 전투형 슬롯 효과 배율과 몬스터 원소 배율을 곱해서 최종 데미지를 계산하는 역할만 담당한다.

**API**:
- `CalculateFinalDamage(float baseDamage, float combatEffectMultiplier, float monsterResistanceMultiplier)`: 세 값을 곱해서 최종 데미지를 반환하는 정적 메서드

**쓰이는 곳**: `TowerBase.Attack()`이 필요한 세 숫자를 전부 알아낸 다음, 마지막 계산 단계에서 이 메서드를 호출한다.

이 클래스는 상태를 가지지 않는 순수 계산 함수만 담고 있다. 어떤 효과가 어떤 스탯을 대상으로 하는지 해석하는 일은 `TowerBase`가 미리 처리해서 숫자로 넘겨주고, 이 클래스는 그 숫자를 곱하는 것만 한다. 그래서 나중에 데미지 계산 공식 자체가 바뀌더라도(예를 들어 곱셈이 아니라 덧셈 방식으로 바뀐다거나) 이 클래스만 수정하면 되고, 어떤 효과가 데미지에 영향을 주는지 판단하는 로직은 건드릴 필요가 없다.

---

## 7. TowerBase.cs

**역할**: 타워의 배치와 현재 위치의 타일 원소 조회를 담당하고, 앞의 여러 클래스를 조합해서 실제 공격을 수행한다. Phase 1에서 사실상 전체 흐름을 지휘하는 클래스다.

**필드**:
- `_towerData` (private): 이 타워의 데이터
- `_elementDataList` (private): 원소 데이터 목록. 지금은 인스펙터에서 직접 채운다
- `_synergyEvaluator` (private): 위 목록으로 생성한 조회기

**API**:
- `PlaceAt(Vector2 position)`: 타워를 주어진 위치로 배치
- `GetCurrentElement()`: 현재 서 있는 타일의 원소를 반환. 타일이 없으면 무속성 반환
- `Attack(MonsterBase target)`: 공격을 수행하는 핵심 메서드

**`Attack` 메서드의 흐름**:
1. 현재 타일의 원소를 조회한다
2. 그 원소와 이 타워의 계층으로 효과를 조회한다
3. 조회된 효과가 `StatModifierEffectSO`이고 대상 스탯이 `Damage`이면, 데미지 배율을 계산한다. 그렇지 않으면 배율은 1로 둔다 (해당 원소가 데미지가 아닌 다른 스탯을 강화하는 경우)
4. 대상 몬스터에게서 이 원소에 대한 저항 배율을 받아온다
5. `DamageCalculator`로 최종 데미지를 계산한다
6. 콘솔에 결과를 출력하고, 대상 몬스터에게 데미지를 적용한다

이 흐름에서 `TowerBase`는 계산 자체를 하지 않고, 계산에 필요한 재료를 모아서 전달하는 역할만 한다. 실제 조회는 `ElementSynergyEvaluator`가, 실제 계산은 `DamageCalculator`가, 실제 저항 판정은 `MonsterBase`가 각각 담당한다. 설계 문서 15항의 단일 책임 원칙이 여기서 그대로 지켜진다.

---

## 스크립트 간 참조 관계 요약

```
TowerBase ---- 소유 ----> TowerDataSO (Phase 0)
TowerBase ---- 생성 ----> ElementSynergyEvaluator
TowerBase ---- 위치 조회 ----> ElementTile.FindTileAt
TowerBase ---- 효과 해석 후 ----> DamageCalculator.CalculateFinalDamage
TowerBase ---- 공격 대상 ----> MonsterBase

ElementSynergyEvaluator ---- 조회 대상 ----> ElementDataSO (Phase 0)
ElementDataSO ---- 슬롯 참조 ----> ElementEffectSO (Phase 0, 추상)
StatModifierEffectSO ---- 상속 ----> ElementEffectSO
StatModifierEffectSO ---- 필드 사용 ----> TargetStat

MonsterBase ---- 소유 ----> MonsterDataSO (Phase 0)
MonsterBase ---- 저항 조회 ----> ElementResistanceEntry (Phase 0, MonsterDataSO 내부)
```

---

## 알려진 단순화 목록 (다음 단계에서 다시 볼 부분)

Phase 1은 계산 로직 자체를 검증하는 것이 목표이기 때문에, 아래 세 가지는 일부러 가장 단순한 형태로만 만들어두었다. 각 항목이 왜 단순화되었고 나중에 무엇으로 바뀌어야 하는지는 대화 중 별도로 자세히 설명했다.

1. `ElementTile`이 타일 하나당 게임 오브젝트 하나를 쓰는 방식 — 스테이지 시스템 작성 시 유니티 타일맵과 결합하는 방식으로 재검토 필요
2. `MonsterBase`의 이동이 단일 목표 지점을 향한 직선 이동 방식 — 스테이지 시스템 작성 시 여러 경유 지점을 통과하는 경로 이동 방식으로 재작성 필요
3. `TowerBase`가 원소 데이터 목록을 직접 보유하는 방식 — 타워 로스터가 늘어나는 Phase 2 이후 중앙 저장소 방식으로 개선 여지 있음
