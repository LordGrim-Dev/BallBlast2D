using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BBGameSceneManager : MonoBehaviour
    {

        void Start()
        {
            BBGameManager.Instance().Init();
            BBGameManager.Instance().StartGame();
        }


        void Update()
        {
            BBGameManager.Instance().OnUpdate();
        }
    }
}