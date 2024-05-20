using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnRadius;
    public static float healthMultiplier = 1f;
    public static float spawnTimer = 5; //Time between spawns
    private float lastSpawn; //Time since last spawn

    public int waveNumber;

    public GameObject timerManager;
    Timer timer;

    private void Awake()
    {
        timerManager = GameObject.Find("TimerManager");
        timer = timerManager.GetComponent<Timer>();
        lastSpawn = spawnTimer;
    }

    void Update()
    {
        if(timer.running)
        {
            if (lastSpawn > spawnTimer)
            {
                Spawn(Random.Range(0, enemies.Length));
                lastSpawn = 0;
            }
            else
            {
                lastSpawn += Time.deltaTime;
            }
        }
    }

    void Spawn(int enemyNum)
    {
        GameObject enemy = Instantiate(enemies[enemyNum], transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), transform.rotation);
        enemy.GetComponent<EnemyHealth>().health = Mathf.RoundToInt(enemy.GetComponent<EnemyHealth>().baseHealth * healthMultiplier);
    }
}
