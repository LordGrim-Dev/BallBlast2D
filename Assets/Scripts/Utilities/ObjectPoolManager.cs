using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Game.Common
{
    public class ObjectPoolManager<T> where T : IPoolMember
    {
        HashSet<IPoolMember> m_ObjectPool;

        IPoolMember m_Prefab = null;
        Transform m_Parent;

        public ObjectPoolManager(IPoolMember inPrefab, Transform inParent)
        {
            m_Prefab = inPrefab;
            m_Parent = inParent;

            m_ObjectPool = new HashSet<IPoolMember>();
        }

        public IPoolMember GetObjectFromPool()
        {
            IPoolMember poolMember = null;

            int totalPoolObject = m_ObjectPool.Count;

            foreach (var poolItem in m_ObjectPool)
            {
                if (!poolItem.IsInUse)
                {
                    poolMember = poolItem;
                }
            }

            if (poolMember == null)
            {
                IPoolMember newMember = GetNewPoolMember();
                m_ObjectPool.Add(newMember);
                poolMember = newMember;
            }

            return poolMember;
        }


        private IPoolMember GetNewPoolMember()
        {
            IPoolMember newMember = null;

            GameObject prefab = ((MonoBehaviour)m_Prefab).gameObject;

            var newObj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, m_Parent);

            newMember = newObj.GetComponent<IPoolMember>();
            
            newMember.Init();

            return newMember;
        }

        public void OnDestroy()
        {
            m_ObjectPool.Clear();
            m_ObjectPool = null;
        }

    }
}