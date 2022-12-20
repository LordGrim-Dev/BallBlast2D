using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager _instance = null;

        private bool m_IsDestroyed;
        void Awake()
        {
            if (_instance == null)

                _instance = this;

            else if (_instance != this)

                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public static CoroutineManager Instance { get { return _instance; } }

        public delegate bool CoroutineCondition();

        void OnDestroy()
        {
            m_IsDestroyed = true;
        }

        public Coroutine StartMyCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }


        public void StopMyCoroutine(Coroutine coroutine)
        {
            if (!m_IsDestroyed)
            {
                StopCoroutine(coroutine);
            }
        }


        public IEnumerator WaitForEndOfTheFrame(Action inOnComplete = null)
        {

            yield return new WaitForEndOfFrame();
            if (inOnComplete != null)
            {
                inOnComplete();
            }
        }



        public void WaitForSecondsForAction(float inDelay, Action inOnComplete = null)
        {
            StartMyCoroutine(WaitForSeconds(inDelay, inOnComplete));
        }


        public IEnumerator WaitForSeconds(float inDelay, Action inOnComplete = null)
        {
            yield return new WaitForSeconds(inDelay);
            yield return new WaitForEndOfFrame(); //if delay is 0

            inOnComplete?.Invoke();

        }

    }
}