using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

namespace Rafiwui.Networking
{
    public class CustomNetworkLobbyManager : NetworkLobbyManager
    {
        #region Public Inspector Variables

        [Space]
        [Space]
        [Header("Custom Elements")]
        public string title;

        [Space]
        [Tooltip("Time in seconds until the game starts after every player is ready")]
        public int prematchCountdown;

        [Space]
        [Header("UI Elements")]
        public Text gameTitle;
        public RectTransform mainPanel;
        public RectTransform searchServerPanel;
        public RectTransform serverLobbyPanel;
        public Text countdown;

        #endregion

        #region Privat Variables
        private RectTransform activePanel;
        #endregion

        #region Coroutines
        IEnumerator CountdownToStart()
        {
            yield return new WaitForEndOfFrame();
        }
        #endregion

        void Start()
        {
            gameTitle.text = title;
            activePanel = mainPanel;

            mainPanel.anchorMin = Vector2.zero;
            mainPanel.anchorMax = Vector2.one;
            searchServerPanel.anchorMin = Vector2.one;
            searchServerPanel.anchorMax = Vector2.one * 2;
            serverLobbyPanel.anchorMin = Vector2.one;
            serverLobbyPanel.anchorMax = Vector2.one * 2;
        }

        #region OverrideFunctions
        //TODO:
        //-start countdown
        //-change scene
        public override void OnLobbyServerPlayersReady()
        {
            base.OnLobbyServerPlayersReady();
        }
        #endregion

        #region OnClickButtonEvents
        #endregion

        #region OnEndEditEvents
        #endregion

        private void ChangePanelTo(RectTransform panel)
        {
            if(activePanel.gameObject.tag != TagManager.MainPanel)
            {
                activePanel.anchorMin = Vector2.one;
                activePanel.anchorMax = Vector2.one * 2;
            }

            activePanel = panel;
            panel.anchorMin = Vector2.zero;
            panel.anchorMax = Vector2.one;
        }

    }
}