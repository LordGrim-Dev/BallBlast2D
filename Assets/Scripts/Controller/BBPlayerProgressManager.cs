using System;
using System.Collections;
using System.Collections.Generic;
using BallBlast.config;
using Game.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace BallBlast
{
    public class BBPlayerProgressManager
    {
        private int m_PlayerCurrentLevel;

        public int PlayerCurrentLevel { get => m_PlayerCurrentLevel; }

        public void Init()
        {
            var eventManage = Events.GameEventManager.Instance();

            eventManage.OnLevelCompleted -= OnLevelCompleted;
            eventManage.OnLevelCompleted += OnLevelCompleted;

            eventManage.OnGameOver -= SavePlayerProgress;
            eventManage.OnGameOver += SavePlayerProgress;
            
            LoadSavedData();
        }


        private void OnLevelCompleted()
        {
            m_PlayerCurrentLevel += 1;
            Events.GameEventManager.Instance().TriggerLevelUp(m_PlayerCurrentLevel);
        }

        private void LoadSavedData()
        {
            var playerProg = config.BBConfigManager.Instance().GetPlayerProgress();
#if DEBUG
            if (playerProg == null) Game.Common.GameUtilities.ShowLog("NULL PLAYER PROGRESS!!!");
#endif
            if (playerProg != null)
                m_PlayerCurrentLevel = playerProg.PlayerSavedLevel;

            if (m_PlayerCurrentLevel == 0)
                m_PlayerCurrentLevel = 1;
        }

        public void SavePlayerProgress()
        {
#if DEBUG
            GameUtilities.ShowLog("SavePlayerProgress");
#endif

            PlayerProgressData progress = new PlayerProgressData(m_PlayerCurrentLevel, BBScoreManager.Instance().TotalScore);
            config.BBConfigManager.Instance().SavePlayerProgress(progress);
        }
    }
}