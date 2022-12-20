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
        Transform m_BulletPivotTransform;

        private float m_MaxPlayerBound;

        private float m_FireCooldownTime;

        [SerializeField]
        private bool m_FireAllowed;



        public void Init()
        {
            m_FireCooldownTime = 0;
            m_FireAllowed = false;
            CoroutineManager.Instance.StartCoroutine(CheckAndFireBullet());
            InputManager.Instance().SubscribeToMouseEvent(OnMouseDownEvent, OnMouseUpEvet);
            InputManager.Instance().SubscribeToGetMousePosition(OnMousePositionUpdate);

            BoxCollider2D playerCollider = transform.GetComponent<BoxCollider2D>();
            m_MaxPlayerBound = playerCollider.bounds.size.x;
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
            transform.DOMoveX(inDistance, moveSpeed, false).SetEase(Ease.Linear);
        }

        private void OnMouseUpEvet()
        {
            EnablePlayerControl(false);
            ResetFireCoolDown();
        }

        private void ResetFireCoolDown()
        {
            m_FireCooldownTime = BBConstants.KMAX_FIRE_COOLDOWN;
        }

        private void OnMouseDownEvent()
        {
            EnablePlayerControl(true);
        }

        public void EnablePlayerControl(bool inENable)
        {
            m_FireAllowed = inENable;
        }

        private IEnumerator CheckAndFireBullet()
        {
            while (true)
            {
                if (m_FireAllowed && m_FireCooldownTime <= 0)
                {
                    m_FireCooldownTime = BBConstants.KMAX_FIRE_COOLDOWN; ;
                    FireBullet();
                }
                else
                {
                    if (m_FireCooldownTime > 0)
                        m_FireCooldownTime -= 0.25f;
                }

                yield return null;
            }
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

            float BorderX = Mathf.Abs((float)BBScreenBorderHandler.Instance().LeftBorder);

            float roundedPosition = BorderX - halfWidth;

            if ((nextPos + halfWidth) <= BorderX)
                roundedPosition = inMousePos.x;
            else if (inMousePos.x < 0)
                roundedPosition = -BorderX + halfWidth;

            return roundedPosition;
        }

        private void OnDestroy()
        {
            InputManager.Instance().UnSubscribeToMouseEvent(OnMouseDownEvent, OnMouseUpEvet);
            InputManager.Instance().UnSubscribeToGetMousePosition(OnMousePositionUpdate);
        }
    }
}