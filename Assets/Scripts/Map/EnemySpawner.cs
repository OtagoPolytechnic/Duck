using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnRadius;
    public float timeBetweenSpawn;
    public float spawnTime;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime)
        {
            Spawn();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void Spawn()
    {
        float randX = Random.Range(-spawnRadius, spawnRadius);
        float randY = Random.Range(-spawnRadius, spawnRadius);
        Instantiate(enemy, transform.position + new Vector3(randX, randY, 0), transform.rotation);
    }
}
