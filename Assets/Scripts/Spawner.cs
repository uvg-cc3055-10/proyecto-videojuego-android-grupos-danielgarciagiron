// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy[] enemies;
    public float x;
    public float y;
    public bool infiniteMode;

    public Wave[] waves;
    Wave WaveInfo;

    private int currentWave;
    private float currentDelay;
    private int currentID;
    private int counter;
    private int remainingEnemies;
    private bool gameHasEnded = false;
    private int enemyIndex;
    private int enemiesPerRound;
    private float timeBetweenRounds;
    private float nextSpawnTime;
    private bool hasFinished = false;
    private bool roundTimerReset = false;
    private float intermediateTimer;
    private float infiniteTimer;
    private bool playerDeath=false;

    void Start()
    {
        NextWave();
    }

    void Update()
    {
        if (infiniteMode == false)
        {
            if (gameHasEnded == false)
            {
                if (enemiesPerRound > 0 && Time.time > nextSpawnTime)
                {
                    Enemy spawnedEnemy = Instantiate(enemies[currentID], Vector3.zero, Quaternion.identity) as Enemy;
                    enemiesPerRound--;
                    nextSpawnTime = Time.time + currentDelay;
                }
                if (remainingEnemies == 0 && roundTimerReset == false)
                {
                    timeBetweenRounds = Time.time + intermediateTimer;
                    roundTimerReset = true;
                }
                if (Time.time > timeBetweenRounds && roundTimerReset == true)
                {
                    NextWave();
                }

            }
            else
            {
                if (hasFinished == false)
                {
                    EndGame();
                    hasFinished = true;
                }

            }
        }
        else
        {
            if (Time.time>infiniteTimer && playerDeath==false)
            {
                Enemy spawnedEnemy = Instantiate(enemies[currentID], Vector3.zero, Quaternion.identity) as Enemy;
                infiniteTimer = Time.time + 5;
            }
        }

    }

    void NextWave()
    {
        if (currentWave< waves.Length)
        {
            WaveInfo = waves[counter];
            currentWave = counter+1;
            currentDelay = WaveInfo.delayBetweenSpawns;
            currentID = WaveInfo.enemyID;
            intermediateTimer = WaveInfo.timer;
            enemiesPerRound = WaveInfo.numberOfEnemies;
            remainingEnemies = WaveInfo.numberOfEnemies;
            EventManager.triggerEvent("RoundDisplay", 1);
            roundTimerReset = false;
            counter++;
        }
        else
        {
            gameHasEnded = true;
        }
    }

    void EndGame()
    {
        EventManager.triggerEvent("EndGame", 1);
    }


    void OnEnable()
    {
        EventManager.startListening("EnemyDeath", EnemyDeath);
        EventManager.startListening("StopSpawner", StopSpawner);
    }

    void OnDisable()
    {
        EventManager.stopListening("EnemyDeath", EnemyDeath);
        EventManager.stopListening("StopSpawner", StopSpawner);
    }

    void StopSpawner(int info)
    {
        playerDeath = true;
    }

    void EnemyDeath(int info)
    {
        remainingEnemies--;
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyID;
        public int numberOfEnemies;
        public float delayBetweenSpawns;
        public float timer;
    }

}

