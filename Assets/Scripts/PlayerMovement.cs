using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject balloonPrefab; // 풍선 프리팹에 대한 참조

    private GameObject currentBalloon; // 현재 활성화된 풍선
    private ConstantForce2D consForce;
    private Rigidbody2D balloonRb;

    private float minX = -6.56f; // x축의 최소 위치
    private float maxX = 6.65f;  // x축의 최대 위치
    private float upwardForce = 10.0f; // 위쪽으로 가하는 힘의 크기

    private bool balloonReleased = false; // 풍선이 분리되었는지 여부 체크

    private void Start()
    {
        // 첫 번째 풍선 생성
        SpawnNewBalloon();
    }

    private void Update()
    {
        // 마우스를 따라 수평으로 이동
        FollowMouse();

        // 마우스 클릭 시 위로 힘을 가함
        if (Input.GetMouseButtonDown(0) && !balloonReleased)
        {
            ReleaseBalloon();
            ApplyUpwardForce();
        }
    }

    private void FollowMouse()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // y값은 고정하고 x값만 변경
        Vector3 newPos = player.transform.position;
        newPos.x = Mathf.Clamp(mousePos.x, minX, maxX); // 플레이어의 x 위치를 제한된 범위 내로 설정

        player.transform.position = newPos; // 새로운 위치로 플레이어 이동
    }

    private void ApplyUpwardForce()
    {
        // 위로 힘을 가하여 올라가도록 설정
        balloonRb.AddForce(new Vector2(0, upwardForce), ForceMode2D.Impulse);
    }

    private void ReleaseBalloon()
    {
        // 풍선을 플레이어에서 분리
        currentBalloon.transform.SetParent(null);
        balloonReleased = true;

        // 다음 클릭 시 새로운 풍선을 생성할 수 있도록 설정
        Invoke(nameof(SpawnNewBalloon), 0.5f); // 0.5초 후에 새로운 풍선 생성
    }

    private void SpawnNewBalloon()
    {
        // 프리팹에서 새로운 풍선 인스턴스 생성
        currentBalloon = Instantiate(balloonPrefab, player.transform.position, Quaternion.identity);

        // 컴포넌트 설정
        consForce = currentBalloon.GetComponent<ConstantForce2D>();
        if (consForce == null)
        {
            consForce = currentBalloon.AddComponent<ConstantForce2D>(); // 없을 경우 ConstantForce2D 추가
        }

        balloonRb = currentBalloon.GetComponent<Rigidbody2D>();
        if (balloonRb == null)
        {
            balloonRb = currentBalloon.AddComponent<Rigidbody2D>(); // 없을 경우 Rigidbody2D 추가
        }

        // 새로운 풍선이 처음에는 플레이어를 따라가도록 설정
        currentBalloon.transform.SetParent(player.transform);

        // 풍선이 분리되었는지 여부 재설정
        balloonReleased = false;
    }
}
