using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private Spawner spawner;

    private void Awake ()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        spawner = FindObjectOfType<Spawner>();

        spawner.OnNewWave += mapGenerator.OnNewWave;
    }

    private void Start ()
    {
        mapGenerator.GenerateMap();
        spawner.NextWave();
    }
}
