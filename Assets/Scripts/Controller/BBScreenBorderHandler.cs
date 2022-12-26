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
