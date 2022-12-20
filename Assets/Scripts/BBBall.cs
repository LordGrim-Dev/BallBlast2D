using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBBall : MonoBehaviour, IPoolMember
    {
        private bool m_IsInUse;
        public bool IsInUse => m_IsInUse;

        public void Hide()
        {
            m_IsInUse = false;

            gameObject.SetActive(false);
        }

        public void Init()
        {
            m_IsInUse = false;
        }

        public void Show()
        {
            m_IsInUse = true;
            gameObject.SetActive(true);
            Vector2 randomePosition = BBBallManager.Instance().GetBallSpawnPoint();
            transform.position = randomePosition;
        }
    }
}