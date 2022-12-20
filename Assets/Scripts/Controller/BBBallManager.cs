using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;
namespace BallBlast
{
    public class BBBallManager : SingletonObserverBase<BBBallManager>
    {
        ObjectPoolManager<BBBall> m_BallPoolManagerInstance;


        public override void Init()
        {
            base.Init();
            m_BallPoolManagerInstance = new ObjectPoolManager<BBBall>();
        }

        public void SpawnNextBall()
        {

        }

        public Vector2 GetBallSpawnPoint()
        {
            var commonRefHolder = BBManagerMediator.Instance().CommonRefHolderMB;
            int totalRandomPositions = commonRefHolder.BallSpawnPoints.SpawnPoint.Length;
            int radomIndex = Random.Range(0, totalRandomPositions);

            Vector2 position = commonRefHolder.BallSpawnPoints.SpawnPoint[radomIndex].position;

            return position;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}