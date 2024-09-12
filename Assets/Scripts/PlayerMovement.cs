using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject[] balloonPrefabs; // 각 단계의 풍선 프리팹 배열 (빨주노초파 순서)
    public GameObject kodari;
    public GameObject strap;
    public GameObject balloonCreate;
    public GameObject deadLine;

    private GameObject currentBalloon;
    private ConstantForce2D consForce;
    private Rigidbody2D balloonRb;

    private float minX = -5.77f; // x축의 최소 위치
    private float maxX = 5.77f;  // x축의 최대 위치
    private float upwardForce = 10.0f; // 위쪽으로 가하는 힘의 크기

    private bool balloonReleased = false; // 풍선이 분리되었는지 여부 체크
    private List<int> balloonQueue = new List<int>(); // 랜덤 풍선 큐
    private int currentSetIndex = 0; // 현재 묶음 인덱스

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
            if (Input.GetMouseButtonDown(0) && !balloonReleased)
            {
                ReleaseBalloon();
                ApplyUpwardForce();
                dead.Initialize();
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
    }

    private void SpawnNewBalloon()
    {
        Vector3 spawnPosition = balloonCreate.transform.position;

        if (balloonQueue.Count == 0)
        {
            GenerateBalloonQueue(); // 모든 풍선이 사용된 경우 새로운 묶음 생성
        }

        int nextBalloonIndex = balloonQueue[0];
        balloonQueue.RemoveAt(0);

        // 새로운 풍선을 생성하고 이를 currentBalloon에 할당
        currentBalloon = Instantiate(balloonPrefabs[nextBalloonIndex], spawnPosition, Quaternion.identity);
        currentBalloon.transform.SetParent(balloonCreate.transform); // "Balloon" 오브젝트의 자식으로 설정

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
        // 빨주노초파 묶음 생성
        int[] balloonSet = { 0, 1, 2, 3, 4 };
        List<int> shuffledSet = new List<int>(balloonSet);

        for (int i = 0; i < balloonSet.Length; i++)
        {
            int randIndex = Random.Range(0, shuffledSet.Count);
            balloonQueue.Add(shuffledSet[randIndex]);
            shuffledSet.RemoveAt(randIndex);
        }
    }

    private GameObject FindChildWithTag(Transform parent, string tag)
    {
        // 부모 오브젝트의 모든 자식 오브젝트를 순회
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
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
