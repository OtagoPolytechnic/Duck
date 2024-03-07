using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnRadius;
    public float spawnTimer; //Time between spawns
    public float lastSpawn; //Time since last spawn
    public int enemyHealth = 100;

    public int waveNumber;

    public GameObject timerManager;
    Timer timer;

    private void Awake()
    {
        timerManager = GameObject.Find("TimerManager");
        timer = timerManager.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.running)
        {
            if (lastSpawn > spawnTimer)
            {
                Spawn(Random.Range(0, enemies.Length));
                lastSpawn = 0;
            }
            else{
                lastSpawn += Time.deltaTime;
            }
        }
    }

    void Spawn(int enemyNum)
    {
        GameObject enemy = Instantiate(enemies[enemyNum], transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), transform.rotation);
        enemy.GetComponent<EnemyMovement>().health = enemyHealth;
    }
}
