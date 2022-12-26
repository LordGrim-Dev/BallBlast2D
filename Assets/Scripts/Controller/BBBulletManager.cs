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

        public BBBullet GetFreeBulletFromPool(Vector3 inPos)
        {
            BBBullet bullet = null;

            bullet = m_BulletPoolManager.GetNewBallFromPool() as BBBullet;

            bullet.transform.position = inPos;

            return bullet;
        }

        internal bool IsBullet(int inItemID, out uint outDamage)
        {
            outDamage = 0;
            bool isBullet = m_BulletPoolManager.IsItemPresentInPool(inItemID);
            if (isBullet)
            {
                outDamage = 1;// Need to CHange based one Bullet update
            }

            return isBullet;
        }
    }
}
