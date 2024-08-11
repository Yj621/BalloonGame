using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject balloon;
    private ConstantForce2D consForce;

    private float minX = -6.56f; // x축의 최소 위치
    private float maxX = 6.65f;  // x축의 최대 위치
    private float upwardForce = 10.0f; // 위쪽으로 가하는 힘의 크기
    private void Start()
    {
        consForce = balloon.GetComponent<ConstantForce2D>();
        if (consForce == null)
        {
            consForce = balloon.AddComponent<ConstantForce2D>(); // ConstantForce2D 컴포넌트 추가
        }

        Rigidbody2D rb = balloon.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = balloon.AddComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 추가
        }
    }


    private void Update()
    {
        // 마우스를 따라 수평으로 이동
        FollowMouse();

        // 마우스 클릭 시 위로 힘을 가함
        if (Input.GetMouseButtonDown(0))
        {
            ApplyUpwardForce();
            Debug.Log("Test");
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
        consForce.force = new Vector2(0, upwardForce);
    }
}
