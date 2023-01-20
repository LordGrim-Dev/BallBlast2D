using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace BallBlast.UI
{
    public class LevelUpUI : BaseUI
    {

        [SerializeField]
        TMPro.TextMeshProUGUI m_LevelHeader;


        [SerializeField]
        TMPro.TextMeshProUGUI m_LevelCount;

        [SerializeField]
        Image m_NumberBG;

        private int m_TargetLevelCount;
        Action m_AnimationStarted;

        private void OnEnable()
        {
            m_LevelHeader.text = config.BBConfigManager.Instance().GetLocalisedStringForKey(config.ConfigJsonConstants.kLevel);

            m_LevelCount.transform.DOKill();

            m_NumberBG.fillAmount = 1;

            StartCoroutine(StartAnimation());
        }

        //Called before Starting animation
        public void Initialise(int inTargetLevelCount, Action inAnimationAtrted)
        {
            m_AnimationStarted = inAnimationAtrted;


            m_TargetLevelCount = inTargetLevelCount;

            m_LevelCount.text = (inTargetLevelCount - 1).ToString();
        }


        private IEnumerator StartAnimation()
        {
            yield return null;
            m_LevelHeader.transform.DOKill();

            float duration = 1;
            m_NumberBG.DOFillAmount(0, duration).SetEase(Ease.OutBack).
            OnStart(() => m_AnimationStarted?.Invoke()).
            OnComplete(() =>
            {
                m_NumberBG.DOFillAmount(1, duration).SetEase(Ease.InBack).OnComplete(() =>
                {
                    BBUIManager.Instance().OnLevelUpAnimationCompleted();
                    ResetChanges();
                });
            });

        }

        private void ResetChanges()
        {
            m_LevelHeader.transform.DOKill();
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

    }
}
