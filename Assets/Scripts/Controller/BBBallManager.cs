using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;
namespace BallBlast
{
    public class BBBallManager : SingletonObserverBase<BBBallManager>
    {
        ObjectPoolManager<BBBall> m_ParentBallPool;

        ObjectPoolManager<BBBall> m_SplitBallPool;

        private int m_TotalParentBallOnScreen, m_SplitBallsOnScreen;

        private float m_SpawnTimeGap;

        private bool m_IsGamePaused, m_SpawnAllowed;

        // These two variable used to monitor level up
        // Max-Count = maximum count for the level to clear up
        // will be distributed among balls with distribution
        private uint m_MaxCountForTheLevel, m_CurrentLevelDrawCountPending;
        private uint[] m_BallSizeProbabality;
        Coroutine m_CheckAndSpawnParent;

        bool m_IsLevelUpRequired, m_IsInitialisationComplete;

        public void Init(BBBallManagerMB inInstance)
        {
            m_IsInitialisationComplete = false;
            m_ParentBallPool = new ObjectPoolManager<BBBall>(inInstance.ParentBall, inInstance.transform);
            m_SplitBallPool = new ObjectPoolManager<BBBall>(inInstance.SplitBall, inInstance.transform);

            OnLoadNextLevel();

            m_TotalParentBallOnScreen = m_SplitBallsOnScreen = 0;
            m_SpawnTimeGap = BBConstants.k_MAX_SPAWN_TIME_GAP;

            m_IsGamePaused = false;
            m_IsLevelUpRequired = false;
            m_SpawnAllowed = true;

            Events.GameEventManager.Instance().OnGamePause -= OnPauseGame;
            Events.GameEventManager.Instance().OnGamePause += OnPauseGame;

            Events.GameEventManager.Instance().OnGameStarted -= StartParentBallSpawn;
            Events.GameEventManager.Instance().OnGameStarted += StartParentBallSpawn;

            Events.GameEventManager.Instance().OnGamePause -= PauseAll;
            Events.GameEventManager.Instance().OnGamePause += PauseAll;

            Events.GameEventManager.Instance().OnGameOver -= OnGameOver;
            Events.GameEventManager.Instance().OnGameOver += OnGameOver;

            Events.GameEventManager.Instance().OnLevelCompleted -= HideAll;
            Events.GameEventManager.Instance().OnLevelCompleted += HideAll;


            Events.GameEventManager.Instance().OnLoadNextLevel -= OnLoadNextLevel;
            Events.GameEventManager.Instance().OnLoadNextLevel += OnLoadNextLevel;

            m_IsInitialisationComplete = true;

        }

        private void OnGameOver()
        {
            if (m_CheckAndSpawnParent != null)
                CoroutineManager.Instance.StopMyCoroutine(m_CheckAndSpawnParent);
            PauseAll(true);
        }

        public void PauseAll(bool inPauseStatus)
        {
            m_ParentBallPool.OnPause(inPauseStatus);
            m_SplitBallPool.OnPause(inPauseStatus);
        }

        private void OnLoadNextLevel()
        {
            m_SpawnAllowed = true;

            var currentLevelD = config.BBConfigManager.Instance().GetCurrentLevelData();

            m_MaxCountForTheLevel = currentLevelD.MaxCount;
            m_BallSizeProbabality = currentLevelD.BallSizeProbability;
            m_CurrentLevelDrawCountPending = m_MaxCountForTheLevel;
        }

        private void OnPauseGame(bool pauseStatus)
        {
            m_IsGamePaused = pauseStatus;
        }

        private void StartParentBallSpawn()
        {
            if (m_CheckAndSpawnParent != null)
                CoroutineManager.Instance.StopMyCoroutine(m_CheckAndSpawnParent);

            m_CheckAndSpawnParent = CoroutineManager.Instance.StartMyCoroutine(CheckAndSpawnNewBall());
        }

