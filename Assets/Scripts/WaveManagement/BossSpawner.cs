using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] bosses;
    public float spawnRadius;
    public static float healthMultiplier = 1f;
    public static float spawnTimer = 5f;
    private float lastSpawn;
    private int enemyCap = 1000;
    public static List<GameObject> currentEnemies = new List<GameObject>();
    public int currentWaveNumber;
    private VisualElement document;
    private VisualElement container;
    public int bossHealth=200;
    public int bossMaxHealth=200;
    public GameObject bossHealthBar;
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




   public void SpawnBoss()
    {
        GameObject enemyChoice = bosses[Random.Range(0,bosses.Length)];
        if (enemyChoice != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyChoice, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss1 called. Boss spawned at: " + spawnPosition);
            bossInstance.GetComponent<EnemyHealth>().health = bossHealth;
            document = bossHealthBar.GetComponent<UIDocument>().rootVisualElement;
            container = document.Q<VisualElement>("BossHealthContainer");
            container.visible = true;

            BossHealth.Instance.boss = bossInstance.GetComponent<EnemyHealth>();
            BossHealth.Instance.BossMaxHealth = bossHealth;
        }
        else
        {
            Debug.LogError("enemyBoss1Prefab is not assigned!");
        }
    }
}

   