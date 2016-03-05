using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

namespace Rafiwui.Networking
{
    public class CustomNetworkLobbyManager : NetworkLobbyManager
    {
        [Space]
        [Space]
        [Header("Custom Elements")]
        public string gameTitle;

        [Space]
        [Tooltip("Time in seconds until the game starts after every player is ready")]
        public int prematchCountdown;

        [Space]
        [Header("UI Elements")]
        public Text title;
        public Text countdown;
        public InputField lANGameName;
        public InputField lANGamePassword;

        #region Coroutines
        IEnumerator CountdownToStart()
        {
            yield return new WaitForEndOfFrame();
        }
        #endregion

        void Start()
        {
            title.text = gameTitle;
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
        public void OnClickHostButton()
        {
            StartMatchMaker();
            matchMaker.CreateMatch(lANGameName.text, (uint)maxPlayers, true, lANGamePassword.text, OnMatchCreate);
        }

        public void OnClickSearchButton()
        {

        }

        public void OnClickLocalServerButton()
        {

        }
        #endregion

        #region OnEndEditEvents
        public void OnEndEditLANHostName(string name)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickHostButton();
            }
        }
        #endregion

    }
}