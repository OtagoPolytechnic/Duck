using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnRadius;
    public float timeBetweenSpawn;
    public float spawnTime;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime)
        {
            int enemyNum = Random.Range(0, enemies.Length);
            Spawn(enemyNum);
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void Spawn(int enemyNum)
    {
        float randX = Random.Range(-spawnRadius, spawnRadius);
        float randY = Random.Range(-spawnRadius, spawnRadius);
        Instantiate(enemies[enemyNum], transform.position + new Vector3(randX, randY, 0), transform.rotation);
    }
}
