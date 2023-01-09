using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBParentBall : BBBall
    {
        public override uint BallID => BBConstants.k_PARENT_BALL_ID;

        protected override void OnHitBullet(uint inDamage)
        {
            base.OnHitBullet(inDamage);
            if (m_BallHitCounter >= m_MaxBalHitCountToSplit || CurrentBallSizeLevel == BallSize.eLevel_0)
            {
                var currentPosition = transform.position;
                var currentScale = transform.localScale;
                Hide();
                BBBallManager.Instance().OnBallDeath(BallID);
                BBBallManager.Instance().CheckForSplitAndSpawn(m_MaxBalHitCountToSplit, currentPosition, CurrentBallSizeLevel);
            }
        }

        // called by Parent PoolManager
        public override void InitialiseNewBall(uint inBallMaxHitCount, Vector3 inPostion, BallSize inBallSize, float inXVelocity)
        {
            base.InitialiseNewBall(inBallMaxHitCount, inPostion, inBallSize, inXVelocity);

            m_MaxBalHitCountToSplit = inBallMaxHitCount;

            Vector3 doMovePos = GetSpawnPosition(inPostion);

            AnimateSlowSpawn(doMovePos,
            () => SetRigidBodyType(RigidbodyType2D.Dynamic));

            m_IsBallBouncingTowardsRight = inPostion.x > 0 ? false : true;
            m_IsInitialisationComplete = true;
        }

        private void AnimateSlowSpawn(Vector3 spawnPostion, Action OnComplete = null)
        {
            transform.DOMove(spawnPostion, 1f, false)
            .OnComplete(() => OnComplete?.Invoke()).SetId(ID)
            .SetEase(Ease.Linear);
        }
    }
}