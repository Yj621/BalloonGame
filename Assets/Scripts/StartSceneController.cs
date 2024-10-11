using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public GameObject rankPanel;
    public GameObject blockingPanel;
    private bool isPanel;
    // Start is called before the first frame update
    void Start()
    {
        blockingPanel.SetActive(false);
        rankPanel.SetActive(false);
        isPanel = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Play Scene");
    }

    public void Rank()
    {
        rankPanel.SetActive(true);
        OnBlockingPanel();
    }

    public void CloseRankPanel()
    {
        rankPanel.SetActive(false);
        OffBlockingPanel();
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
