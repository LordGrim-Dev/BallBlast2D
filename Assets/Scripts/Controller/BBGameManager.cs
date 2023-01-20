using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEditor;
using UnityEngine;



namespace BallBlast
{

    public class BBGameManager : SingletonObserverBase<BBGameManager>
    {
        public bool IsGamePause { get; private set; }
        public bool IsGameStarted { get; private set; }
        public bool IsGameOver { get; private set; }

        public void InitialiseAllManagers()
        {
            BBManagerMediator mediator = BBManagerMediator.Instance();
            mediator.PlayerController.Init();
            mediator.BulletManagerMB.Init();
            mediator.BallManagerMB.Init();
            mediator.BorderRefHolderMB.Init();

            UI.BBUIManager.Instance().ShowIntroUI();
        }

        public override void Init()
        {
            IsGameOver = IsGameStarted = IsGamePause = false;

            base.Init();
            config.BBConfigManager.Instance().LoadConfig();

            Events.UI.UIEventManager.Instance().OnCountDownZero -= ResumeGame;
            Events.UI.UIEventManager.Instance().OnCountDownZero += ResumeGame;

            Events.GameEventManager.Instance().OnPlayerLifeLost -= OnPlayerLivesLost;
            Events.GameEventManager.Instance().OnPlayerLifeLost += OnPlayerLivesLost;

            InputManager.Instance();

            BBManagerMediator mediator = BBManagerMediator.Instance();
            mediator.ProgressManager.Init();
            BBScreenSize.Instance().Init(mediator.MainCamera.Camera);
        }

        //Called when and all counter UI is enabled and Counter become zero
        private void ResumeGame()
        {
            if (!IsGameStarted)
            {
                IsGameStarted = true;
                Events.GameEventManager.Instance().TriggerStartGame();
            }
            else
            {
                BBManagerMediator mediator = BBManagerMediator.Instance();
                mediator.PlayerController.OnCountDownZero();
                PauseManagers(false);
            }
        }

        internal void OnGamePause(bool inPauseStatue)
        {
            IsGamePause = inPauseStatue;
            Events.GameEventManager.Instance().TriggerPause(inPauseStatue);
        }


        public void OnLevelUp(int inCurrentLevel)
        {
            UI.BBUIManager.Instance().ShowLevelUpUI(inCurrentLevel, null);
            PauseManagers(true);
        }

        internal void OnLevelUpAnimationCompleted()
        {
            PauseManagers(false);
            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            Events.GameEventManager.Instance().TriggerLoadNextLevel();
        }

        public void OnUpdate()
        {
            InputManager.Instance().OnUpdate();
        }

        internal void OnExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        internal void OnPlayerLivesLost(int inPlayerLivesLeft)
        {
            if (inPlayerLivesLeft <= 0)
            {
                OnGameOver();
                inPlayerLivesLeft = 0;
            }
            else
            {
                PauseManagers(true);
                UI.BBUIManager.Instance().ShowCountDownUI();
            }
            Events.UI.UIEventManager.Instance().TriggerPlayerLivesUpdate(inPlayerLivesLeft);
        }

        private void PauseManagers(bool inPauseStatus)
        {
            InputManager.Instance().PauseUpdate(inPauseStatus);
            BBBallManager.Instance().PauseAll(inPauseStatus);
            BBBulletManager.Instance().PauseAll(inPauseStatus);
        }

        internal void OnGameOver()
        {
            IsGameOver = true;
            Events.GameEventManager.Instance().TriggerGameOver();
            UI.BBUIManager.Instance().ShowGameOverUI();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}