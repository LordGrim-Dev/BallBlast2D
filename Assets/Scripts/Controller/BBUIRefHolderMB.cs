using UnityEngine;

namespace BallBlast
{
    internal class BBUIRefHolderMB : MonoBehaviour
    {
        [SerializeField]
        private Canvas m_ActiveCanvas;
        public Canvas Canvas => m_ActiveCanvas;
    }
}