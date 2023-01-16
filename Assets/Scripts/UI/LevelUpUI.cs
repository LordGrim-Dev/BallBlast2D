using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

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


        private void OnEnable()
        {
            m_LevelHeader.text = config.BBConfigManager.Instance().GetLocalisedStringForKey(config.ConfigJsonConstants.kLevel);

            m_LevelCount.transform.DOKill();

            StartCoroutine(StartAnimation());
        }


        private IEnumerator StartAnimation()
        {
            yield return null;
            m_LevelHeader.transform.DOKill();
        }

        private void ResetChanges(Color inOriginalcolor)
        {
            m_LevelHeader.transform.DOKill();
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

    }
}
