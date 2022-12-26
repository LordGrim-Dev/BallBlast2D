using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBBallManagerMB : MonoBehaviour
    {
        [SerializeField]
        BBBall m_BallPrefab;

        public BBBall BallPrefab { get => m_BallPrefab; }

        public void Init()
        {
            BBBallManager.Instance().Init(this);
        }
    }
}
