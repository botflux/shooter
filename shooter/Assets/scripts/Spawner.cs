using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool devMode;

    public Wave[] waves;
    public Enemy enemy;

    private LivingEntity playerEntity;
    private Transform playerTransform;

    private Wave currentWave;
    private int currentWaveNumber;

    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    private MapGenerator map;

    private float timeBetweenCampingChecks = 2f;
    private float campThresholdDistance = 1.5f;
    private float nextCampCheckTime;
    private Vector3 campPositionOld;
    private bool isCamping;

    private bool isDisabled;

    public event System.Action<int> OnNewWave;

    private void Awake ()
    {
        map = FindObjectOfType<MapGenerator>();
        playerEntity = FindObjectOfType<Player>();
        playerTransform = playerEntity.transform;
        playerEntity.OnDeath += OnPlayerDeath;
        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerTransform.position;
    }

    private void Update ()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;
                isCamping = (Vector3.Distance(playerTransform.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerTransform.position;

            }

            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine("SpawnEnemy");
            }
        }

        if (devMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine("SpawnEnemy");
                foreach(Enemy enemy in FindObjectsOfType<Enemy> ())
                {
                    Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    IEnumerator SpawnEnemy ()
    {
        float spawnDelay = 1f;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerTransform.position);
        }

        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = Color.white;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed,1)); 

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;

        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);
    }

    private void OnEnemyDeath ()
    {
        Debug.Log("Enemy died");
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    private void OnPlayerDeath ()
    {
        isDisabled = true;
    }

    public void NextWave()
    {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {

            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
                OnNewWave(currentWaveNumber);

            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition ()
    {
        playerTransform.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3f;
    }

    [System.Serializable]
    public class Wave
    {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColor;

    }
}
