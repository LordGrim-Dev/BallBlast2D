using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public abstract class BBBall : MonoBehaviour, IPoolMember, IBallProperties
    {
        [SerializeField]
        protected Rigidbody2D m_BallRigidBody;

        [SerializeField]
        TextMesh m_CountText;

        protected bool m_IsInUse;

        protected Vector2 m_BallForce;

        protected bool m_IsBallBouncingTowardsRight;

        [SerializeField]
        protected uint m_MaxBalHitCountToSplit, m_BallHitCounter;
        public bool IsInUse => m_IsInUse;

        public int ID => transform.GetInstanceID();

        public abstract uint BallID { get; }

        public BallSize CurrentBallSizeLevel { get; protected set; }

        protected bool m_IsInitialisationComplete;

        private float m_MaxYBounceForce;

        Tween m_RotationTween;


        // Called only once when Object is been created
        public virtual void OnCreated()
        {
            m_IsInUse = false;
            m_IsBallBouncingTowardsRight = true;
            m_BallForce = Vector2.zero;
            m_BallHitCounter = 0;
            m_IsInitialisationComplete = false;
            UpdateCountText(0);

            Vector3 rotation = Vector3.one;
            rotation.z = 360f;
            float rotationDur = 3f;

            m_RotationTween = transform.DORotate(rotation, rotationDur, RotateMode.LocalAxisAdd)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
            m_RotationTween.Pause();
        }

        // Called before Showing the object
        public virtual void InitialiseNewBall(uint inBallMaxHitCount, Vector3 inPostion, BallSize inBallSize, float inXVelocity)
        {
            m_MaxBalHitCountToSplit = inBallMaxHitCount;
            UpdateCountText(m_MaxBalHitCountToSplit);
            ResetBallProperties();
            transform.position = inPostion;
            CurrentBallSizeLevel = inBallSize;
            UpdateScale();
            Show();
            m_MaxYBounceForce = GetBounceForce();
        }

        private void UpdateScale()
        {
            Vector2 scale = Vector2.one;
            switch (CurrentBallSizeLevel)
            {
                case BallSize.eLevel_0:
                    scale = Vector2.one;
                    break;

                case BallSize.eLevel_1:
                    scale = Vector2.one * 1.5f;
                    break;

                case BallSize.eLevel_2:
                    scale = Vector2.one * 1.75f;
                    break;

                case BallSize.eLevel_3:
                    scale = Vector2.one * 2f;
                    break;
            }
            transform.localScale = scale;
        }

        protected void SetRigidBodyType(RigidbodyType2D inBodyType)
        {
            m_BallRigidBody.bodyType = inBodyType;
        }

        public void Hide()
        {
            m_IsInUse = false;

            var resetPos = transform.position;
            resetPos.x = resetPos.y = 0;
            transform.position = resetPos;

            m_RotationTween.Pause();

            gameObject.SetActive(false);

        }

        public void Show()
        {
            m_IsInUse = true;
            gameObject.SetActive(true);
            m_RotationTween.Play();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_IsInitialisationComplete) return;

            bool isBullet = BBBulletManager.Instance().IsBullet(other.transform.GetInstanceID(), out uint bulletDamage);
            if (isBullet)
            {
                // Decrease Count for hit
                // Check for Split required
                OnHitBullet(bulletDamage);
            }

            // We need to check did we hit on any border or not
            // If Border get the border and apply force to oppsite direction
            BBBallManager.Instance().CheckHitBorderAndGetDir(other.transform, CurrentBallSizeLevel, out Direction hitBorderDirection);

            if (hitBorderDirection == Direction.eNone) return;

            UpdateBallForceForTheDirection(hitBorderDirection, ref m_BallForce);

            ApplyForce(m_BallForce);
        }

        protected void ApplyForce(Vector2 inForce)
        {
            m_BallRigidBody.velocity = inForce;
        }

        protected virtual void OnHitBullet(uint inDamage)
        {
            m_BallHitCounter += inDamage;

            UpdateCountText(m_MaxBalHitCountToSplit - m_BallHitCounter);

            BBScoreManager.Instance().OnBulletHitBall();
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
                //to make sure not going out of the screeen
                outForce.y = -2;

                if (borderDirection == Direction.eBottom)
                {
                    outForce.y = m_MaxYBounceForce;
                    if (m_IsBallBouncingTowardsRight)
                        outForce.x = BBConstants.k_MAX_XVELOCITY;
                    else
                        outForce.x = -BBConstants.k_MAX_XVELOCITY; // To Left
                }
            }
        }

        private float GetBounceForce()
        {
            float bounceForce = 30;
            switch (CurrentBallSizeLevel)
            {
                case BallSize.eLevel_0:
                    bounceForce = 32f;
                    break;

                case BallSize.eLevel_1:
                    bounceForce = 35f;
                    break;

                case BallSize.eLevel_2:
                    bounceForce = 37f;
                    break;

                case BallSize.eLevel_3:
                    bounceForce = 45f;
                    break;
            }

            return bounceForce;
        }



        protected Vector3 GetSpawnPosition(Vector3 inPos)
        {
            Vector3 spawnPoint = transform.position;
            spawnPoint.x = transform.position.x - 1.5f;
            if (inPos.x < 0)
                spawnPoint.x = transform.position.x + 1.5f;

            spawnPoint.y = transform.position.y - 2.35f;
            return spawnPoint;
        }


        public void ResetBallProperties()
        {
            SetRigidBodyType(RigidbodyType2D.Static);
            DOTween.Kill(ID);
            m_IsInUse = false;
            m_IsBallBouncingTowardsRight = false;
            m_BallHitCounter = 0;
        }


        private void UpdateCountText(uint inCount)
        {
            if (inCount >= 0)
                m_CountText.text = inCount.ToString();
        }

        public void OnPause(bool inPauseStatus)
        {
            if (inPauseStatus)
            {
                SetRigidBodyType(RigidbodyType2D.Static);
                m_RotationTween.Pause();
            }
            else
            {
                m_RotationTween.Play();
                SetRigidBodyType(RigidbodyType2D.Dynamic);
            }
        }
    }
}