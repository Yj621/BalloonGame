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

    private float minX = -5.77f; // x축의 최소 위치
    private float maxX = 5.77f;  // x축의 최대 위치
    private float upwardForce = 10.0f; // 위쪽으로 가하는 힘의 크기

    private bool balloonReleased = false; // 풍선이 분리되었는지 여부 체크



    UIController uIController;
    Dead dead;

    private void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
        dead = FindAnyObjectByType<Dead>();
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
        
        Debug.Log(balloonReleased);
    }
    

    public void BtnClick()
    {
        if (releaseBalloonBtn.interactable)
        {
            OnclickMouse();
            StartCoroutine(DisableButtonTemporarily());
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
                ReleaseBalloon();
                ApplyUpwardForce();
                dead.Initialize();
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = putSprite;
            }
        }
    }

    private void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = player.transform.position;
        newPos.x = Mathf.Clamp(mousePos.x, minX, maxX);
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
        
        // 모든 풍선들을 찾아서 currentBalloon이 아닌 경우 dead.isInitialized = true로 설정
        foreach (GameObject balloon in GameObject.FindGameObjectsWithTag("Balloon"))
        {
            if (balloon != currentBalloon)
            {
                Dead deadScript = balloon.GetComponent<Dead>();
                if (deadScript != null)
                {
                    deadScript.isInitialized = true;
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
        currentBalloon = Instantiate(balloonPrefabs[nextBalloonIndex], spawnPosition, Quaternion.identity);
        currentBalloon.transform.SetParent(balloonCreate.transform);

        UpdateNextBalloonUI();
        // realBalloon을 찾기
        GameObject realBalloon = FindChildWithTag(currentBalloon.transform, "Balloon");

        if (realBalloon != null)
        {
            // realBalloon의 ConstantForce2D 설정
            consForce = realBalloon.GetComponent<ConstantForce2D>();
            if (consForce == null)
            {
                consForce = realBalloon.AddComponent<ConstantForce2D>();
            }
            consForce.enabled = false; // 기본적으로 비활성화

            // realBalloon의 Rigidbody2D 설정
            balloonRb = realBalloon.GetComponent<Rigidbody2D>();
            if (balloonRb == null)
            {
                balloonRb = realBalloon.AddComponent<Rigidbody2D>();
            }
            balloonRb.isKinematic = true; // 기본적으로 중력과 상호작용하지 않도록 설정
        }
        else
        {
            Debug.LogWarning("태그 'Balloon'을 가진 realBalloon을 찾을 수 없습니다.");
        }

        strap.SetActive(true);
        kodari.SetActive(true);

        balloonReleased = false;
    }

    private void GenerateBalloonQueue()
    {
        // 큐를 명확하게 초기화
        balloonQueue.Clear();
        
        // 빨주노초파 묶음 생성
        int[] balloonSet = { 0, 1, 2, 3, 4 };
        List<int> shuffledSet = new List<int>(balloonSet);

        // 풍선 인덱스를 중복 없이 섞어서 큐에 추가
        while (shuffledSet.Count > 0)
        {
            int randIndex = Random.Range(0, shuffledSet.Count);
            balloonQueue.Enqueue(shuffledSet[randIndex]); // Queue 사용
            shuffledSet.RemoveAt(randIndex); // 사용한 인덱스를 제거하여 중복 방지
        }
    }


    // 큐에서 다음 풍선을 표시하는 메서드
    private void UpdateNextBalloonUI()
    {
        // 큐가 비어 있으면 새로운 큐를 생성
        if (balloonQueue.Count == 0)
        {
            GenerateBalloonQueue();
        }

        // 큐에서 다음 풍선을 확인하여 스프라이트 갱신
        if (balloonQueue.Count > 0)
        {
            int nextBalloon = balloonQueue.Peek();  // 큐에서 다음 풍선 확인 (제거하지 않음)
            nextBalloonImage.sprite = balloonSprites[nextBalloon];  // 다음 풍선 스프라이트 UI 갱신
        }
    }


    private GameObject FindChildWithTag(Transform parent, string tag)
    {
        // 부모 오브젝트의 모든 자식 오브젝트를 순회
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))  // 자식이 해당 태그를 가지고 있는지 확인
            {
                return child.gameObject;  // 태그가 일치하면 그 자식 오브젝트 반환
            }

            // 자식 오브젝트의 자식 오브젝트를 재귀적으로 탐색
            GameObject result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }

        // 태그를 가진 오브젝트를 찾지 못했을 경우 null 반환
        return null;
    }

}
