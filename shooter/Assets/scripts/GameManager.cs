using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private NavMeshBuilder navMeshBuilder;

    private void Awake ()
    {
        navMeshBuilder = GetComponent<NavMeshBuilder>();
    }

    private void Start ()
    {
        navMeshBuilder.BuildNavMesh();
    }
}
