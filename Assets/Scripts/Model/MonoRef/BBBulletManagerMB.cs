using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBBulletManagerMB : MonoBehaviour
    {
        [SerializeField]
        BBBullet m_BulletPrefab;

        public BBBullet BulletPrefab { get => m_BulletPrefab; }

        public void Init()
        {
            BBBulletManager.Instance().Init(this);
        }
    }
}