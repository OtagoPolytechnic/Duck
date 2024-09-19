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
    void Update()
    {
        if (GameSettings.gameState != GameState.InGame){return;}

            if (attackCooldown <= 0)
            {
                Shoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    
    void Shoot()
    {
        // Define the radius around the enemy within which bombs will be instantiated
        float bulletSpeed = Random.Range(3f, 7f); // Adjust the speed range as needed
        float spawnRadius = 1f; // Adjust this radius as needed
        // Generate a random position within the radius
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPosition = (Vector2)transform.position + randomPosition;

       
        GameObject bulletInstance = Instantiate(bullet, spawnPosition, Quaternion.identity);

        // Apply a random direction to the bullet's velocity
      
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        // Generate a random direction
        float randomAngle = Random.Range(0f, 360f);
        Vector2 shootDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        bulletRb.velocity = shootDirection * bulletSpeed;
        attackCooldown = attackInterval;
    }
}