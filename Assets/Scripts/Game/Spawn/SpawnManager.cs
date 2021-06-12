using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting
    }

    [Serializable]
    public class Wave
    {
        public Transform enemy;
        public int numberOfEnemies = 3;
        public float rate;
    }

    public Wave wave;
    public Enemy enemyStats;
    public Transform[] spawnPoints;

    public int currentWave;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float _enemySearchCountdown = 1f;

    public SpawnState state = SpawnState.Counting;

    //cuando el player cruza el portal (teleportplayer)
    public bool gameStarted;

    private void Start()
    {
        gameStarted = false;
        waveCountdown = timeBetweenWaves;
        currentWave = 1;
    }

    private void Update()
    {
        if (!gameStarted) return;
            
        if (state == SpawnState.Waiting)
        {
            //check enemies still alive
            if (!IsEnemyAlive())
            {
                //new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                //start
                StartCoroutine(SpawnWave(wave));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;
        wave.numberOfEnemies += 3;

        currentWave++;

        Debug.Log("Wave compeleted" + wave.numberOfEnemies);
    }

    bool IsEnemyAlive()
    {
        _enemySearchCountdown -= Time.deltaTime;
        if (_enemySearchCountdown <= 0f)
        {
            _enemySearchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.Spawning;

        //Mejora de enemigos por oleada
        enemyStats.sightRange += 5;
        enemyStats.maxHealth += 15;

        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            SpawnEnemy(wave.enemy);
            //wave delay
            yield return new WaitForSeconds(wave.rate);
        }

        state = SpawnState.Waiting;
    }

    void SpawnEnemy(Transform enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}