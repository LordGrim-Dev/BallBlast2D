using System;
using Game.Common;

namespace BallBlast.Events.UI
{
    public class UIEventManager : SingletonObserverBase<UIEventManager>
    {
        public event Action<int> OnPlayerLivesUpdate;
        public event Action<int> OnPlayerScoreUpdate;

        public event Action OnCountDownZero;

        public void TriggerPlayerLivesUpdate(int inLivesLeft)
        {
            OnPlayerLivesUpdate?.Invoke(inLivesLeft);
        }
        

        public void TriggerPlayerScoreUpdate(int inScore)
        {
            OnPlayerScoreUpdate?.Invoke(inScore);
        }

        public void TriggerCountDownZero()
        {
            OnCountDownZero?.Invoke();
        }
    }
}
