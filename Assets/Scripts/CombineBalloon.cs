using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineBalloon : MonoBehaviour
{
    public GameObject[] balloonLevels; // 각 레벨의 풍선을 저장하는 배열
    public int currentLevel = 0; // 현재 풍선의 레벨
    private bool hasCollided = false; // 충돌 여부를 확인하는 변수
    private int[] levelScores = { 2, 3, 5, 8, 12, 16, 20 }; // 각 레벨에 따른 점수 배열
    public static int currentScore = 0; // 게임 전체에서 기록되는 총 점수

    UIController uIController;

    void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided)
            return;

        if (collision.gameObject.CompareTag("Balloon"))
        {
            CombineBalloon otherBalloon = collision.gameObject.GetComponent<CombineBalloon>();

            if (otherBalloon != null && otherBalloon.currentLevel == this.currentLevel)
            {
                if (otherBalloon.hasCollided)
                    return;

                if (currentLevel < balloonLevels.Length - 1)
                {
                    Vector2 newPosition = ((transform.position + collision.transform.position)/2);                   
                    GameObject newBalloon = Instantiate(balloonLevels[currentLevel], newPosition, Quaternion.identity);

                    Collider2D newBalloonCollider = newBalloon.GetComponent<Collider2D>();
                    if (newBalloonCollider != null)
                    {
                        newBalloonCollider.enabled = false;
                        StartCoroutine(ReenableCollider(newBalloonCollider, 0.5f));
                    }

                    hasCollided = true;
                    otherBalloon.hasCollided = true;
                }

                currentScore += levelScores[currentLevel];

                uIController.t_Score.text = currentScore.ToString();


                Destroy(collision.gameObject);
                //Destroy(transform.parent.gameObject);
                Destroy(gameObject);
                Dead.Instance.isReleased = true;
            }
        }
    }

    private IEnumerator ReenableCollider(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }
}
