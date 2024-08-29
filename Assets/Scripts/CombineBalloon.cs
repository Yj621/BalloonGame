using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineBalloon : MonoBehaviour
{
    public GameObject[] balloonLevels; // 각 레벨의 풍선 프리팹 배열
    public int currentLevel = 0; // 현재 풍선의 레벨
    private bool hasCollided = false; // 충돌 체크 변수
    private int[] levelScores = { 2, 4, 8, 16, 32, 64, 128 }; //점수 배열
    private static int score = 0; // 총 점수를 저장하는 정적 변수

    UIController uIController;

    void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 이미 충돌 처리된 경우 리턴
        if (hasCollided)
            return;

        // 충돌한 오브젝트가 같은 태그인지 확인
        if (collision.gameObject.CompareTag("Balloon"))
        {
            CombineBalloon otherBalloon = collision.gameObject.GetComponent<CombineBalloon>();

            // 두 풍선이 같은 레벨인지 확인
            if (otherBalloon != null && otherBalloon.currentLevel == this.currentLevel)
            {
                // 이미 충돌 처리된 경우 리턴
                if (otherBalloon.hasCollided)
                    return;

                // 현재 레벨이 마지막 레벨이 아닌 경우에만 다음 단계의 풍선을 생성
                if (currentLevel < balloonLevels.Length - 1)
                {                    
                    Vector2 newPosition = (transform.position + collision.transform.position) / 2;
                    newPosition.y -= 0.1f; // 살짝 아래로 이동하여 충돌 문제 방지
                    GameObject newBalloon = Instantiate(balloonLevels[currentLevel + 1], newPosition, Quaternion.identity);

                    // 새로 생성된 풍선의 충돌 처리 비활성화 후 일정 시간 후 활성화
                    Collider2D newBalloonCollider = newBalloon.GetComponent<Collider2D>();
                    if (newBalloonCollider != null)
                    {
                        newBalloonCollider.enabled = false;
                        StartCoroutine(ReenableCollider(newBalloonCollider, 0.1f)); // 0.1초 후 충돌 처리 활성화
                    }
                    Debug.Log(balloonLevels.Length);
                    // 현재 풍선과 충돌한 풍선이 추가 충돌되지 않도록 설정(풍선이 새로 생기자마자 추가 충돌로 풍선이 여러개 생성돼 랙 걸림)
                    hasCollided = true;
                    otherBalloon.hasCollided = true;
                }
                /*
                else
                {
                    //마지막 레벨의 풍선 생성@@@@@@@@@@@@@@
                }
                */
                score += levelScores[currentLevel];
                uIController.t_Score.text = score.ToString();
                Debug.Log("Score: " + score);
                
                // 충돌한 두 풍선 제거
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ReenableCollider(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }
}
