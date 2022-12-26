using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class UnityUpdateCallbackManager : MonoBehaviour
    {
        HashSet<IUnityUpdateCallbacks> m_CallBackRegistry;
        
        private void Awake()
        {
            m_CallBackRegistry = new HashSet<IUnityUpdateCallbacks>();
        }

        public void SubScribeToUpdateCallBacks(IUnityUpdateCallbacks inCallBackInstance)
        {
            if (!m_CallBackRegistry.Contains(inCallBackInstance))
                m_CallBackRegistry.Add(inCallBackInstance);
        }

        public void UnSubScribeToUpdateCallBacks(IUnityUpdateCallbacks inCallBackInstance)
        {
            if (m_CallBackRegistry.Contains(inCallBackInstance))
                m_CallBackRegistry.Remove(inCallBackInstance);
        }
        

        void Update()
        {
            foreach (var instance in m_CallBackRegistry)
            {
                if (instance.IsObjectActiveInScene && instance.IsObjectEnabled)
                    instance.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (var instance in m_CallBackRegistry)
            {
                if (instance.IsObjectActiveInScene && instance.IsObjectEnabled)
                    instance.OnFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            foreach (var instance in m_CallBackRegistry)
            {
                if (instance.IsObjectActiveInScene && instance.IsObjectEnabled)
                    instance.OnLateUpdate();
            }
        }

        private void OnDestroy()
        {
            m_CallBackRegistry.Clear();
            m_CallBackRegistry = null;
        }
    }
}