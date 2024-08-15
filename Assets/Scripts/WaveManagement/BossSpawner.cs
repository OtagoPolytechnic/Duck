using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject enemyBoss1Prefab;
    public GameObject enemyBoss2Prefab;
    public GameObject enemyBoss3Prefab;
    public GameObject enemyBoss4Prefab;
    public GameObject enemyBoss5Prefab;
    public float spawnRadius;
    public static float healthMultiplier = 1f;
    public static float spawnTimer = 5f;
    private float lastSpawn;
    private int enemyCap = 1000;
    public static List<GameObject> currentEnemies = new List<GameObject>();
    public int currentWaveNumber;

    // Array to track if a boss has been spawned for a given wave
    private bool[] bossesSpawned = new bool[5];

    void Awake()
    {
        lastSpawn = spawnTimer;
        // Initialize the bossesSpawned array to false
        for (int i = 0; i < bossesSpawned.Length; i++)
        {
            bossesSpawned[i] = false;
        }
    }

    void Update()
    {
        currentWaveNumber = GameSettings.waveNumber;
        waveNumberCheck();
    }

    public void waveNumberCheck()
    {
        // Determine which boss should be spawned based on the wave number
        int bossIndex = (currentWaveNumber / 5) - 1;

        // Check if the bossIndex is within bounds and if that boss has already been spawned
        if (bossIndex >= 0 && bossIndex < bossesSpawned.Length && !bossesSpawned[bossIndex])
        {
            switch (bossIndex)
            {
                case 0:
                    SpawnBossOne();
                    break;
                case 1:
                    SpawnBossTwo();
                    break;
                case 2:
                    SpawnBossThree();
                    break;
                case 3:
                    SpawnBossFour();
                    break;
                case 4:
                    SpawnBossFive();
                    break;
            }
            // Mark the boss as spawned
            bossesSpawned[bossIndex] = true;
        }
    }

    void SpawnBossOne()
    {
        if (enemyBoss1Prefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyBoss1Prefab, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss1 called. Boss spawned at: " + spawnPosition);
            bossInstance.GetComponent<EnemyHealth>().health =bossInstance.GetComponent<EnemyHealth>().baseHealth;
         
        }
        else
        {
            Debug.LogError("enemyBoss1Prefab is not assigned!");
        }
    }

    void SpawnBossTwo()
    {
        if (enemyBoss2Prefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyBoss2Prefab, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss2 called. Boss spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogError("enemyBoss2Prefab is not assigned!");
        }
    }

    void SpawnBossThree()
    {
        if (enemyBoss3Prefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyBoss3Prefab, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss3 called. Boss spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogError("enemyBoss3Prefab is not assigned!");
        }
    }

    void SpawnBossFour()
    {
        if (enemyBoss4Prefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyBoss4Prefab, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss4 called. Boss spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogError("enemyBoss4Prefab is not assigned!");
        }
    }

    void SpawnBossFive()
    {
        if (enemyBoss5Prefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyBoss5Prefab, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss5 called. Boss spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogError("enemyBoss5Prefab is not assigned!");
        }
    }
}
