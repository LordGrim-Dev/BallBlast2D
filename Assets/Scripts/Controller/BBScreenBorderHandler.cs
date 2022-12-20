using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBScreenBorderHandler : SingletonObserverBase<BBScreenBorderHandler>
    {
        private double m_Top, m_Bottom, m_LeftBorder, m_RightBorder;

        public double Top { get => m_Top; }
        public double Bottom { get => m_Bottom; }
        public double LeftBorder { get => m_LeftBorder; }
        public double RightBorder { get => m_RightBorder; }

        public void Init(Camera inMainCamera)
        {
            CacheBorderValues(inMainCamera);
        }

        private void CacheBorderValues(Camera inMainCamera)
        {
            float height = inMainCamera.orthographicSize * 2;
            float width = height * inMainCamera.aspect;

            m_Top = height/2;
            m_Bottom = -(m_Bottom);
            m_RightBorder = width/2;
            m_LeftBorder = -(m_RightBorder);

            GameUtilities.ShowLog($"TOP = {m_Top} : BOTTOM = {m_Bottom} : RIGHT ={m_RightBorder} : LEFT = {m_LeftBorder} ");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
