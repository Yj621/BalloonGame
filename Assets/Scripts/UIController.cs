using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject blockingPanel;
    public GameObject menuPanel;
    public bool isPanel = false;

    void Start()
    {
        menuPanel.SetActive(false);
        blockingPanel.SetActive(false);
    }

    void Update()
    {
        
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

    public void MainScene()
    {

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

}
