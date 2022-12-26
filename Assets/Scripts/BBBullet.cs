﻿
using DG.Tweening;
using UnityEngine;

namespace BallBlast
{
    public class BBBullet : MonoBehaviour, IPoolMember
    {
        private bool m_IsInUse;

        public bool IsInUse => m_IsInUse;

        int IPoolMember.ID => transform.GetInstanceID();

        public void Show()
        {
            gameObject.SetActive(true);
            m_IsInUse = true;
            float time = 1.0f;
            float topBorder = (float)BBBulletManager.Instance().GetEndPointYPosition();
            transform.DOMoveY(topBorder, time, false).SetEase(Ease.Linear).OnComplete(Hide);
        }

        public void Hide()
        {
            m_IsInUse = false;
            transform.DOKill();
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void OnCreated()
        {
            m_IsInUse = false;
            gameObject.SetActive(false);
        }


        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     Hide();
        // }
    }
}
