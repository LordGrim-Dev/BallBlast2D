using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BallBlast.UI
{
    public class CounterUI : BaseUI
    {
        [SerializeField]
        TMPro.TextMeshProUGUI m_CounterText;

        private int m_MaxCountDownTime;

        private void OnEnable()
        {
            m_CounterText.transform.DOKill();

            m_MaxCountDownTime = 3;

            StartCoroutine(StartCounter());
        }
        

        private IEnumerator StartCounter()
        {
            m_CounterText.transform.DOKill();
            Transform transform = m_CounterText.transform;
            Vector3 originalScale = transform.localScale;
            Vector3 scaleUp = originalScale * 2;
            float duration = 0.65f;
            float fade = 0.20f;
            Color originalColor = m_CounterText.color;

            while (true)
            {

                m_CounterText.text = m_MaxCountDownTime.ToString();
                transform.DOScale(scaleUp, duration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    m_CounterText.DOFade(0, fade).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        m_CounterText.gameObject.SetActive(false);
                        m_CounterText.color = originalColor;
                    });
                });

                if (m_MaxCountDownTime == 0)
                {
                    BBUIManager.Instance().OnCounterZero();
                    ResetChanges(originalColor);
                    yield break;
                }

                yield return new WaitForSeconds(1);

                transform.localScale = originalScale;
                m_CounterText.gameObject.SetActive(true);


                m_MaxCountDownTime--;

            }
        }
        
        private void ResetChanges(Color inOriginalcolor)
        {
            m_CounterText.transform.DOKill();
            m_CounterText.color = inOriginalcolor;
            m_CounterText.text = m_MaxCountDownTime.ToString();
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

    }
}
