using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject[] balloonPrefabs; // 각 단계의 풍선 프리팹 배열 (빨주노초파 순서)
    public Queue<int> balloonQueue = new Queue<int>();  // 풍선 큐
    public Image nextBalloonImage;  // 다음 풍선을 표시할 UI Image
    public Sprite[] balloonSprites; // 풍선 이미지 배열
    public GameObject kodari;
    public GameObject strap;
    public GameObject balloonCreate;
    public GameObject deadLine;
    public Sprite holdSprite;
    public Sprite putSprite;
    public Button releaseBalloonBtn; //ReleaseBalloonBtn

    private GameObject currentBalloon;
    private ConstantForce2D consForce;
    private Rigidbody2D balloonRb;

    private float minX = -5.44f; // x축의 최소 위치
    private float maxX = 5.1f;  // x축의 최대
    private float upwardForce = 10.0f; // 위쪽으로 가하는 힘의 크기
    private float shuffledBalloonWidth; //셔플한 풍선의 x값

    private bool balloonReleased = false; // 풍선이 분리되었는지 여부 체크

    UIController uIController;

    private void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
        GenerateBalloonQueue();
        SpawnNewBalloon();
        balloonCreate = player.transform.Find("Balloon").gameObject; // "Balloon" 오브젝트 찾기
    }

    private void Update()
    {
        if (!uIController.isPanel)
        {
            FollowMouse();
        }
    }

    public void BtnClick()
    {
        if (releaseBalloonBtn.interactable)
        {
            OnclickMouse();
            StartCoroutine(DisableButtonTemporarily());
            ActivateCollider();
        }
    }

    private IEnumerator DisableButtonTemporarily()
    {
        releaseBalloonBtn.interactable = false;  // 버튼 비활성화
        yield return new WaitForSeconds(0.5f);  // 0.5초 후 다시 활성화
        releaseBalloonBtn.interactable = true;  // 버튼 활성화
    }

    private void OnclickMouse()
    {
        if (!uIController.isPanel)
        {
            if (!balloonReleased)
            {
                // ClickSound 재생
                SoundController.Instance.PlaySoundEffect("ClickSound");

                ReleaseBalloon();
                ApplyUpwardForce();
                Dead.Instance.isReleased = true;

                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = putSprite;
            }
        }
    }


    private void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = player.transform.position;
        newPos.x = Mathf.Clamp(mousePos.x, minX + (shuffledBalloonWidth / 2), maxX - (shuffledBalloonWidth / 2));
        player.transform.position = newPos;
    }

    private void ApplyUpwardForce()
    {
        balloonRb.AddForce(new Vector2(0, upwardForce), ForceMode2D.Impulse);
    }

    private void ReleaseBalloon()
    {
        currentBalloon.transform.SetParent(null);
        balloonReleased = true;

        strap.SetActive(false);
        kodari.SetActive(false);

        consForce.enabled = true;
        balloonRb.isKinematic = false;

        // 모든 풍선들을 찾아서 currentBalloon이 아닌 경우 dead.isReleased = true로 설정
        foreach (GameObject balloon in GameObject.FindGameObjectsWithTag("Balloon"))
        {
            if (balloon != currentBalloon)
            {
                if (Dead.Instance != null)
                {
                    Dead.Instance.isReleased = true;
                }
            }
        }

        Invoke(nameof(SpawnNewBalloon), 0.5f);
        UpdateNextBalloonUI();  // UI 업데이트
    }

    private void SpawnNewBalloon()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = holdSprite;

        Vector3 spawnPosition = balloonCreate.transform.position;

        // 큐가 비어 있으면 새로운 큐를 생성
        if (balloonQueue.Count == 0)
        {
            GenerateBalloonQueue();
        }

        int nextBalloonIndex = balloonQueue.Dequeue();  // Queue에서 요소 제거 및 반환

        // 새로운 풍선을 생성하고 이를 currentBalloon에 할당
        currentBalloon = Instantiate(balloonPrefabs[nextBalloonIndex], spawnPosition, Quaternion.Euler(0, 0, -90));
        currentBalloon.transform.SetParent(balloonCreate.transform);

        // 생성된 (손에서 놓기 전의)풍선 인스턴스의 PolygonCollider가 비활성화 상태로 시작
        PolygonCollider2D collider = currentBalloon.GetComponent<PolygonCollider2D>();
        collider.enabled = false;

        // EdgeCollider2D의 x축 길이 계산(player 축이 풍선 크기에 따라 움직일 수 있는 범위가 변하게 하기 위함)
        EdgeCollider2D edgeCollider = currentBalloon.GetComponent<EdgeCollider2D>();
        if (edgeCollider != null)
        {
            Vector2[] points = edgeCollider.points;

            float minX = float.MaxValue;
            float maxX = float.MinValue;

            foreach (Vector2 point in points)
            {
                float worldX = currentBalloon.transform.TransformPoint(point).x; // 로컬 좌표를 월드 좌표로 변환
                if (worldX < minX) minX = worldX;
                if (worldX > maxX) maxX = worldX;
            }

            shuffledBalloonWidth = maxX - minX; // x축 길이 계산
            edgeCollider.enabled = false; // 필요한 경우 비활성화
        }
        else
        {
            Debug.LogWarning("Current Balloon에는 EdgeCollider2D가 없습니다.");
            shuffledBalloonWidth = 0; // 기본값 설정
        }

        UpdateNextBalloonUI();

        if (currentBalloon != null)
        {
            consForce = currentBalloon.GetComponent<ConstantForce2D>();
            if (consForce == null)
            {
                consForce = currentBalloon.AddComponent<ConstantForce2D>();
            }
            consForce.enabled = false;

            balloonRb = currentBalloon.GetComponent<Rigidbody2D>();
            if (balloonRb == null)
            {
                balloonRb = currentBalloon.AddComponent<Rigidbody2D>();
            }
            balloonRb.isKinematic = true;

            kodari.GetComponent<MeshRenderer>().material = currentBalloon.GetComponent<MeshRenderer>().material;
        }

        strap.SetActive(true);
        kodari.SetActive(true);

        balloonReleased = false;
    }

    private void GenerateBalloonQueue()
    {
        balloonQueue.Clear();
        int[] balloonSet = { 0, 1, 2, 3, 4 };
        List<int> shuffledSet = new List<int>(balloonSet);

        while (shuffledSet.Count > 0)
        {
            int randIndex = Random.Range(0, shuffledSet.Count);
            balloonQueue.Enqueue(shuffledSet[randIndex]);
            shuffledSet.RemoveAt(randIndex);
        }
    }

    private void UpdateNextBalloonUI()
    {
        if (balloonQueue.Count == 0)
        {
            GenerateBalloonQueue();
        }

        if (balloonQueue.Count > 0)
        {
            int nextBalloon = balloonQueue.Peek();
            nextBalloonImage.sprite = balloonSprites[nextBalloon];
        }
    }

    private void ActivateCollider()
    {
        // 클릭할 때 풍선의 콜라이더 비활성화->활성화
        PolygonCollider2D collider = currentBalloon.GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
            collider.enabled = true; // 비활성화 상태에서 활성화
        }
        else
        {
            Debug.Log("ss");
        }
    }
}
