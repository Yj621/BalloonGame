using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject blockingPanel;
    public GameObject blockingimg;
    public GameObject menuPanel;
    public bool isPanel = false;
    public TextMeshProUGUI t_Score;

    public GameObject uiGameOver;

    void Start()
    {
        menuPanel.SetActive(false);
        blockingPanel.SetActive(false);
        uiGameOver.SetActive(false);
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        uiGameOver.SetActive(true);
        OnBlockingImg();
    }

    public void OnBtnMenu()
    {
        OnBlockingPanel();
        menuPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Play Scene");
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Start Scene");
    }
    public void OnRestart()
    {
        // 현재 씬 다시 불러오기
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnBlockingPanel()
    {
        blockingPanel.SetActive(true);
        isPanel = true;
    }

    public void OffBlockingPanel()
    {
        blockingPanel.SetActive(false);
        isPanel = false;        
    }

    public void OnBlockingImg()
    {
        blockingimg.SetActive(true);
        isPanel = true;
    }

    public void OffBlockingImg()
    {
        blockingimg.SetActive(false);
        isPanel = false;        
    }

}
