using UnityEngine;
using System.Collections;

public class TestMonster : MonoBehaviour
{
    [SerializeField]
    float timeOffset = 1f; // 이동시간 = 거리 * timeOffset

    Transform[] wayPoints; // 이동 경로 정보
    int wayPointCount; // 이동 경로 개수
    int currentIndex = 0; // 현재 목표지점 인덱스

    public void Setup(Transform[] wayPoints)
    {
        // 적 이동 경로(WayPoints) 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적 위치를 첫 번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        currentIndex++;

        // 적 이동 제어
        StartCoroutine(nameof(Process));
    }

    private void Update()
    {
        //적 오브젝트 회전
        transform.Rotate(Vector3.forward, 360 * Time.deltaTime);
    }

    IEnumerator Process()
    {
        while (true)
        {
            //현재 위치에서 목표 위치(WayPoint)까지 이동
            yield return StartCoroutine(MoveAToB(transform.position, wayPoints[currentIndex].position));

            //다음 이동 위치 (WayPoint) 설정
            if (currentIndex < wayPointCount - 1) currentIndex++;
            else
            {
                Destroy(gameObject);// TODO : 몬스터 사망 이벤트(골드획득)
            }
        }
    }

    IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0f;
        float moveTime = Vector3.Distance(start, end) * timeOffset;

        while (percent < 1f)
        {
            percent += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

}
