using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System;

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
        Action m_AnimationStarted, m_AnimationCompleted;

        public void Initialise(int inTargetLevelCount, Action inAnimationAtrted, Action inAnimationCompleted)
        {
            m_AnimationStarted = inAnimationAtrted;
            m_AnimationCompleted = inAnimationCompleted;
            m_TargetLevelCount = inTargetLevelCount;
        }

        private void OnEnable()
        {
            m_TargetLevelCount = 0;

            m_LevelHeader.text = config.BBConfigManager.Instance().GetLocalisedStringForKey(config.ConfigJsonConstants.kLevel);

            m_LevelCount.transform.DOKill();

            StartCoroutine(StartAnimation());
        }


        private IEnumerator StartAnimation()
        {
            yield return null;
            m_LevelHeader.transform.DOKill();

            m_LevelCount.DOFill

        }

        private void ResetChanges(Color inOriginalcolor)
        {
            m_LevelHeader.transform.DOKill();
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

    }
}
