using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBGameSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            BBGameManager.Instance();
        }

        void Start()
        {
            BBGameManager.Instance().InitialiseAllManagers();
        }
 

        void Update()
        {
            if (!IsGameStarted()) return;

            BBGameManager.Instance().OnUpdate();
        }
        private void OnApplicationPause(bool pause)
        {
            if (!IsGameStarted()) return;

            BBGameManager.Instance().OnGamePause(pause);
        }

        private void OnApplicationQuit()
        {
            BBGameManager.Instance().OnExitGame();
        }

        bool IsGameStarted()
        {
            return BBGameManager.Instance().IsGameStarted;
        }
    }
}