        private IEnumerator CheckAndSpawnNewBall()
        {
            bool nextSpawnIsRequired = false;
            while (true)
            {
                if (m_IsGamePaused || !m_SpawnAllowed || !m_IsInitialisationComplete) yield return null;

                m_IsLevelUpRequired = !(m_CurrentLevelDrawCountPending > 1);

                bool isChildBallCountReachedMax = m_SplitBallsOnScreen < BBConstants.k_MAX_SPLIT_BALLS_ON_SCREEN;
                bool isParentballReachedMax = m_TotalParentBallOnScreen < BBConstants.k_MAX_PARENT_BALLS_ON_SCREEN;

                nextSpawnIsRequired = isParentballReachedMax && !m_IsLevelUpRequired && isChildBallCountReachedMax;

                if (nextSpawnIsRequired)
                {
                    if (m_SpawnTimeGap <= 0)
                    {
                        m_SpawnTimeGap = BBConstants.k_MAX_SPAWN_TIME_GAP;
                        SpawnParentBall();
                    }
                    m_SpawnTimeGap -= Time.deltaTime;
                }

                yield return null;
            }
        }

        private void SpawnParentBall()
        {
            m_TotalParentBallOnScreen++;
            Vector3 randomPosition = GetBallSpawnPoint();
            IPoolMember newBall = m_ParentBallPool.GetNewBallFromPool();

            IBallProperties ballProperties = newBall as IBallProperties;

            uint nextParentBallHit = GetMaxHitCount();

            m_CurrentLevelDrawCountPending -= nextParentBallHit;

            ballProperties.InitialiseNewBall(nextParentBallHit, randomPosition, GetNextBallSize(), 0);
        }


        private Vector3 GetBallSpawnPoint()
        {
            var commonRefHolder = BBManagerMediator.Instance().CommonRefHolderMB;
            int totalRandomPositions = commonRefHolder.BallSpawnPoints.SpawnPoint.Length;
            int radomIndex = UnityEngine.Random.Range(0, totalRandomPositions);

            Vector3 position = commonRefHolder.BallSpawnPoints.SpawnPoint[radomIndex].position;
            position.z = GetZOrderValue();

            return position;
        }


        public void OnBallDeath(uint inId)
        {
            if (inId == BBConstants.k_PARENT_BALL_ID)
            {
                m_TotalParentBallOnScreen--;

                m_TotalParentBallOnScreen = m_TotalParentBallOnScreen <= 0 ? 0 : m_TotalParentBallOnScreen;
            }
            else if (inId == BBConstants.k_SPLIT_BALL_ID)
            {
                OnSplitBallDeath();
            }

            int ParentBallFromPoolEnabled = m_ParentBallPool.GetAllEnabledPoolMemberCount();
            int childCountsEnabled = m_SplitBallPool.GetAllEnabledPoolMemberCount();
            bool levelCompleted = (ParentBallFromPoolEnabled == 0 && childCountsEnabled == 0
                                    && m_IsLevelUpRequired && (m_SplitBallsOnScreen <= 0));

#if DEBUG
            string log = $"parentballCount : {ParentBallFromPoolEnabled} , childCountsEnabled : {childCountsEnabled} ,m_IsLevelUpRequired: {m_IsLevelUpRequired} , m_m_SplitBallsOnScreen: {m_SplitBallsOnScreen}";
            GameUtilities.ShowLog(log);
#endif

            if (levelCompleted)
                Events.GameEventManager.Instance().TriggerLevelCompleted();
        }

        internal void CheckHitBorderAndGetDir(Transform transform, BallSize currentBallSizeLevel, out Direction whichBorder)
        {
            BBBorderManager.Instance().DidWeHitBorder(transform, out whichBorder);
            if (whichBorder == Direction.eBottom)
            {
                GetShakeStrength(currentBallSizeLevel, out float strength, out float duration);
                BBManagerMediator.Instance().MainCamera.ShakeCamera(duration, strength);
            }
        }

