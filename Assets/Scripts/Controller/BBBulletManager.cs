using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBBulletManager : SingletonObserverBase<BBBulletManager>
    {
        ObjectPoolManager<BBBullet> m_BulletPoolManager;

        public void Init(BBBulletManagerMB inInstance)
        {
            m_BulletPoolManager = new ObjectPoolManager<BBBullet>(inInstance.BulletPrefab, inInstance.transform);

            Events.GameEventManager.Instance().OnGamePause -= PauseAll;
            Events.GameEventManager.Instance().OnGamePause += PauseAll;

            Events.GameEventManager.Instance().OnGameOver -= OnGameOver;
            Events.GameEventManager.Instance().OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            PauseAll(true);
        }

        public BBBullet GetNewBullet(Vector3 inPosition)
        {
            BBBullet newBullet = GetFreeBulletFromPool(inPosition);
            return newBullet;
        }


        public double GetEndPointYPosition()
        {
            float extraOffset = 5;
            double endPointY = BBScreenSize.Instance().Top + extraOffset;
            return endPointY;
        }


        public float GetBulletTravelSpeed()
        {
            float bulletSpeed = 3f;

            return bulletSpeed;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            m_BulletPoolManager.OnDestroy();
            m_BulletPoolManager = null;
        }

        internal uint GetBulletDamage()
        {
            // for now sending 1 as bullet damage
            return 1;
        }

        public BBBullet GetFreeBulletFromPool(Vector3 inPos)
        {
            BBBullet bullet = null;

            bullet = m_BulletPoolManager.GetNewBallFromPool() as BBBullet;

            bullet.transform.position = inPos;

            return bullet;
        }

        internal bool IsBullet(int inItemID, out uint outDamage)
        {
            outDamage = 1;
            bool isBullet = m_BulletPoolManager.IsItemPresentInPool(inItemID, out IPoolMember bullet);
            if (isBullet)
            {
                BBBullet castedBullet = bullet as BBBullet;
                outDamage = castedBullet.BulletDamage;
                HideThisBullet(inItemID, bullet);
            }
            return isBullet;
        }

        private void HideThisBullet(int inItemID, IPoolMember bullet)
        {
            if (bullet != null)
                bullet.Hide();
        }

        public void PauseAll(bool inPauseStatus)
        {
            m_BulletPoolManager.OnPause(inPauseStatus);
        }
    }
}
