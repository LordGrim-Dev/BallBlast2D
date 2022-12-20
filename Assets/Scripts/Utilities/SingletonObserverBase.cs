using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Common
{
    public abstract class SingletonObserverBase<T> : ISingltonMember where T : class, new()
    {
        private static T s_instance = null;
        public static T Instance()
        {
            if (s_instance == null)
                s_instance = new T();
            return s_instance;
        }

        protected SingletonObserverBase()
        {
            Init();
        }

        public virtual void Init()
        {
            SingletonObserverManager.Instance.Register(this);
        }

        public virtual void OnDestroy()
        {
            s_instance = null;
        }
    }

}