using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace BallBlast.UI
{
    public class BBUIManager : SingletonObserverBase<BBUIManager>
    {
        internal void OnCounterZero()
        {
            var ui = UserInterfaceSystem.Instance().LoadUI<GamePlayUI>((int)UserInterface.eGamePlayUI);

            UserInterfaceSystem.Instance().HideUI((uint)UserInterface.eCounterUI);

            UserInterfaceSystem.Instance().ShowUi(ui);

            Events.UI.UIEventManager.Instance().TriggerCountDownZero();
        }

        public void OnClickExitGame()
        {
            BBGameManager.Instance().OnExitGame();
        }


        public void OnClickContinue()
        {
            UserInterfaceSystem.Instance().HideUI((uint)UserInterface.eIntroUI);

            ShowCountDownUI();
        }

        internal void ShowIntroUI()
        {
            var ui = UserInterfaceSystem.Instance().LoadUI<IntroUI>((int)UserInterface.eIntroUI);

            UserInterfaceSystem.Instance().ShowUi(ui);
        }


        internal void ShowGameOverUI()
        {
            UserInterfaceSystem.Instance().HideUI((uint)UserInterface.eGamePlayUI);

            var ui = UserInterfaceSystem.Instance().LoadUI<GameOverUI>((int)UserInterface.eGameOverUI);

            UserInterfaceSystem.Instance().ShowUi(ui);
        }


        internal void ShowCountDownUI()
        {
            CounterUI ui = UserInterfaceSystem.Instance().LoadUI<CounterUI>((uint)UserInterface.eCounterUI);

            UserInterfaceSystem.Instance().ShowUi(ui, 4);

        }

        internal void ShowLevelUpUI(int inLevel, Action inLevelUpAnimationStart)
        {
            var ui = UserInterfaceSystem.Instance().LoadUI<LevelUpUI>((uint)UserInterface.eLevelUP);

            ui.Initialise(inLevel, inLevelUpAnimationStart);

            UserInterfaceSystem.Instance().ShowUi(ui, inOrder: 5);
        }


        internal void OnLevelUpAnimationCompleted()
        {
            UserInterfaceSystem.Instance().HideUI((uint)UserInterface.eLevelUP);

            var ui = (GamePlayUI)UserInterfaceSystem.Instance().GetUI(UserInterface.eGamePlayUI);

            int currentLevel = BBManagerMediator.Instance().ProgressManager.PlayerCurrentLevel;
            ui.OnLevelUP(currentLevel);

            BBGameManager.Instance().OnLevelUpAnimationCompleted();
        }


    }
}