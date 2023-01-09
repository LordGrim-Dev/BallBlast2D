using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class UtilityConstants
    {

        public const int MOUSE_UP = 1;
        public const int MOUSE_DOWN = 0;
        public const int MOUSE_CLICK_AND_HOLD = 3;
#if UNITY_ANDROID
        public const float SWIPE_THRESHOLD = 1f;
#else
        public const float SWIPE_THRESHOLD = 2f;
#endif
    }
    public enum SwipeDirection
    {
        none = 0,
        eUp,
        eDown,
        eRight,
        eLeft,
    }

}