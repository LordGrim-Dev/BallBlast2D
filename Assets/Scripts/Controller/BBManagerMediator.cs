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

        [SerializeField]
        BBCamera m_MainCamera;

        [SerializeField]
        BBBallManagerMB m_BallManagerMB;

        [SerializeField]
        BBBorderRefHolderMB m_BorderRefHolderMB;

        [SerializeField]
        BBUIRefHolderMB m_UIReferanceHolder;

        BBPlayerProgressManager m_ProgressManager;

        public BBPlayerControlMB PlayerController { get => m_PlayerController; }
        public BBBulletManagerMB BulletManagerMB { get => m_BulletManagerMB; }
        public BBCommonRefHolderMB CommonRefHolderMB { get => m_CommonRefHolderMB; }
        public BBBallManagerMB BallManagerMB { get => m_BallManagerMB; }
        public BBBorderRefHolderMB BorderRefHolderMB { get => m_BorderRefHolderMB; }
        internal BBUIRefHolderMB UIReferanceHolder { get => m_UIReferanceHolder; }
        public BBPlayerProgressManager ProgressManager { get => m_ProgressManager; }
        public BBCamera MainCamera { get => m_MainCamera; }

        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                Destroy(gameObject);

            m_ProgressManager = new BBPlayerProgressManager();
        }

    }
}
