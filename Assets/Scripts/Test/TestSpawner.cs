using UnityEngine;
using System.Collections;

public class TestSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab; // 적 프리펩
    [SerializeField]
    float      spawnTime;   // 적 생성 주기
    [SerializeField]
    Transform[] wayPoints;  // 현재 스테이지의 이동경로

    private void Awake()
    {
        StartCoroutine(nameof(SpawnEnemy));
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);                // 적 오브젝트 생성
            TestMonster enemy = clone.GetComponent<TestMonster>();      // 방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(wayPoints);         // wayPoints 정보를 매개변수로 Setup() 메소드 호출

            yield return new WaitForSeconds(spawnTime);                 // spawnTime 시간 동안 대기
        }
    }
}
