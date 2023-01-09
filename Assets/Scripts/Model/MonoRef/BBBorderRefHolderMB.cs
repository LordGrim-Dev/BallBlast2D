using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBBorderRefHolderMB : MonoBehaviour
    {
        [SerializeField]
        private BBBorder[] m_Borders;

        public BBBorder[] Borders { get => m_Borders; }

        public void Init()
        {
            BBBorderManager.Instance().Init(this);
        }
    }
}