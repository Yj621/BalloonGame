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

    public void Initialize()
    {
        isReleased = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (isReleased && other.gameObject.CompareTag("Dead"))
        {
            uIController.GameOver();
            resultScore = CombineBalloon.currentScore;
            GameManager.Instance.resultScoreText.text = resultScore.ToString();
            PlayerPrefs.SetInt("ResultScore", resultScore); 
        }
    }
}
