using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusAttack : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bombPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    private float attackCooldown;

    private MapManager mapManager;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
    }

    void Start()
    {
        speed = 10 + (GameSettings.waveNumber / 5) * 5; // Increase speed by 5 for every 5 levels
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame) { return; }
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        //turns enemy towards player
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);

        if (distance >= attackRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (speed * tileSpeedModifier) * Time.deltaTime);
        }
        else
        {
            if (attackCooldown <= 0)
            {
                Shoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }
    void Shoot()
    {
        // Define the radius around the enemy within which bombs will be instantiated
        float spawnRadius = 10f; // Adjust this radius as needed

        // Generate a random position within the radius
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPosition = (Vector2)transform.position + randomPosition;

        // Create a new bullet (bomb) instance at the random position
        GameObject bulletInstance = Instantiate(bullet, spawnPosition, Quaternion.identity);

        // Apply a random direction to the bullet's velocity
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // Generate a random direction
            float randomAngle = Random.Range(0f, 360f);
            Vector2 shootDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

            float bulletSpeed = 20f; // Set your desired bullet speed here
            bulletRb.velocity = shootDirection * bulletSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D component missing on bullet prefab.");
        }

        attackCooldown = attackInterval;

        // Play the enemy shooting sound
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.EnemyShootSound();
        }
        else
        {
            Debug.LogError("SFXManager instance is null in RadiusAttack.Shoot().");
        }

        // Debug information
        Debug.Log($"Bomb instantiated at: {spawnPosition}");
    }
}