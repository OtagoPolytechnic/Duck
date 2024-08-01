using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnRadius;
    public static float healthMultiplier = 1f;
    public static float spawnTimer = 5f; //Time between spawns
    private float lastSpawn; //Time since last spawn
    private int enemyCap = 1000; //temp value to stop lag
    public static List<GameObject> currentEnemies = new List<GameObject>();
    public int waveNumber;

    void Awake()
    {
        lastSpawn = spawnTimer;
    }

    void Update()
    {
        if(GameSettings.gameState == GameState.InGame)
        {
            if (lastSpawn > spawnTimer && currentEnemies.Count < enemyCap)
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
        currentEnemies.Add(enemy);
    }
}
