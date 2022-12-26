using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBBall : MonoBehaviour, IPoolMember, IBallProperties
    {
        [SerializeField]
        private Rigidbody2D m_BallRigidBody;

        private bool m_IsInUse;

        private Vector2 m_BallForce;

        private bool m_IsBallBouncingTowardsRight;

        [SerializeField]
        private uint m_BallHitCounter, m_MaxBalHitCountToSplit;
        public bool IsInUse => m_IsInUse;

        public Vector2 Position => transform.position;

        public int ID => transform.GetInstanceID();

        bool m_IsInitialisationComplete;


        // Called only once when Object is been created
        public void OnCreated()
        {
            m_IsInUse = false;
            m_IsBallBouncingTowardsRight = true;
            m_BallForce = Vector2.zero;
            m_BallHitCounter = 0;
            m_IsInitialisationComplete = false;
        }

        // Called before Showing the object
        public void InitialiseNewBall(uint inBallMaxHitCount, Vector2 inPostion, float inXVelocity, bool inInitialSlowWallSpawnAnim = false)
        {
            SetRigidBodyType(RigidbodyType2D.Static);
            transform.position = inPostion;

            m_BallForce.x = inXVelocity;
            m_BallForce.y = m_BallRigidBody.velocity.y;

            ResetBallProperties();
            m_MaxBalHitCountToSplit = inBallMaxHitCount;
            Show();

            m_IsBallBouncingTowardsRight = inXVelocity > 0 ? false : true;

            if (inInitialSlowWallSpawnAnim)
            {
                Vector2 spawnPostion = GetSpawnPosition(m_IsBallBouncingTowardsRight);

                AnimateSlowSpawn(spawnPostion,
                () =>
                {
                    SetRigidBodyType(RigidbodyType2D.Dynamic);
                    ApplyForce(m_BallForce);
                });
            }
            else
            {
                SetRigidBodyType(RigidbodyType2D.Dynamic);
                ApplyForce(m_BallForce);
            }

            m_IsInitialisationComplete = true;
        }

        private void SetRigidBodyType(RigidbodyType2D inBodyType)
        {
            m_BallRigidBody.bodyType = inBodyType;
        }

        public void Hide()
        {
            m_IsInUse = false;

            gameObject.SetActive(false);
        }

        public void Show()
        {
            m_IsInUse = true;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_IsInitialisationComplete) return;
            //Check if we hit on Bullet/Border

            bool isBullet = BBBulletManager.Instance().IsBullet(other.transform.GetInstanceID(), out uint bulletDamage);
            if (isBullet)
            {
                // Decrease Count for hit
                // Check for Split required
                OnHitBullet(bulletDamage);
            }
            else
            {
                // We need to check did we hit on any border or not
                // If Border get the border and apply force to oppsite direction
                BBBallManager.Instance().GetBorderDirection(other.transform, out Direction hitBorderDirection);

                if (hitBorderDirection == Direction.eNone) return;

                UpdateBallForceForTheDirection(hitBorderDirection, ref m_BallForce);

                ApplyForce(m_BallForce);
            }

        }

        private void ApplyForce(Vector2 inForce)
        {
            m_BallRigidBody.velocity = inForce;
        }

        public void OnHitBullet(uint inDamage)
        {
            m_BallHitCounter += inDamage;
            if (m_MaxBalHitCountToSplit <= 0)
            {
                Hide();
                BBBallManager.Instance().OnBallDeath();
            }
            else
            {
                if (m_BallHitCounter >= m_MaxBalHitCountToSplit)
                {
                    Hide();
                    BBBallManager.Instance().CheckForSplitAndSpawn(m_MaxBalHitCountToSplit);
                }
            }

        }


        private void UpdateBallForceForTheDirection(Direction borderDirection, ref Vector2 outForce)
        {
            if (borderDirection == Direction.eNone) return;

            outForce = m_BallRigidBody.velocity;

            if (borderDirection == Direction.eLeft || borderDirection == Direction.eRight)
            {
                // Left or right Border
                if (borderDirection == Direction.eRight)
                {
                    outForce.x = -BBConstants.k_MAX_XVELOCITY;
                    m_IsBallBouncingTowardsRight = false;
                }
                else
                {
                    outForce.x = BBConstants.k_MAX_XVELOCITY;
                    m_IsBallBouncingTowardsRight = true;
                }
            }
            else
            {
                outForce.y = GetBounceForce();
                if (m_IsBallBouncingTowardsRight)
                    outForce.x = BBConstants.k_MAX_XVELOCITY;
                else
                    outForce.x = -BBConstants.k_MAX_XVELOCITY; // To Left
            }
        }

        private float GetBounceForce()
        {
            return 40f;
        }

        private void AnimateSlowSpawn(Vector2 spawnPostion, Action OnComplete = null)
        {
            transform.DOMove(spawnPostion, 1f, false)
            .OnComplete(() => OnComplete?.Invoke())
            .SetEase(Ease.Linear);
        }

        private Vector2 GetSpawnPosition(bool isSpawnFromLeft)
        {
            Vector2 spawnPoint = Vector2.one;
            spawnPoint.x = transform.position.x - 1.5f;
            if (isSpawnFromLeft)
                spawnPoint.x = transform.position.x + 1.5f;

            spawnPoint.y = transform.position.y - 2.35f;
            return spawnPoint;
        }


        public void ResetBallProperties()
        {
            transform.DOKill();
            m_IsInUse = false;
            m_IsBallBouncingTowardsRight = false;
            m_BallHitCounter = 0;
        }
    }
}