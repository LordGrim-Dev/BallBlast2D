using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public interface IUnityUpdateCallbacks
    {
        bool IsObjectEnabled { get; }
        bool IsObjectActiveInScene { get; }
        void OnUpdate();
        void OnFixedUpdate();

        void OnLateUpdate();

    }
}
