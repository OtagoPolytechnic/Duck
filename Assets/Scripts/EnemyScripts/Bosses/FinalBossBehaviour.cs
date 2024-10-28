using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;

    private float attackCooldown;

    [SerializeField] private List<GameObject> enemyPrefabs; 
    [SerializeField] private int enemiesToSpawn = 12; 
    [SerializeField] private float spawnRadius = 10f; 
    [SerializeField] private float minSpawnDistance = 10f; 
    [SerializeField] private float respawnDelay = 5f;

    private bool enemiesSpawnedAt75;
    private bool enemiesSpawnedAt50;
    private bool enemiesSpawnedAt25;

    [SerializeField] private GameObject bossShieldPrefab;
    private GameObject currentShield; 

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = attackInterval;

        enemiesSpawnedAt75 = false;
        enemiesSpawnedAt50 = false;
        enemiesSpawnedAt25 = false;
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame || Dying) { return; }

        if (SkillEffects.Instance.decoyActive && !stopCheck)
        {
            player = GameObject.FindGameObjectWithTag("Decoy");
            stopCheck = true;
        }
        else if (!SkillEffects.Instance.decoyActive && stopCheck)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            stopCheck = false;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (SkillEffects.Instance.vanishActive) { return; }
        else
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);
        }

     
        if (Health <= MaxHealth * 0.75f && !enemiesSpawnedAt75)
        {
            StartCoroutine(SpawnEnemies());
            enemiesSpawnedAt75 = true; 
        }
        else if (Health <= MaxHealth * 0.50f && !enemiesSpawnedAt50)
        {
            StartCoroutine(SpawnEnemies());
            enemiesSpawnedAt50 = true; 
        }
        else if (Health <= MaxHealth * 0.25f && !enemiesSpawnedAt25)
        {
            StartCoroutine(SpawnEnemies());
            enemiesSpawnedAt25 = true; 
        }

        if (distance >= attackRange)
        {
            Move();
        }
        else
        {
            if (attackCooldown <= 0)
            {
                if (SkillEffects.Instance.vanishActive) { return; }
                Shoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }

        Bleed();
    }

    public override void Move()
    {
        if (SkillEffects.Instance.vanishActive) { return; }
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (Speed * tileSpeedModifier) * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, false); // Pass false for shotgun
        newBullet.GetComponent<BossBullet>().originEnemy = this;
        SFXManager.Instance.PlaySFX("EnemyShoot");
        attackCooldown = attackInterval;
    }

    private IEnumerator SpawnEnemies()
    {
        SpawnEnemiesAroundBoss();

   
        yield return new WaitUntil(() => CountActiveEnemies() <= 1);
        isImmune = false;

        if (currentShield != null)
        {
            Destroy(currentShield);
            currentShield = null; 
        }
    }

    private void SpawnEnemiesAroundBoss()
    {
        isImmune = true;
        // Instantiate the BossShield at the boss's position
        currentShield = Instantiate(bossShieldPrefab, transform.position, Quaternion.identity);
        currentShield.transform.parent = this.transform;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            Vector2 spawnPosition;

   
            do
            {
                spawnPosition = GetRandomSpawnPosition();
            } while (Vector2.Distance(spawnPosition, player.transform.position) < minSpawnDistance);

            Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f);
        float radian = angle * Mathf.Deg2Rad;

        float x = transform.position.x + Mathf.Cos(radian) * spawnRadius;
        float y = transform.position.y + Mathf.Sin(radian) * spawnRadius;

        return new Vector2(x, y);
    }

    private int CountActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length; 
    }
}
