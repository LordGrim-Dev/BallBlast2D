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

        public event Action<int> OnPlayerLifeLost;

        public event Action OnLoadNextLevel;

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

        public void TriggerPlayerLifeLost(int inRemainingLive)
        {
#if DEBUG
            GameUtilities.ShowLog($"TriggerPlayerLifeLost : {inRemainingLive}");
#endif
            OnPlayerLifeLost?.Invoke(inRemainingLive);
        }



        public void TriggerLoadNextLevel()
        {
#if DEBUG
            GameUtilities.ShowLog($"TriggerLoadNextLevel :");
#endif
            OnLoadNextLevel?.Invoke();
        }

    }
}