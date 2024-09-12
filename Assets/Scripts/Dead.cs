using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public bool isInitialized = false;
    UIController uIController;

    private void Start() 
    {
        uIController = FindAnyObjectByType<UIController>();    
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
        }
    }
}
