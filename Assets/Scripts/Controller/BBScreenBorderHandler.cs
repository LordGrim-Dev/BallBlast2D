using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBScreenSize : SingletonObserverBase<BBScreenSize>
    {
        private float m_Top, m_Bottom, m_Left, m_Right;

        public float Top { get => m_Top; }
        public float Bottom { get => m_Bottom; }
        public float Left { get => m_Left; }
        public float Right { get => m_Right; }

        public void Init(Camera inMainCamera)
        {
            CacheBorderValues(inMainCamera);
            UpdateScreenBGSize(inMainCamera);
        }

        private void UpdateScreenBGSize(Camera inMainCamera)
        {
            var screenBgTransform = BBManagerMediator.Instance().CommonRefHolderMB.BGSprite;

            Vector2 scale = Vector2.one;

            var bound = screenBgTransform.GetComponent<SpriteRenderer>().bounds.size;

            // For Camera shake 
            float extraOffset = 1.5f;
            float extraYOffset = 0.25f;

            scale.x = (m_Right * 2 / bound.x) + extraOffset;

            scale.y = (m_Top * 2 / bound.y) + extraYOffset;

            screenBgTransform.localScale = scale;
        }

        private void CacheBorderValues(Camera inMainCamera)
        {
            float height = inMainCamera.orthographicSize * 2;
            float width = height * inMainCamera.aspect;

            m_Top = height / 2;
            m_Bottom = -(m_Bottom);
            m_Right = width / 2;
            m_Left = -(m_Right);

            GameUtilities.ShowLog($"TOP = {m_Top} : BOTTOM = {m_Bottom} : RIGHT ={m_Right} : LEFT = {m_Left} ");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
