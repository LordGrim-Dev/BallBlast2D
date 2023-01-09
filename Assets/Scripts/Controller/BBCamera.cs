using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBCamera : MonoBehaviour
    {
        [SerializeField]
        Camera m_MainCamera;
        
        public Camera Camera { get => m_MainCamera; }

        public void ShakeCamera(float inDuration, float inStrength = 0.15f)
        {
            float strength = inStrength;
            int vibrato = 50;
            float randdomNess = 60;
            m_MainCamera.DOKill();
            m_MainCamera.DOShakePosition(inDuration, strength, vibrato, randdomNess, true);
        }
    }
}