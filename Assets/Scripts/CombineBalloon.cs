using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineBalloon : MonoBehaviour
{
    public GameObject[] balloonLevels; // �� ������ ǳ�� ������ �迭
    public int currentLevel = 0; // ���� ǳ���� ����
    private bool hasCollided = false; // �浹 üũ ����

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �̹� �浹 ó���� ��� ����
        if (hasCollided)
            return;

        // �浹�� ������Ʈ�� ���� �±����� Ȯ��
        if (collision.gameObject.CompareTag("Balloon"))
        {
            CombineBalloon otherBalloon = collision.gameObject.GetComponent<CombineBalloon>();

            // �� ǳ���� ���� �������� Ȯ��
            if (otherBalloon != null && otherBalloon.currentLevel == this.currentLevel)
            {
                // �̹� �浹 ó���� ��� ����
                if (otherBalloon.hasCollided)
                    return;

                // ���� ������ ������ ������ �ƴ� ��쿡�� ���� �ܰ��� ǳ���� ����
                if (currentLevel < balloonLevels.Length - 1)
                {
                    
                    Vector2 newPosition = (transform.position + collision.transform.position) / 2;
                    GameObject newBalloon = Instantiate(balloonLevels[currentLevel + 1], newPosition, Quaternion.identity);

                    // ���� ������ ǳ���� �浹 ó�� ��Ȱ��ȭ �� ���� �ð� �� Ȱ��ȭ
                    Collider2D newBalloonCollider = newBalloon.GetComponent<Collider2D>();
                    if (newBalloonCollider != null)
                    {
                        newBalloonCollider.enabled = false;
                        StartCoroutine(ReenableCollider(newBalloonCollider, 0.1f)); // 0.1�� �� �浹 ó�� Ȱ��ȭ
                    }
                    Debug.Log(balloonLevels.Length);
                    // ���� ǳ���� �浹�� ǳ���� �߰� �浹���� �ʵ��� ����(ǳ���� ���� �����ڸ��� �߰� �浹�� ǳ���� ������ ������ �� �ɸ�)
                    hasCollided = true;
                    otherBalloon.hasCollided = true;
                }
                else
                {
                    //������ ������ ǳ�� ����@@@@@@@@@@@@@@
                }

                // �浹�� �� ǳ�� ����
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ReenableCollider(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }
}
