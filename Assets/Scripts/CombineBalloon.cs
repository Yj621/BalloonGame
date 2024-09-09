using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineBalloon : MonoBehaviour
{
    public GameObject[] balloonLevels; // 각 레벨의 풍선을 저장하는 배열
    public int currentLevel = 0; // 현재 풍선의 레벨
    private bool hasCollided = false; // 충돌 여부를 확인하는 변수
    private int[] levelScores = { 2, 4, 8, 16, 32, 64, 128 }; // 각 레벨에 따른 점수 배열
    private static int score = 0; // 게임 전체에서 기록되는 총 점수

    UIController uIController;

    void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 이미 충돌 처리가 된 경우에는 리턴
        if (hasCollided)
            return;

        // 충돌한 오브젝트의 태그가 "Balloon"인지 확인
        if (collision.gameObject.CompareTag("Balloon"))
        {
            CombineBalloon otherBalloon = collision.gameObject.GetComponent<CombineBalloon>();

            // 다른 풍선의 레벨이 나와 같은지 확인
            if (otherBalloon != null && otherBalloon.currentLevel == this.currentLevel)
            {
                // 이미 충돌 처리가 된 경우에는 리턴
                if (otherBalloon.hasCollided)
                    return;

                // 현재 풍선의 레벨이 마지막 레벨이 아닌 경우에만 다음 레벨의 풍선을 생성
                if (currentLevel < balloonLevels.Length - 1)
                {                    
                    Vector2 newPosition = (transform.position + collision.transform.position) / 2;
                    newPosition.y -= 0.7f; // 살짝 아래로 이동해서 충돌 후 겹치지 않도록 설정
                    GameObject newBalloon = Instantiate(balloonLevels[currentLevel + 1], newPosition, Quaternion.identity);

                    // 새로 생성된 풍선의 충돌 처리를 잠시 비활성화한 후 일정 시간이 지나면 다시 활성화
                    Collider2D newBalloonCollider = newBalloon.GetComponent<Collider2D>();
                    if (newBalloonCollider != null)
                    {
                        newBalloonCollider.enabled = false;
                        StartCoroutine(ReenableCollider(newBalloonCollider, 0.5f)); // 0.05초 후에 충돌 처리 활성화
                    }
                    Debug.Log(balloonLevels.Length);
                    // 현재 풍선과 다른 풍선의 충돌 처리를 비활성화(풍선이 더 이상 충돌하지 않도록 설정)
                    hasCollided = true;
                    otherBalloon.hasCollided = true;
                }
                /*
                else
                {
                    //최고 레벨인 풍선 처리@@@@@@@@@@@@@@
                }
                */
                score += levelScores[currentLevel];
                uIController.t_Score.text = score.ToString();
                Debug.Log("Score: " + score);
                
                // 충돌한 두 풍선 삭제
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

