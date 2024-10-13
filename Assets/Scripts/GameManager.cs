using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI resultScoreText;
    public int _score;
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }


    void Update()
    {
        _score = Convert.ToInt32(resultScoreText.text);
    }
}
