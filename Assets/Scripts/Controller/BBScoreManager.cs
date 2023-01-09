using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBScoreManager : SingletonObserverBase<BBScoreManager>
    {
        public int TotalScore { get; internal set; }

        private int m_ScoreMultiplier;

        public override void Init()
        {
            base.Init();
            m_ScoreMultiplier = config.BBConfigManager.Instance().GameSetting.ScoreMultiplier;
        }

        public void OnBulletHitBall()
        {
            int totalScore = TotalScore + m_ScoreMultiplier;
            UpdateScore(totalScore);
        }

        private void UpdateScore(int inTotal)
        {
            if (inTotal >= TotalScore)
                TotalScore = inTotal;

            Events.UI.UIEventManager.Instance().TriggerPlayerScoreUpdate(TotalScore);
        }
    }
}