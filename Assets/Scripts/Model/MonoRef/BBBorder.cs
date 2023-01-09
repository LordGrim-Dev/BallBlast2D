using Game.Common;
using UnityEngine;
namespace BallBlast
{
    public class BBBorder : MonoBehaviour
    {
        private Direction m_BorderPlacementDirection;
        public Direction BorderPlacementDirection { get => m_BorderPlacementDirection; set => m_BorderPlacementDirection = value; }
    }
}