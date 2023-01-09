using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace BallBlast.ECS
{
    public struct BulletComponent : IComponentData
    {
        // time in Seconds -> used for tweening the position
        public float m_MoveSpeed;
        public float m_TopBorderPosition;
    }
}