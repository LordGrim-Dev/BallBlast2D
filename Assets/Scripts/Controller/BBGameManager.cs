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

        public override void Init()
        {
            IsGameOver = IsGameStarted = IsGamePause = false;

            base.Init();
            config.BBConfigManager.Instance().LoadConfig();

            Events.UI.UIEventManager.Instance().OnCountDownZero -= ResumeGame;
            Events.UI.UIEventManager.Instance().OnCountDownZero += ResumeGame;

            Events.GameEventManager.Instance().OnPlayerLifeLost -= OnPlayerLivesLost;
            Events.GameEventManager.Instance().OnPlayerLifeLost += OnPlayerLivesLost;

            Events.GameEventManager.Instance().OnLevelUp -= OnCurrentLevelCompleted;
            Events.GameEventManager.Instance().OnLevelUp += OnCurrentLevelCompleted;

            InputManager.Instance();

            BBManagerMediator mediator = BBManagerMediator.Instance();
            mediator.ProgressManager.Init();
            BBScreenSize.Instance().Init(mediator.MainCamera.Camera);
        }

        private void OnCurrentLevelCompleted(int inCurrentLevel)
        {
            
        }

        public void InitialiseAllManagers()
        {
            BBManagerMediator mediator = BBManagerMediator.Instance();
            mediator.PlayerController.Init();
            mediator.BulletManagerMB.Init();
            mediator.BallManagerMB.Init();
            mediator.BorderRefHolderMB.Init();

            UI.BBUIManager.Instance().ShowIntroUI();
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
                InputManager.Instance().PauseUpdate(false);
                BBBallManager.Instance().PauseAll(false);
                BBBulletManager.Instance().PauseAll(false);
            }
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

        internal void OnGamePause(bool inPauseStatue)
        {
            IsGamePause = inPauseStatue;
            Events.GameEventManager.Instance().TriggerPause(inPauseStatue);
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
                InputManager.Instance().PauseUpdate(true);
                BBBallManager.Instance().PauseAll(true);
                BBBulletManager.Instance().PauseAll(true);
                UI.BBUIManager.Instance().ShowCountDownUI();
            }
            Events.UI.UIEventManager.Instance().TriggerPlayerLivesUpdate(inPlayerLivesLeft);
        }

        internal void OnGameOver()
        {
            IsGameOver = true;
            Events.GameEventManager.Instance().TriggerGameOver();
            UI.BBUIManager.Instance().ShowGameOverUI();
        }

        internal config.LevelDetails GetCurrentLevelData()
        {
            config.LevelDetails levelData;

            int cureentLevel = BBManagerMediator.Instance().ProgressManager.PlayerCurrentLevel;
            levelData = config.BBConfigManager.Instance().GetCurrentLevelData(cureentLevel);

            return levelData;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}