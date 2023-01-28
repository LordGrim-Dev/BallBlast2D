using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBSplitBall : BBBall
    {
        public override uint BallID => BBConstants.k_SPLIT_BALL_ID;

        protected override void OnHitBullet(uint inDamage)
        {
            base.OnHitBullet(inDamage);

            if (CurrentBallSizeLevel == BallSize.eLevel_0 && m_BallHitCounter >= m_MaxBalHitCountToSplit)
            {
                Hide();
                BBBallManager.Instance().OnBallDeath(BallID);
            }
            else
            {
                if (m_BallHitCounter >= m_MaxBalHitCountToSplit)
                {
                    var currentPosition = transform.position;
                    var scale = transform.localScale;

                    BallSize decresedSize = CurrentBallSizeLevel - 1;

                    CurrentBallSizeLevel = decresedSize == BallSize.eLevel_0 ? BallSize.eLevel_0 : decresedSize;
                    Hide();
                    BBBallManager.Instance().CheckForSplitAndSpawn(m_MaxBalHitCountToSplit, currentPosition, CurrentBallSizeLevel);
                }
            }
        }

        public override void InitialiseNewBall(uint inBallMaxHitCount, Vector3 inPostion, BallSize inSize, float inXVelocity)
        {
            base.InitialiseNewBall(inBallMaxHitCount, inPostion, inSize, inXVelocity);

            m_BallForce.x = inXVelocity;
            m_BallForce.y = GetSplitBounce();

            SetRigidBodyType(RigidbodyType2D.Dynamic);
            ApplyForce(m_BallForce);

            m_IsBallBouncingTowardsRight = inXVelocity > 0 ? true : false;
            m_IsInitialisationComplete = true;
        }


        public float GetSplitBounce()
        {
            return 17.5f;
        }
    }
}