        private void GetShakeStrength(BallSize currentBallSizeLevel, out float outShakeStrength, out float outShakeDur)
        {
            outShakeStrength = 0.0f;
            outShakeDur = 0.01f;
            switch (currentBallSizeLevel)
            {
                case BallSize.eLevel_3:
                    outShakeStrength = 0.285f;
                    outShakeDur = 0.75f;
                    break;

                case BallSize.eLevel_2:
                    outShakeStrength = 0.20f;
                    outShakeDur = 0.50f;
                    break;

                case BallSize.eLevel_1:
                    outShakeStrength = 0.10f;
                    outShakeDur = 0.20f;
                    break;

            }
        }


        internal void CheckForSplitAndSpawn(uint inID, uint inMaxHitCount, Vector3 inParentPos, BallSize inCurrentBallSize)
        {
            if (inID == BBConstants.k_SPLIT_BALL_ID)
                OnSplitBallDeath();

            uint hitCountForSplitBall = inMaxHitCount / 2;
            if (hitCountForSplitBall > 1)
            {
                int numberOfSpawnForSplit = 2;
                bool toLeft = false;
                float xVelocity;

                for (int i = 0; i < numberOfSpawnForSplit; i++)
                {
                    IPoolMember newBall = m_SplitBallPool.GetNewBallFromPool();
                    IBallProperties ballProperties = newBall as IBallProperties;

                    Vector3 postion = inParentPos;

                    xVelocity = BBConstants.k_SPLIT_BALL_INIT_X_VELOCITY;

                    if (toLeft)
                    {
                        xVelocity = -BBConstants.k_SPLIT_BALL_INIT_X_VELOCITY;
                    }

                    postion.z = GetZOrderValue();

                    ballProperties.InitialiseNewBall(hitCountForSplitBall, postion, inCurrentBallSize, xVelocity);
                    toLeft = !toLeft;

                }

                m_SplitBallsOnScreen += numberOfSpawnForSplit;
            }

        }

        private void OnSplitBallDeath()
        {
            m_SplitBallsOnScreen--;

            m_SplitBallsOnScreen = m_SplitBallsOnScreen < 0 ? 0 : m_SplitBallsOnScreen;
        }

        public bool IsHitBallID(int inId)
        {
            IPoolMember poolMember = null;

            m_SplitBallPool.IsItemPresentInPool(inId, out poolMember);
            if (poolMember == null)
            {
                m_ParentBallPool.IsItemPresentInPool(inId, out poolMember);
            }
            return poolMember != null;
        }

        private BallSize GetNextBallSize()
        {
            BallSize size = BallSize.eLevel_0;

            int randomNum = UnityEngine.Random.Range(0, 100);

            uint pm = m_BallSizeProbabality[(uint)BallSize.eLevel_0];
            uint pn = m_BallSizeProbabality[(uint)BallSize.eLevel_1];
            uint po = m_BallSizeProbabality[(uint)BallSize.eLevel_2];
            uint pp = m_BallSizeProbabality[(uint)BallSize.eLevel_3];

            if (randomNum <= pm)
            {
                // Level 0
                size = BallSize.eLevel_0;

            }
            else if (randomNum <= (pm + pn))
            {
                // Level -1
                size = BallSize.eLevel_1;
            }

            else if (randomNum <= (pm + pn + po))
            {
                // Level-3
                size = BallSize.eLevel_2;
            }
            else
            {
                // Level-4
                size = BallSize.eLevel_3;
            }

            return size;
        }


        private void HideAll()
        {
            m_SpawnAllowed = false;
            m_SplitBallPool.HideAll();
            m_ParentBallPool.HideAll();
        }

        private uint GetMaxHitCount()
        {
            uint nextBallHitCount;
            nextBallHitCount = (uint)UnityEngine.Random.Range(2, m_CurrentLevelDrawCountPending);
            return nextBallHitCount;
        }



        //Get different Z value to set the order in View
        private float GetZOrderValue()
        {
            return UnityEngine.Random.Range(-0.1f, -1f);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            m_ParentBallPool = m_SplitBallPool = null;
        }
    }
}