using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Common
{
    public class InputManager : SingletonObserverBase<InputManager>
    {
        private event Action<SwipeDirection, float> m_TouchUpdate;
        private event Action m_MouseDownEvent;
        private event Action m_MouseClickAndHoldEvent;
        private event Action m_MouseUpEvent;
        private event Action<Vector2> m_MousePositionUpdateEvent;

        private Vector2 m_FingerDownPos, m_FingerUpPos, m_PreviousMousePostion;

        private SwipeDirection m_PreviousSwipeDir;
        private bool m_IsGamePause, m_IsGameOver;

        public override void Init()
        {
            base.Init();
            m_IsGamePause = m_IsGameOver = false;

            BallBlast.Events.GameEventManager.Instance().OnGameOver -= () => m_IsGameOver = true;
            BallBlast.Events.GameEventManager.Instance().OnGameOver += () => m_IsGameOver = true;

            BallBlast.Events.GameEventManager.Instance().OnGamePause -= (status) => m_IsGamePause = status;
            BallBlast.Events.GameEventManager.Instance().OnGamePause += (status) => m_IsGamePause = status;

            m_FingerDownPos = m_PreviousMousePostion = m_FingerUpPos = Vector2.zero;
            m_PreviousSwipeDir = SwipeDirection.none;
        }

        public void OnUpdate()
        {

            if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject() || m_IsGamePause|| m_IsGameOver) return;

            // TouchDevices (); // Enable based on touch swipe is required

            MouseClikEventDetection();
            MousePositionUpdate();

#if UNITY_EDITOR
            NonTouchDevices();
#endif

        }

        private void MousePositionUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 inputMouse = Input.mousePosition;
                Vector2 currentMousPosInWorldCo = Camera.main.ScreenToWorldPoint(inputMouse);

                if (inputMouse.x == m_PreviousMousePostion.x || m_MousePositionUpdateEvent == null) return;

                m_PreviousMousePostion = inputMouse;
                m_MousePositionUpdateEvent?.Invoke(currentMousPosInWorldCo);
            }
        }

        private void NonTouchDevices()
        {

#if UNITY_EDITOR
            KeyBoardInput();
#endif
        }


        #region  EVENT-SUBSCRIPTION and UN-SUBSCRIPTION

        public void SubscribeToGetSwipe(Action<SwipeDirection, float> action)
        {
            m_TouchUpdate += action;
        }

        public void UnSubscribeToGetSwipet(Action<SwipeDirection, float> action)
        {
            m_TouchUpdate -= action;
        }

        public void SubscribeToGetMousePosition(Action<Vector2> inCallBack)
        {
            m_MousePositionUpdateEvent += inCallBack;
        }

        public void UnSubscribeToGetMousePosition(Action<Vector2> inCallBack)
        {
            m_MousePositionUpdateEvent -= inCallBack;
        }

        internal void PauseUpdate(bool inPause)
        {
            m_IsGamePause = inPause;
        }

        public void SubscribeToMouseEvent(Action inOnMouseDown, Action inOnMouseUp, Action inMouseClickAndHold)
        {
            m_MouseDownEvent += inOnMouseDown;
            m_MouseUpEvent += inOnMouseUp;
            m_MouseClickAndHoldEvent += inMouseClickAndHold;
        }

        public void UnSubscribeToMouseEvent(Action inOnMouseDown, Action inOnMouseUp, Action inMouseClickAndHold)
        {
            m_MouseDownEvent -= inOnMouseDown;
            m_MouseUpEvent -= inOnMouseUp;
            m_MouseClickAndHoldEvent -= inMouseClickAndHold;
        }

        #endregion

        #region  MOUSE EVENTS
        private void MouseClikEventDetection()
        {
#if UNITY_EDITOR
            if (!Input.mousePresent) return;

#endif
            if (Input.GetMouseButtonUp(0))
            {
                m_MouseUpEvent?.Invoke();
            }
            if (Input.GetMouseButtonDown(0))
            {
                m_MouseDownEvent?.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                m_MouseClickAndHoldEvent?.Invoke();
            }
        }
        private void MouseSwipeDetection()
        {
#if UNITY_EDITOR
            // if not using the mouse, bail
            if (!Input.mousePresent) return;

#endif
            if (Input.GetMouseButtonUp(0))
            {
                m_FingerUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DetectSwipe();
            }

            if (Input.GetMouseButtonDown(0))
            {
                m_FingerUpPos = m_FingerDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            else if (Input.GetMouseButton(0))
            {
                m_FingerDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DetectSwipe();
            }
        }
        #endregion

        #region  KEY_BOARD INPUT
        private void KeyBoardInput()
        {
            int unitDisatnce = 1;
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                InformSwipe(SwipeDirection.eUp, unitDisatnce);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                InformSwipe(SwipeDirection.eDown, unitDisatnce);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InformSwipe(SwipeDirection.eLeft, unitDisatnce);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                InformSwipe(SwipeDirection.eRight, unitDisatnce);
            }
#endif
        }

        #endregion

        #region  TOUCH
        private void TouchDevices()
        {

            MouseClikEventDetection();

            TouchSwipeDetection();

        }

        private void TouchSwipeDetection()
        {
            if (Input.touchCount < 0 || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            // else Go through touch

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    m_FingerUpPos = touch.position;
                    m_FingerDownPos = touch.position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    m_FingerDownPos = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    m_FingerDownPos = touch.position;
                }
                DetectSwipe();
            }
        }

        #endregion

        #region  SWIPE-DETCTION
        void DetectSwipe()
        {
            float verticalDist = VerticalMoveValue();
            float horizontalDist = HorizontalMoveValue();

            if (verticalDist > UtilityConstants.SWIPE_THRESHOLD && verticalDist > horizontalDist)
            {
                // GameUtilities.ShowLog("Vertical Swipe Detected!");

                if (m_FingerDownPos.y - m_FingerUpPos.y > 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                            InformSwipe(SwipeDirection.eUp,verticalDist);
#else
                    InformSwipe(SwipeDirection.eDown, verticalDist);
#endif
                }
                else if (m_FingerDownPos.y - m_FingerUpPos.y < 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                            InformSwipe(SwipeDirection.eDown,verticalDist);
#else
                    InformSwipe(SwipeDirection.eUp, verticalDist);
#endif
                }
                m_FingerUpPos = m_FingerDownPos;

            }
            else if (horizontalDist > UtilityConstants.SWIPE_THRESHOLD)//&& horizontalDist > verticalDist)
            {
                // GameUtilities.ShowLog("Horizontal Swipe Detected!");

                if (m_FingerDownPos.x - m_FingerUpPos.x > 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    InformSwipe(SwipeDirection.eRight,horizontalDist);
#else
                    InformSwipe(SwipeDirection.eRight, horizontalDist);
#endif

                }
                else if (m_FingerDownPos.x - m_FingerUpPos.x < 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                InformSwipe(SwipeDirection.eLeft,horizontalDist);
#else
                    InformSwipe(SwipeDirection.eLeft, horizontalDist);
#endif
                }
                m_FingerUpPos = m_FingerDownPos;

            }
            else
            {
#if DEBUG
                GameUtilities.ShowLog("No Swipe Detected!");

#endif
            }
        }


        float VerticalMoveValue()
        {
            return Mathf.Abs(m_FingerDownPos.y - m_FingerUpPos.y);
        }


        float HorizontalMoveValue()
        {
            return Mathf.Abs(m_FingerDownPos.x - m_FingerUpPos.x);
        }

        private void InformSwipe(SwipeDirection inDirection, float swipeDistance)
        {
            if (inDirection == SwipeDirection.none)
            {
                return;
            }

#if DEBUG
            GameUtilities.ShowLog($"InformSwipe : m_PreviousSwipeDir{m_PreviousSwipeDir}");
#endif
            m_TouchUpdate?.Invoke(inDirection, swipeDistance);
        }

        #endregion
        public override void OnDestroy()
        {
            base.OnDestroy();
            m_TouchUpdate = null;
            m_MouseDownEvent = null;
            m_MouseUpEvent = null;

        }

    }
}