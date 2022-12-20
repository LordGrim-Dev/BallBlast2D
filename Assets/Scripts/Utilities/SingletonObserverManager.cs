
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class SingletonObserverManager : MonoBehaviour
    {
        private List<ISingltonMember> m_SingleTonClassList;

        private static SingletonObserverManager s_Instance = null;
        public static SingletonObserverManager Instance
        {
            get => s_Instance;
        }

        void Awake()
        {
            m_SingleTonClassList = new List<ISingltonMember>();

            if (s_Instance == null)
                s_Instance = this;
            else
                Destroy(gameObject);
        }



        public void Register(ISingltonMember inMember)
        {
            GameUtilities.ShowLog($"REGISTER+ :{inMember.GetType().Name}");
            if (!m_SingleTonClassList.Contains(inMember))
            {
                m_SingleTonClassList.Add(inMember);
            }
        }


        public void UnRegister(ISingltonMember inMember)
        {
            if (m_SingleTonClassList.Contains(inMember))
            {
                m_SingleTonClassList.Remove(inMember);
            }
        }


        void OnDestroy()
        {
            DestoryNonMonoSingleTon();
        }


        private void DestoryNonMonoSingleTon()
        {
            foreach (ISingltonMember member in m_SingleTonClassList)
            {
                GameUtilities.ShowLog($"OnDestroy+ :{member.GetType().Name}");
                member?.OnDestroy();
            }
        }
    }

}