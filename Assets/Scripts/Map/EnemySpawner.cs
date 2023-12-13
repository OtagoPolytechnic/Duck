using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnRadius;
    public float spawnTimer; //Time between spawns
    public float lastSpawn; //Time since last spawn

    // Update is called once per frame
    void Update()
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

    void Spawn(int enemyNum)
    {
        float randX = Random.Range(-spawnRadius, spawnRadius);
        float randY = Random.Range(-spawnRadius, spawnRadius);
        Instantiate(enemies[enemyNum], transform.position + new Vector3(randX, randY, 0), transform.rotation);
    }
}
