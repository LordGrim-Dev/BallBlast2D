using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBBallManagerMB : MonoBehaviour
    {
        [SerializeField]
        BBSplitBall m_SplitBall;

        [SerializeField]
        BBParentBall m_ParentBall;

        public BBBall SplitBall { get => m_SplitBall; }

        public BBBall ParentBall { get => m_ParentBall; }

        public void Init()
        {
            BBBallManager.Instance().Init(this);
        }
    }
}
