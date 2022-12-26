using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;
namespace BallBlast
{
    public class BBBallManager : SingletonObserverBase<BBBallManager>
    {
        ObjectPoolManager<BBBall> m_BallPoolManagerInstance;

        private Vector2 m_LeftForce, m_RightForce, m_TopForce, m_BottomForce;

        private int m_TotalBallsOnScreen;
        private float m_SpawnTimeGap;

        public void Init(BBBallManagerMB inInstance)
        {
            m_BallPoolManagerInstance = new ObjectPoolManager<BBBall>(inInstance.BallPrefab, inInstance.transform);

            m_LeftForce = Vector2.left;
            m_RightForce = Vector2.right;
            m_TopForce = Vector2.up;
            m_BottomForce = Vector2.down;

            m_TotalBallsOnScreen = 0;
            m_SpawnTimeGap = BBConstants.k_MAX_SPAWN_TIME_GAP;

            CoroutineManager.Instance.StartCoroutine(CheckAndSpawnNewBall());

        }

        private IEnumerator CheckAndSpawnNewBall()
        {
            bool nextSpawnIsRequired = false;
            while (true)
            {
                nextSpawnIsRequired = m_TotalBallsOnScreen < BBConstants.kMAX_BALLS_ON_SCREEN;
                if (nextSpawnIsRequired)
                {
                    if (m_SpawnTimeGap <= 0)
                    {
                        m_SpawnTimeGap = BBConstants.k_MAX_SPAWN_TIME_GAP;
                        SpawnNextBall();
                    }
                    m_SpawnTimeGap -= Time.deltaTime;
                }

                yield return null;
            }
        }

        public void SpawnNextBall()
        {
            m_TotalBallsOnScreen++;
            Vector2 randomPosition = GetBallSpawnPoint();
            IPoolMember newBall = m_BallPoolManagerInstance.GetNewBallFromPool();
            IBallProperties ballProperties = newBall as IBallProperties;
            ballProperties.InitialiseNewBall(GetNextMaxCountForBall(), randomPosition, 0, true);
        }


        public Vector2 GetBallSpawnPoint()
        {
            var commonRefHolder = BBManagerMediator.Instance().CommonRefHolderMB;
            int totalRandomPositions = commonRefHolder.BallSpawnPoints.SpawnPoint.Length;
            int radomIndex = Random.Range(0, totalRandomPositions);
            Vector2 position = commonRefHolder.BallSpawnPoints.SpawnPoint[radomIndex].position;
            return position;
        }


        public void OnBallDeath()
        {
            m_TotalBallsOnScreen--;
            if (m_TotalBallsOnScreen <= 0) m_TotalBallsOnScreen = 0;
        }

        internal void GetBorderDirection(Transform transform, out Direction whichBorder)
        {
            BBBorderManager.Instance().DidWeHitBorder(transform, out whichBorder);
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            m_BallPoolManagerInstance = null;
        }

        internal void CheckForSplitAndSpawn(uint inMaxHitCount)
        {
            if (inMaxHitCount <= 0)
            {

            }
            else
            {
                int numberOfSpawnForSplit = 2;

                bool toLeft = false;
                float xVelocity = 1;

                for (int i = 0; i < numberOfSpawnForSplit; i++)
                {
                    IPoolMember newBall = m_BallPoolManagerInstance.GetNewBallFromPool();
                    IBallProperties ballProper = newBall as IBallProperties;

                    Vector2 postion = ballProper.Position;
                    if (toLeft)
                    {
                        xVelocity = -1;
                    }
                    else
                        xVelocity = 1;

                    ballProper.InitialiseNewBall(inMaxHitCount / 2, postion, xVelocity, false);
                    toLeft = !toLeft;
                }
            }
        }


        public uint GetNextMaxCountForBall()
        {
            return (uint)Random.Range(8, 10);
        }
    }
}