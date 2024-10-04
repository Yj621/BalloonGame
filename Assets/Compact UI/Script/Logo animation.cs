using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logoanimation : MonoBehaviour
{
    public Image logoImage; // 로고 이미지
    public float speed = 0.5f; // 이동 속도
    public float height = 10f; // 상하 반복 높이

    private Vector3 startPos;

    void Start()
    {
        startPos = logoImage.transform.localPosition;
    }

    void Update()
    {
        // 상하로 움직이는 애니메이션
        float newY = Mathf.Sin(Time.time * speed) * (height / 3);
        logoImage.transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
