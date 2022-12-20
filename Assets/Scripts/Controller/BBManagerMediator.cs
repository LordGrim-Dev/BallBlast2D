using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBManagerMediator : MonoBehaviour
    {
        private static BBManagerMediator s_Instance = null;
        public static BBManagerMediator Instance()
        {
            return s_Instance;
        }


        [SerializeField]
        BBPlayerControlMB m_PlayerController;

        [SerializeField]
        BBBulletManagerMB m_BulletManagerMB;

        [SerializeField]
        BBCommonRefHolderMB m_CommonRefHolderMB;

        public BBPlayerControlMB PlayerController { get => m_PlayerController; }
        public BBBulletManagerMB BulletManagerMB { get => m_BulletManagerMB; }
        public BBCommonRefHolderMB CommonRefHolderMB { get => m_CommonRefHolderMB; }

        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                Destroy(gameObject);
        }

    }
}
