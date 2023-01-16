
using DG.Tweening;
using UnityEngine;

namespace BallBlast
{
    public class BBBullet : MonoBehaviour, IPoolMember
    {
        public bool IsInUse { get; private set; }

        public int ID => transform.GetInstanceID();

        public uint BulletDamage { get; private set; }

        private Tween m_BulletMovingTween;

        public void Show()
        {
            gameObject.SetActive(true);
            IsInUse = true;

            float time = 0.70f;
            float topBorder = (float)BBBulletManager.Instance().GetEndPointYPosition();
            
            DOTween.Kill(ID);
            
            m_BulletMovingTween = transform.DOMoveY(topBorder, time, false)
                .SetEase(Ease.Linear)
                .SetId(ID).OnComplete(Hide);
        }

        public void Hide()
        {
            IsInUse = false;

            transform.position = Vector2.zero;

            m_BulletMovingTween.Pause();

            gameObject.SetActive(false);
        }

        public void OnCreated()
        {
            IsInUse = false;
            gameObject.SetActive(false);
            BulletDamage = GetBulletDamage();
        }

        public uint GetBulletDamage()
        {
            return BBBulletManager.Instance().GetBulletDamage();
        }

        public void OnPause(bool inPauseStatus)
        {
            if (inPauseStatus)
                m_BulletMovingTween.Pause();
            else
                m_BulletMovingTween.Play();
        }
    }
}
