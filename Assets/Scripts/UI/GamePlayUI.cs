using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BallBlast.UI
{
    [System.Serializable]
    public class GamePlayUI : BaseUI
    {
        [UnityEngine.SerializeField]
        TextMeshProUGUI m_ScoreText;

        [UnityEngine.SerializeField]
        TextMeshProUGUI m_ScoreHeader;

        [UnityEngine.SerializeField]
        TextMeshProUGUI m_PlayerLivesLeft;

        [SerializeField]
        TextMeshProUGUI m_LevelUpHeader;

        [SerializeField]
        TextMeshProUGUI m_LevelUpValue;


        private void OnEnable()
        {
            Events.UI.UIEventManager.Instance().OnPlayerScoreUpdate += OnPlayerScoreUpdate;

            Events.UI.UIEventManager.Instance().OnPlayerLivesUpdate += OnLivesLeftUpdate;

            Events.GameEventManager.Instance().OnGamePause += OnGamePause;

            Events.GameEventManager.Instance().OnGameOver += OnGameOver;
        }


        private void OnDisable()
        {
            Events.UI.UIEventManager.Instance().OnPlayerScoreUpdate -= OnPlayerScoreUpdate;

            Events.UI.UIEventManager.Instance().OnPlayerLivesUpdate -= OnLivesLeftUpdate;

            Events.GameEventManager.Instance().OnGamePause -= OnGamePause;

            Events.GameEventManager.Instance().OnGameOver -= OnGameOver;
        }

        protected override void Initialise()
        {
            var configinstance = config.BBConfigManager.Instance();
            m_ScoreHeader.text = configinstance.GetLocalisedStringForKey(config.ConfigJsonConstants.kScore);
            m_LevelUpHeader.text = configinstance.GetLocalisedStringForKey(config.ConfigJsonConstants.kLevel) + " - ";


            m_ScoreText.text = "0";

            string lives = configinstance.GameSetting.MaxLives.ToString();

            m_PlayerLivesLeft.text = lives;

            base.Initialise();
        }

        private void OnLivesLeftUpdate(int inLivesLeft)
        {
            string lives = inLivesLeft.ToString();
            m_PlayerLivesLeft.text = lives;

            float duration = 0.5f;
            AnimateScaleUp(m_PlayerLivesLeft.transform, duration);
        }


        private void OnPlayerScoreUpdate(int inScore)
        {
            m_ScoreText.text = inScore.ToString();
        }


        private void AnimateScaleUp(Transform inTransform, float inDur)
        {
            Vector3 originalScale = inTransform.localScale;
            Vector3 scaleUp = originalScale * 1.25f;
            inTransform.DOKill();
            inTransform.DOScale(scaleUp, inDur).SetEase(Ease.Linear).OnComplete(() =>
            {
                inTransform.localScale = originalScale;
            });
        }

        public void OnLevelUP(int inLevel)
        {
            m_LevelUpValue.text = inLevel.ToString();
        }

        private void OnGameOver()
        {

        }


        private void OnGamePause(bool inPause)
        {

        }




    }
}