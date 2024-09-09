using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject[] balloonPrefabs; // 각 단계의 풍선 프리팹 배열 (빨주노초파 순서)
    public GameObject kodari;
    public GameObject strap;

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

    private void Start()
    {
        uIController = FindAnyObjectByType<UIController>();
        GenerateBalloonQueue();
        SpawnNewBalloon();
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

        Invoke(nameof(SpawnNewBalloon), 0.5f);
    }

    private void SpawnNewBalloon()
    {
        Vector3 spawnPosition = new Vector3(player.transform.position.x, -5.8f, -0.6f);

        if (balloonQueue.Count == 0)
        {
            GenerateBalloonQueue(); // 모든 풍선이 사용된 경우 새로운 묶음 생성
        }

        int nextBalloonIndex = balloonQueue[0];
        balloonQueue.RemoveAt(0);

        currentBalloon = Instantiate(balloonPrefabs[nextBalloonIndex], spawnPosition, Quaternion.identity);
        strap.SetActive(true);
        kodari.SetActive(true);

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

        currentBalloon.transform.SetParent(player.transform);
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
}
