using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBCommonRefHolderMB : MonoBehaviour
{
    [SerializeField]
    private Camera m_MainCamera;

    [SerializeField]
    BallSpawnPoints m_BallSpawnPoints;

    public Camera MainCamera { get => m_MainCamera; }

    public BallSpawnPoints BallSpawnPoints { get => m_BallSpawnPoints; }
}

[System.Serializable]
public class BallSpawnPoints
{
    [SerializeField]
    private Transform[] m_SpawnPoint;
    public Transform[] SpawnPoint { get => m_SpawnPoint; }
}
