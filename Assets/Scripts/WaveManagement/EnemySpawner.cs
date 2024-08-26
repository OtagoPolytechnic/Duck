using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    [SerializeField] private GameObject[] enemies;
    private float spawnTimer = 1f; //Time between spawns
    private float lastSpawn; //Time since last spawn
    private int enemyCap = 100;
    public List<GameObject> currentEnemies = new List<GameObject>();
    //public int waveNumber;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        lastSpawn = spawnTimer;
    }

    void Update()
    {
       
        if(GameSettings.gameState == GameState.InGame && GameSettings.waveNumber%5!=0)
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
        Vector3 location = new Vector3();
        int side = Random.Range(1,5); //Which side of the screen the enemy spawns at
        Debug.Log(side);
        switch (side)
        {
            case 1:  
                location = Camera.main.ViewportToWorldPoint(new Vector3(1,Random.Range(0f,1f),0));
            break;
            case 2:
                location = Camera.main.ViewportToWorldPoint(new Vector3(0,Random.Range(0f,1f),0));
            break;
            case 3:
                location = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f,1f),1,0));
            break;
            case 4:
                location = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f,1f),0,0));
            break;
        }

        GameObject enemy = Instantiate(enemies[enemyNum], location, Quaternion.identity);
        currentEnemies.Add(enemy);
    }
}