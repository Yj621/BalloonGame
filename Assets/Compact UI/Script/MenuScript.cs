using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuScriptController
{
    public class MenuScript : MonoBehaviour
    {
        public GameObject CoinShop;
        public GameObject GemShop;
        public GameObject Messages;
        public GameObject Ranking;
        public GameObject Missions;
        public GameObject SignIn;
        public GameObject Lose;
        public GameObject Win;
        public GameObject Settings;
        public GameObject Rewarsds;

        public Button coinShop_btn;
        public Button gemShop_btn;
        public Button messages_btn;
        public Button ranking_btn;
        public Button missions_btn;
        public Button signIn_btn;
        public Button lose_btn;
        public Button win_btn;
        public Button settings_btn;
        public Button rewards_btn;

        public GameObject panel;
        private void Start()
        {
            panel.SetActive(false);
            coinShop_btn.onClick.AddListener(OpenCoinShop);
            gemShop_btn.onClick.AddListener(OpenGemShop);
            messages_btn.onClick.AddListener(OpenMessages);
            ranking_btn.onClick.AddListener(OpenRanking);
            missions_btn.onClick.AddListener(OpenMissions);
            signIn_btn.onClick.AddListener(OpenSignIn);
            lose_btn.onClick.AddListener(OpenLose);
            win_btn.onClick.AddListener(OpenWin);
            settings_btn.onClick.AddListener(OpenSettings);
            rewards_btn.onClick.AddListener(OpenRewards);
        }

        void OpenCoinShop()
        {
            CoinShop.SetActive(true);
            panel.SetActive(true);
        }
        void OpenGemShop()
        {
            GemShop.SetActive(true);
            panel.SetActive(true);
        }
        void OpenMessages()
        {
            Messages.SetActive(true);
            panel.SetActive(true);
        }
        void OpenRanking()
        {
            Ranking.SetActive(true);
            panel.SetActive(true);
        }
        void OpenMissions()
        {
            Missions.SetActive(true);
            panel.SetActive(true);
        }
        void OpenSignIn()
        {
            SignIn.SetActive(true);
            panel.SetActive(true);
        }
        void OpenLose()
        {
            Lose.SetActive(true);
            panel.SetActive(true);
        }
        void OpenWin()
        {
            Win.SetActive(true);
            panel.SetActive(true);
        }
        void OpenSettings()
        {
            Settings.SetActive(true);
            panel.SetActive(true);
        }
        void OpenRewards()
        {
            Rewarsds.SetActive(true);
            panel.SetActive(true);
        }
    }
}