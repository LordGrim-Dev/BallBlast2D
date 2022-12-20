using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;



namespace BallBlast
{

    public class BBGameManager : SingletonObserverBase<BBGameManager>
    {
        public override void Init()
        {
            base.Init();
            InputManager.Instance();
            BBScreenBorderHandler.Instance();


            BBManagerMediator mediator = BBManagerMediator.Instance();
            mediator.PlayerController.Init();
            mediator.BulletManagerMB.Init();

            BBScreenBorderHandler.Instance().Init(mediator.CommonRefHolderMB.MainCamera);
        }

        public void StartGame()
        {

        }


        public void OnUpdate()
        {
            InputManager.Instance().OnUpdate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}