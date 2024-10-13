using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartSceneController : MonoBehaviour
{
    public GameObject rankPanel;
    public GameObject blockingPanel;
    public TextMeshProUGUI[] scores;
    private bool isPanel;
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
    void SaveHighScore()
    {
        // 현재 게임의 점수를 불러옴
        int newscore = PlayerPrefs.GetInt("ResultScore", 0);

        //점수를 저장할 배열
        List<int> highScores = new List<int>();


        //기존 저장된 점수를 불러옴
        for(int i = 0; i < scores.Length; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("HighScore" + i, 0));
        }

        //새 점수 추가
        highScores.Add(newscore);
        //리스트에 있는 값들을 내림차순으로 정렬(a,b는 앞에 있는 값과 뒤에 있는 값을 비교하려고 사용)
        //b가 a보다 크면 앞쪽으로 배치한다는 의미
        //a.CompareTo(b)라고 하면 내림차순으로 정렬
        highScores.Sort((a, b) => b.CompareTo(a));

        //상위 점수를 남기기 위해 리스트 배열 크기만큼 잘라서 추출
        //GetRange(int index, intCount) : index위치부터 시작해서 count개의 요소를 새로운 리스트로 반환
        //Mathf.Min()함수는 전달된 두값 중에서 더 작은 값 반환
        highScores = highScores.GetRange(0, Mathf.Min(scores.Length, highScores.Count));

        //정렬된 점수를 다시 저장
        for(int i=0; i<highScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, highScores[i]);
        }

        //Text에 점수 표시
        for(int i = 0; i<scores.Length; i++)
        {
            if(i < highScores.Count)
            {
                scores[i].text = (i+1)+". " + highScores[i].ToString();
            }
            else
            {
                scores[i].text = (i+1)+". ---";
            }
        }
    }
    
}
