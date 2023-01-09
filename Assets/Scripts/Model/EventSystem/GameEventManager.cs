using System;
using Game.Common;

namespace BallBlast.Events
{
    internal class GameEventManager : SingletonObserverBase<GameEventManager>
    {

        public event Action<bool> OnGamePause;

        public event Action OnGameOver;

        public event Action OnGameStarted;

        public event Action OnLevelCompleted;

        public event Action<int> OnLevelUp;

        public event Action<int> OnPlayerLifeLost;

        internal void TriggerPause(bool pauseStatus)
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerPause : " + pauseStatus);
#endif
            OnGamePause?.Invoke(pauseStatus);
        }

        internal void TriggerGameOver()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerGameOver");
#endif
            OnGameOver?.Invoke();
        }

        internal void TriggerStartGame()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerStartGame");
#endif
            OnGameStarted?.Invoke();
        }

        internal void TriggerLevelCompleted()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerLevelCompleted");
#endif
            OnLevelCompleted?.Invoke();
        }

        internal void TriggerLevelUp(int inNextLevel)
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerLevelUp");
#endif
            OnLevelUp?.Invoke(inNextLevel);
        }

        public void TriggerPlayerLifeLost(int inRemainingLive)
        {
#if DEBUG
            GameUtilities.ShowLog($"TriggerPlayerLifeLost : {inRemainingLive}");
#endif
            OnPlayerLifeLost?.Invoke(inRemainingLive);
        }
    }
}