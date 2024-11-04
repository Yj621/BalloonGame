using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public bool isReleased = false;
    UIController uIController;
    public int resultScore;
    private static Dead instance;
    public static Dead Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start() 
    {
        uIController = FindAnyObjectByType<UIController>();
    }

    private void StopAll()
    {
        // 3D Rigidbody ∏ÿ√ﬂ±‚
        Rigidbody[] allRigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 2D Rigidbody ∏ÿ√ﬂ±‚
        Rigidbody2D[] allRigidbodies2D = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb2D in allRigidbodies2D)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
        }

        // 2D ConstantForce ∏ÿ√ﬂ±‚
        ConstantForce2D[] allConstantForces2D = FindObjectsOfType<ConstantForce2D>();
        foreach (ConstantForce2D cf2D in allConstantForces2D)
        {
            cf2D.force = Vector2.zero;
            cf2D.torque = 0f;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (isReleased && other.gameObject.CompareTag("Dead"))
        {
            StopAll();
            uIController.GameOver();
            resultScore = CombineBalloon.currentScore;
            GameManager.Instance.resultScoreText.text = resultScore.ToString();
            PlayerPrefs.SetInt("ResultScore", resultScore); 
        }
    }
}
