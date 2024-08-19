using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HeartShapeCollider : MonoBehaviour
{
    void Start()
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();

        // EdgeCollider2D의 점 가져오기
        Vector2[] edgePoints = edgeCollider.points;

        // PolygonCollider2D의 점 설정
        polygonCollider.points = edgePoints;
    }
}

