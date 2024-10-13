using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public bool isInitialized = false;
    UIController uIController;
    public int resultScore;
    CombineBalloon combineBalloon;
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
        combineBalloon = FindAnyObjectByType<CombineBalloon>();
    }

    public void Initialize()
    {
        isInitialized = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (isInitialized && other.gameObject.CompareTag("Dead"))
        {
            Debug.Log("dead");
            uIController.GameOver();
            resultScore = GameManager.Instance._score;
            combineBalloon.score = resultScore;
            Debug.Log("resultScore : " + resultScore);
        }
    }
}
