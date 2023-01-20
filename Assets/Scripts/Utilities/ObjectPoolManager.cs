using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Game.Common
{
    public class ObjectPoolManager<T> where T : IPoolMember
    {
        int m_ItemCOunter;
        Dictionary<int, IPoolMember> m_ObjectPool;

        IPoolMember m_Prefab = null;
        Transform m_Parent;

        public ObjectPoolManager(IPoolMember inPrefab, Transform inParent)
        {
            m_ItemCOunter = 0;
            m_Prefab = inPrefab;
            m_Parent = inParent;

            m_ObjectPool = new Dictionary<int, IPoolMember>();
        }

        public IPoolMember GetNewBallFromPool()
        {
            IPoolMember poolMember = null;

            int totalPoolObject = m_ObjectPool.Count;

            foreach (var poolItem in m_ObjectPool)
            {
                if (!poolItem.Value.IsInUse)
                {
                    poolMember = poolItem.Value;
                }
            }

            if (poolMember == null)
            {
                IPoolMember newMember = GetNewPoolMember();
                m_ObjectPool.Add(newMember.ID, newMember);
                poolMember = newMember;
            }

            return poolMember;
        }


        private IPoolMember GetNewPoolMember()
        {
            IPoolMember newMember = null;

            GameObject prefab = ((MonoBehaviour)m_Prefab).gameObject;

            var newObj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, m_Parent);

            newObj.name += "_" + m_ItemCOunter;

            newMember = newObj.GetComponent<IPoolMember>();

            newMember.OnCreated();

            m_ItemCOunter++;

            return newMember;
        }

        public void OnDestroy()
        {
            m_ObjectPool.Clear();
            m_ObjectPool = null;
        }

        public bool IsItemPresentInPool(int inID, out IPoolMember outMemeber)
        {
            m_ObjectPool.TryGetValue(inID, out outMemeber);

            return (outMemeber != null);
        }

        internal void OnPause(bool inPauseStatus)
        {
            foreach (var mem in m_ObjectPool)
                mem.Value?.OnPause(inPauseStatus);
        }

        public IPoolMember GetPooledMemberByTransformID(int inID)
        {
            m_ObjectPool.TryGetValue(inID, out IPoolMember member);

            return member;
        }

        public int GetAllEnabledPoolMemberCount()
        {
            int numberOfPoolMembersNotHidden = 0;
            foreach (var poolMem in m_ObjectPool)
                if (!poolMem.Value.IsInUse) numberOfPoolMembersNotHidden++;

            return numberOfPoolMembersNotHidden;
        }

        public void HideAll()
        {
            foreach (var poolMem in m_ObjectPool)
            {
                poolMem.Value.Hide();
            }
        }
    }
}