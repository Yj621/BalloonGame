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

        // EdgeCollider2D�� �� ��������
        Vector2[] edgePoints = edgeCollider.points;

        // PolygonCollider2D�� �� ����
        polygonCollider.points = edgePoints;
    }
}

