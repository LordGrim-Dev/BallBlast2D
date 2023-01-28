using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBPlayerControlMB : MonoBehaviour
    {

        [SerializeField]
        private Transform m_BulletPivotTransform;

        private float m_MaxPlayerBound;

        private float m_FireCooldownTime;

        [SerializeField]
        SpriteRenderer m_PlayerBase, m_GunPoint;

        private bool m_FireAllowed;

        private int m_PlayerTotalLives;

        Tween m_PlayerBaseBlink, m_GunBaseBlink;

        private int m_PlayerTweenID;
        public void Init()
        {
            m_PlayerTweenID = (int)DateTime.Now.Ticks;

            m_FireCooldownTime = BBConstants.k_MAX_FIRE_COOLDOWN;
            m_FireAllowed = false;

            BoxCollider2D playerCollider = transform.GetComponent<BoxCollider2D>();
            m_MaxPlayerBound = playerCollider.bounds.size.x;

            InputManager.Instance().SubscribeToMouseEvent(OnMouseDownEvent, OnMouseUpEvet, OnMouseClickAndHold);
            InputManager.Instance().SubscribeToGetMousePosition(OnMousePositionUpdate);

            Events.GameEventManager.Instance().OnGamePause -= EnablePlayerControl;
            Events.GameEventManager.Instance().OnGamePause += EnablePlayerControl;

            Events.GameEventManager.Instance().OnGameStarted -= ResumePlayerControl;
            Events.GameEventManager.Instance().OnGameStarted += ResumePlayerControl;

            Events.GameEventManager.Instance().OnGameOver -= OnGameOver;
            Events.GameEventManager.Instance().OnGameOver += OnGameOver;

            Events.GameEventManager.Instance().OnLevelCompleted -= OnLevelCompleted;
            Events.GameEventManager.Instance().OnLevelCompleted += OnLevelCompleted;

            Events.GameEventManager.Instance().OnLoadNextLevel -= OnLoadNextLevel;
            Events.GameEventManager.Instance().OnLoadNextLevel += OnLoadNextLevel;

            m_PlayerTotalLives = config.BBConfigManager.Instance().GameSetting.MaxLives;
            Events.UI.UIEventManager.Instance().TriggerPlayerLivesUpdate(m_PlayerTotalLives);

            CacheTweeningAnimation();
        }


        private void CacheTweeningAnimation()
        {
            Color endColor = m_PlayerBase.color;
            endColor.a = 0.5f;
            float duration = 0.55f;

            m_PlayerBaseBlink = m_PlayerBase.DOColor(endColor, duration)
                .OnComplete(() =>
                {
                    endColor.a = 1;
                    m_PlayerBase.color = endColor;
                }).SetLoops(-1).SetEase(Ease.Linear);


            endColor = Color.white;
            endColor.a = 0.5f;

            m_GunBaseBlink = m_GunPoint.DOColor(endColor, duration)
                .OnComplete(() =>
                {
                    endColor.a = 1;
                    m_GunPoint.color = endColor;
                }).SetLoops(-1).SetEase(Ease.Linear);

            m_PlayerBaseBlink.Pause();
            m_GunBaseBlink.Pause();
        }

        private void OnGameOver()
        {
            EnablePlayerControl(false);
        }

        private void OnMouseClickAndHold()
        {
            bool isGamePause = BBGameManager.Instance().IsGamePause;
            bool isGameOver = BBGameManager.Instance().IsGameOver;

            if (isGamePause || !m_FireAllowed || isGameOver) return;

            if (m_FireCooldownTime <= 0)
            {
                m_FireCooldownTime = BBConstants.k_MAX_FIRE_COOLDOWN; ;
                FireBullet();
            }
            else
            {
                m_FireCooldownTime -= Time.deltaTime;
            }
        }

        private void ResumePlayerControl()
        {
            m_FireAllowed = true;
        }

        private void OnMousePositionUpdate(Vector2 inMousePos)
        {
            float currentPlayerX = transform.position.x;

            float cappedPostion = GetClampedPosition(inMousePos, currentPlayerX);
            UpdatePlayerX(cappedPostion);
        }

        private void UpdatePlayerX(float inDistance)
        {
            float moveSpeed = 0.10f;
            DOTween.Kill(m_PlayerTweenID);
            transform.DOMoveX(inDistance, moveSpeed, false).SetEase(Ease.Linear).SetId(m_PlayerTweenID);
        }

        private void OnMouseUpEvet()
        {
            EnablePlayerControl(false);
            m_FireCooldownTime = BBConstants.k_MAX_FIRE_COOLDOWN;
        }

        private void OnMouseDownEvent()
        {
            EnablePlayerControl(true);
        }

        public void EnablePlayerControl(bool inENable)
        {
            m_FireAllowed = inENable;
        }

        private void FireBullet()
        {
            BBBullet bullet = BBBulletManager.Instance().GetNewBullet(m_BulletPivotTransform.position);
            bullet.Show();
        }

        private float GetClampedPosition(Vector2 inMousePos, float inCurrentPlayerX)
        {
            float halfWidth = m_MaxPlayerBound / 2;

            float nextPos = Mathf.Abs(inMousePos.x);

            float BorderX = Mathf.Abs((float)BBScreenSize.Instance().Left);

            float roundedPosition = BorderX - halfWidth;

            if ((nextPos + halfWidth) <= BorderX)
                roundedPosition = inMousePos.x;
            else if (inMousePos.x < 0)
                roundedPosition = -BorderX + halfWidth;

            return roundedPosition;
        }

        public void OnLivesLost()
        {
            m_PlayerTotalLives--;

            EnablePlayerControl(false);

            transform.GetComponent<BoxCollider2D>().enabled = false;

            Events.GameEventManager.Instance().TriggerPlayerLifeLost(m_PlayerTotalLives);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool isBallHit = BBBallManager.Instance().IsHitBallID(collision.transform.GetInstanceID());
            if (isBallHit)
            {
                OnLivesLost();
            }
        }

        private void OnLevelCompleted()
        {
            EnablePlayerControl(false);
        }

        private void OnLoadNextLevel()
        {
            EnablePlayerControl(true);
        }

        public void OnCountDownZero()
        {
            EnablePlayerControl(true);

            DOVirtual.DelayedCall(BBConstants.k_RESPAWN_COOLDOWN_TIME, OnRespawn)
                .OnStart(() => BlinkAnimation(true));
        }

        private void BlinkAnimation(bool inEnable)
        {
            if (inEnable)
            {
                m_PlayerBaseBlink.Play();
                m_GunBaseBlink.Play();
            }
            else
            {
                var originalColor = m_PlayerBase.color;
                originalColor.a = 1;

                m_PlayerBase.color = originalColor;

                originalColor = Color.white;
                originalColor.a = 1;

                m_GunPoint.color = originalColor;

                m_PlayerBaseBlink.Pause();
                m_GunBaseBlink.Pause();

            }
        }

        private void OnRespawn()
        {
            BlinkAnimation(false);
            transform.GetComponent<BoxCollider2D>().enabled = true;
        }

        private void OnDestroy()
        {
            InputManager.Instance().UnSubscribeToMouseEvent(OnMouseDownEvent, OnMouseUpEvet, OnMouseClickAndHold);
            InputManager.Instance().UnSubscribeToGetMousePosition(OnMousePositionUpdate);
        }
    }
}