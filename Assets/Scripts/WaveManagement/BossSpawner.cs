using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public float spawnRadius;
    public static float healthMultiplier = 1f;
    public static float spawnTimer = 5f; // Time between spawns for regular enemies
    private float lastSpawn; // Time since last spawn
    private int enemyCap = 1000; // Temp value to stop lag
    public static List<GameObject> currentEnemies = new List<GameObject>();

    public GameObject[] bossPrefabs;
    public int[] bossSpawnWaves;

    private bool[] bossSpawnedForWave; // Array to track if a boss has been spawned for each wave

    private void Start()
    {
        // Initialize the bossSpawnedForWave array based on the number of waves
        bossSpawnedForWave = new bool[bossSpawnWaves.Length];
    }

    private void Update()
    {
        if (GameSettings.gameState == GameState.InGame)
        {
            HandleRegularEnemySpawning();

            int currentWaveNumber = GameSettings.waveNumber;

            // Check if it's time to spawn a boss based on the current wave number
            CheckAndSpawnBoss(currentWaveNumber);
        }
    }

    // Method to handle regular enemy spawning
    private void HandleRegularEnemySpawning()
    {
        if (lastSpawn > spawnTimer && currentEnemies.Count < enemyCap)
        {
            SpawnRandomEnemy();
            lastSpawn = 0;
        }
        else
        {
            lastSpawn += Time.deltaTime;
        }
    }

    // Method to spawn a random enemy
    private void SpawnRandomEnemy()
    {
        int enemyNum = Random.Range(0, bossPrefabs.Length);
        GameObject enemy = Instantiate(bossPrefabs[enemyNum], transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), transform.rotation);
        enemy.GetComponent<EnemyHealth>().health = Mathf.RoundToInt(enemy.GetComponent<EnemyHealth>().baseHealth * healthMultiplier);
        currentEnemies.Add(enemy);
    }

    // Method to check and spawn a boss based on the current wave number
    private void CheckAndSpawnBoss(int waveNumber)
    {
        for (int i = 0; i < bossSpawnWaves.Length; i++)
        {
            if (waveNumber == bossSpawnWaves[i] && !bossSpawnedForWave[i])
            {
                SpawnBoss(bossPrefabs[i]);
                bossSpawnedForWave[i] = true; // Mark that a boss has been spawned for this wave
                return; // Exit after spawning the boss
            }
        }
    }

    // Method to spawn a boss
    private void SpawnBoss(GameObject bossPrefab)
    {
        Debug.Log($"Spawning boss: {bossPrefab.name}");
        GameObject boss = Instantiate(bossPrefab, transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0), transform.rotation);
        boss.GetComponent<EnemyHealth>().health = Mathf.RoundToInt(boss.GetComponent<EnemyHealth>().baseHealth * healthMultiplier);
        currentEnemies.Add(boss);
    }

    // Call this method when the wave changes
    public void OnWaveChange()
    {
        Debug.Log("Wave changed");
        // Reset the bossSpawnedForWave array to allow bosses to spawn in future waves
        for (int i = 0; i < bossSpawnedForWave.Length; i++)
        {
            bossSpawnedForWave[i] = false;
        }
    }
}
