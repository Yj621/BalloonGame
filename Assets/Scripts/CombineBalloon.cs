using System.Collections;
using UnityEngine;

public class CombineBalloon : MonoBehaviour
{
    public GameObject[] balloonLevels; // 각 레벨의 풍선을 저장하는 배열
    public int currentLevel = 0;       // 현재 풍선의 레벨
    private bool hasCollided = false;  // 충돌 여부를 확인하는 변수
    private int[] levelScores = { 2, 3, 5, 8, 12, 16, 20, 25, 32, 40 }; // 점수 배열
    public static int currentScore = 0; // 총 점수

    public string combineSoundName = "CombineSound"; // 결합 효과음 이름
    public string bombSoundName = "BombSound";       // 폭발 효과음 이름

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
                // 레벨 10인 경우 폭발 소리 재생 후 삭제
                if (currentLevel == 10)
                {
                    SoundController.Instance.PlaySoundEffect(bombSoundName);
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    return;
                }

                if (otherBalloon.hasCollided)
                    return;

                if (currentLevel < balloonLevels.Length)
                {
                    Vector2 newPosition = Vector2.Lerp(transform.position, collision.transform.position, 0.9f);

                    GameObject newBalloon = Instantiate(balloonLevels[currentLevel], newPosition, Quaternion.identity);
        

                    StartCoroutine(ReenableCollider(newBalloon.GetComponent<Collider2D>(), 0.5f));
                    hasCollided = true;
                    otherBalloon.hasCollided = true;

                    //합쳐져서 생긴 풍선의 isReleae값을 true로 초기화
                    Dead dead = newBalloon.GetComponent<Dead>();
                    dead.isReleased = true;

                    // 결합 효과음 재생
                    SoundController.Instance.PlaySoundEffect(combineSoundName);
                }

                currentScore += levelScores[currentLevel];
                uIController.t_Score.text = currentScore.ToString();

                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ReenableCollider(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider != null) collider.enabled = true;
    }

    public void SetCurrentScoreToZero()
    {
        currentScore = 0;
    }
}
