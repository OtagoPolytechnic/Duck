using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float spawnRadius;
    public float spawnTimer; //Time between spawns
    public float lastSpawn; //Time since last spawn

    public int waveNumber;

    public GameObject timerManager;
    Timer timer;

    // Update is called once per frame
    void Update()
    {
        //This wasn't working when I tried to decleare the timer in start for some reason, so I am assigning it on the first update as a workaround
        if(timer == null){
            timer = timerManager.GetComponent<Timer>();
        }

        if((waveNumber != timer.waveNumber) && (spawnTimer > 0.2)){
            waveNumber = timer.waveNumber;
                spawnTimer -= 0.1f;
        }

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
        float randX = Random.Range(-spawnRadius, spawnRadius);
        float randY = Random.Range(-spawnRadius, spawnRadius);
        Instantiate(enemies[enemyNum], transform.position + new Vector3(randX, randY, 0), transform.rotation);
    }
}
