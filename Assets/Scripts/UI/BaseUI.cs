using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;
using UnityEngine.UI;

namespace BallBlast.UI
{

    public class BaseUI : MonoBehaviour
    {
        [SerializeField]
        GameObject m_BackgroundHolder;

        private void Start()
        {
            Initialise();
        }

        protected virtual void Initialise()
        {

        }

        public void SetUpBackroundSize(Vector2 inSize)
        {
#if DEBUG
            GameUtilities.ShowLog($"SetUpBackroundSize :{inSize}");
#endif
            if (m_BackgroundHolder != null)
            {
                RectTransform rectTrans = m_BackgroundHolder.GetComponent<RectTransform>();
                Vector2 half = new Vector2(0.5f, 0.5f);

                RectTransform parentRect = this.GetComponent<RectTransform>();

                rectTrans.anchorMin = half;
                rectTrans.anchorMax = half;

                rectTrans.offsetMin = rectTrans.offsetMax = Vector2.zero;

                Vector2 maxPos = (inSize - parentRect.sizeDelta) * (Vector2.one - parentRect.anchorMax) * half;
                Vector2 minPos = (inSize - parentRect.sizeDelta) * (Vector2.zero - parentRect.anchorMin) * half;
                rectTrans.anchoredPosition = (maxPos + minPos);

                AspectRatioFitter aspectFilter = this.GetComponent<AspectRatioFitter>();
                if (aspectFilter != null)
                {
                    rectTrans.anchoredPosition = Vector2.zero;
                }

                rectTrans.sizeDelta = inSize;
            }
        }

    }
}