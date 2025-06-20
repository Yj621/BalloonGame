using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class StartSceneController : MonoBehaviour
{
    public static StartSceneController Instance;
    public AudioClip startSceneBGM; // 시작화면 BGM

    public GameObject rankPanel;
    public GameObject menuPanel;
    public GameObject blockingPanel;
    public TextMeshProUGUI[] scores;
    private bool isPanel;
    void Start()
    {
        blockingPanel.SetActive(false);
        rankPanel.SetActive(false);
        menuPanel.SetActive(false);
        isPanel = false;
        if (startSceneBGM != null)
        {
            SoundController.Instance.PlayBGM(SoundController.Instance.musicSource, startSceneBGM);
        }
    }

    void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하면 해당 객체를 파괴
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // 씬 전환 후에도 파괴되지 않도록 설정
        }
        /*
        else
        {
            Destroy(gameObject);
        }
        */
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartScene()
    {
        SceneManager.LoadScene("MobilePlay");
    }

    public void Rank()
    {
        rankPanel.SetActive(true);
        OnBlockingPanel();
        SaveHighScore();
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
    public void OnBtnMenu()
    {
        OnBlockingPanel();
        menuPanel.SetActive(true);
    }
    void SaveHighScore()
    {
        int newscore = PlayerPrefs.GetInt("ResultScore", 0);
        List<int> highScores = new List<int>();

        // 기존 저장된 점수 불러오기
        for (int i = 0; i < scores.Length; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("HighScore" + i, 0));
        }

        // 새 점수 추가
        highScores.Add(newscore);

        // 중복 점수 제거
        highScores = highScores.Distinct().ToList();

        // 점수 내림차순 정렬
        highScores.Sort((a, b) => b.CompareTo(a));

        // 상위 점수만 남기기
        highScores = highScores.GetRange(0, Mathf.Min(scores.Length, highScores.Count));

        // 정렬된 점수 다시 저장
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, highScores[i]);
        }

        // TextMeshPro에 점수 표시
        for (int i = 0; i < scores.Length; i++)
        {
            if (i < highScores.Count)
            {
                scores[i].text = (i + 1) + ". " + highScores[i].ToString();
            }
            else
            {
                scores[i].text = (i + 1) + ". 0";
            }
        }
    }   
    public void SaveHighScoreByOtherClass()
    {
        SaveHighScore();
    }
}
