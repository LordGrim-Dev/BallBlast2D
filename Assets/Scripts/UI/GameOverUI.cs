
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BallBlast.UI
{

    public class GameOverUI : BaseUI
    {
        [SerializeField]
        TextMeshProUGUI m_GameOverText;

        [SerializeField]
        TextMeshProUGUI m_ClickHereToExit;

        [SerializeField]
        TextMeshProUGUI m_ScoreHeader;

        [SerializeField]
        TextMeshProUGUI m_TotalScore;

        [SerializeField]
        Button m_ExitButton;

        private void OnEnable()
        {
            m_ExitButton.onClick.AddListener(OnClickExitGame);
        }
        

        void Start()
        {
            var score = BBScoreManager.Instance().TotalScore.ToString();

            var instance = config.BBConfigManager.Instance();
            
            m_GameOverText.text = instance.GetLocalisedStringForKey(config.ConfigJsonConstants.kGameOver);
           
            m_ClickHereToExit.text = instance.GetLocalisedStringForKey(config.ConfigJsonConstants.kClickToExit);
           
            m_ScoreHeader.text = instance.GetLocalisedStringForKey(config.ConfigJsonConstants.kScore);
           
            m_TotalScore.text = score;
        }
        
        
        private void OnClickExitGame()
        {
            BBUIManager.Instance().OnClickExitGame();
        }
        

    }
}
