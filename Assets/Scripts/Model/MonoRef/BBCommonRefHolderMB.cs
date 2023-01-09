using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBCommonRefHolderMB : MonoBehaviour
{

    [SerializeField]
    BallSpawnPoints m_BallSpawnPoints;

    [SerializeField]
    Transform m_BGSprite;
    
    public BallSpawnPoints BallSpawnPoints { get => m_BallSpawnPoints; }
    public Transform BGSprite { get => m_BGSprite; }
}

[System.Serializable]
public class BallSpawnPoints
{
    [SerializeField]
    private Transform[] m_SpawnPoint;
    public Transform[] SpawnPoint { get => m_SpawnPoint; }
}
