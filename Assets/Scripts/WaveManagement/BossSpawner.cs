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
    public static List<GameObject> currentEnemies = new List<GameObject>();
    public int currentWaveNumber;
    private VisualElement document;
    private VisualElement container;
    public int bossHealth=2000;
    public int bossMaxHealth=2000;
    public GameObject bossHealthBar;
    public GameObject bigBoss;

    void Awake()
    { 
        lastSpawn = spawnTimer;
   
    }

   public void SpawnBoss()
    {
        GameObject enemyChoice;
        if (GameSettings.waveNumber %25==0)
        {
            enemyChoice = bigBoss;
        }
        else
        {
            enemyChoice = bosses[Random.Range(0, bosses.Length)];
        }
        if (enemyChoice != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyChoice, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss called. Boss spawned at: " + spawnPosition);
            bossInstance.GetComponent<EnemyHealth>().health = bossHealth;
            document = bossHealthBar.GetComponent<UIDocument>().rootVisualElement;
            container = document.Q<VisualElement>("BossHealthContainer");
            container.visible = true;
            BossHealth.Instance.boss = bossInstance.GetComponent<EnemyHealth>();
            BossHealth.Instance.BossMaxHealth = bossHealth;
        }
        else
        {
            Debug.LogError("enemyBossPrefab is not assigned!");
        }
        bossHealth +=2000;
    }
}

   