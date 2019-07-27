﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject[] powerUpSpawns;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject ranger;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject healthPowerUp;
    [SerializeField] GameObject speedPowerUp;
    [SerializeField] Text levelText;
    [SerializeField] int maxPowerUps = 4;

    private bool gameOver = false;
    private int currentLevel;
    private float generatedSpawnTime = 1;
    private float currentSpawnTime = 0;
    private float powerUpSpawnTime = 5;
    private float currentPowerUpSpawnTime = 0;
    private GameObject newEnemy;
    private int powerUps = 0;
    private GameObject newPowerUp;

    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private List<EnemyHealth> killedEnemies = new List<EnemyHealth>();

    public void RegisterEnemy(EnemyHealth enemy)
    {
        enemies.Add(enemy);
    }

    public void KilledEnemy(EnemyHealth enemy)
    {
        killedEnemies.Add(enemy);
    }

    public void RegisterPowerUp()
    {
        powerUps++;
    }

    public void deletePowerUp()
    {
        powerUps--;
    }

    public bool GameOver
    {
        get { return gameOver;}
    }

    public GameObject Arrow
    {
        get { return arrow; }
    }
    public GameObject Player
    {
        get { return player; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
        StartCoroutine(powerUpSpawn());
        currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;
    }

    public void PlayerHit(int currentHP)
    {
        if (currentHP > 0)
        {
            gameOver = false;
        } else
        {
            gameOver = true;
        }
    }

    IEnumerator spawn()
    {
        if (currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;
            if (enemies.Count < currentLevel)
            {
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                int randomEnemy = Random.Range(0, 3);
                if (randomEnemy == 0)
                {
                    newEnemy = Instantiate(soldier) as GameObject;
                } else if ( randomEnemy == 1) {
                    newEnemy = Instantiate(ranger) as GameObject;
                } else if ( randomEnemy == 2)
                {
                    newEnemy = Instantiate(tanker) as GameObject;
                }
                newEnemy.transform.position = spawnLocation.transform.position;
            }
        }
        if (killedEnemies.Count == currentLevel)
        {
            enemies.Clear();
            killedEnemies.Clear();

            yield return new WaitForSeconds(3);
            currentLevel++;
            levelText.text = "Level " + currentLevel;
        }
        yield return null;
        StartCoroutine(spawn());
    }

    IEnumerator powerUpSpawn()
    {
        if (currentPowerUpSpawnTime > powerUpSpawnTime)
        {
            currentPowerUpSpawnTime = 0;
            if (powerUps < maxPowerUps)
            {
                int randomNumber = Random.Range(0, powerUpSpawns.Length - 1);
                GameObject spawnLocation = powerUpSpawns[randomNumber];
                int randomPowerUp = Random.Range(0, 2);
                if (randomPowerUp == 0)
                {
                    newPowerUp = Instantiate(healthPowerUp) as GameObject;
                } else if (randomPowerUp == 1)
                {
                    newPowerUp = Instantiate(speedPowerUp) as GameObject;
                }

                newPowerUp.transform.position = spawnLocation.transform.position;
            }
        }
        yield return null;
        StartCoroutine(powerUpSpawn());
    }
}
