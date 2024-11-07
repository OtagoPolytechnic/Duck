using System.Collections.Generic;
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
    private int bossHealth = 1500;
    public GameObject bossHealthBar;
    public GameObject bigBoss;

    // Reference to the Player object
    public GameObject player;

    // Cache for the TargetIndicator component

    public TargetIndicator targetIndicator;

    private List<GameObject> shuffledBosses;
    private int currentBossIndex = 0;

    void Awake()
    {
        lastSpawn = spawnTimer;
        shuffledBosses = new List<GameObject>(bosses);
        ShuffleBosses();

    }

    void ShuffleBosses()
    {
        // Fisher-Yates shuffle algorithm
        for (int i = shuffledBosses.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = shuffledBosses[i];
            shuffledBosses[i] = shuffledBosses[j];
            shuffledBosses[j] = temp;
        }
    }

    public void SpawnBoss()
    {
        GameObject enemyChoice;

        if (GameSettings.waveNumber % 25 == 0)
        {
            enemyChoice = bigBoss;
        }
        else
        {
            if (currentBossIndex >= shuffledBosses.Count)
            {
                // Shuffle the bosses again if all bosses have been spawned
                ShuffleBosses();
                currentBossIndex = 0;
            }

            enemyChoice = shuffledBosses[currentBossIndex];
            currentBossIndex++;
        }

        if (enemyChoice != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject bossInstance = Instantiate(enemyChoice, spawnPosition, spawnRotation);
            currentEnemies.Add(bossInstance);
            Debug.Log("SpawnBoss called. Boss spawned at: " + spawnPosition);
            bossInstance.GetComponent<EnemyBase>().Health = bossHealth;
            bossInstance.GetComponent<EnemyBase>().MaxHealth = bossHealth;
            Debug.Log(bossInstance.GetComponent<EnemyBase>().MaxHealth);
            bossInstance.GetComponent<EnemyBase>().IsBoss = true;

            // Setup boss health UI
            document = bossHealthBar.GetComponent<UIDocument>().rootVisualElement;
            container = document.Q<VisualElement>("BossHealthContainer");
            container.visible = true;
            BossHealthBar.Instance.boss = bossInstance.GetComponent<EnemyBase>();
            BossHealthBar.Instance.BossMaxHealth = bossHealth;

            // Activate TargetIndicator if assigned
            if (targetIndicator != null)
            {
                targetIndicator.ActivateIndicator(); // Activate the TargetIndicator
                targetIndicator.Target = bossInstance.transform; // Set the newly spawned boss as the target
            }
            else
            {
                Debug.LogWarning("TargetIndicator component is not found on the Player object.");
            }
        }
        else
        {
            Debug.LogError("enemyBossPrefab is not assigned!");
        }
        bossHealth += 2500;
    }
}
