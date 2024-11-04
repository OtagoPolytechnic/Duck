using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public EnemyBase originEnemy;
    public float bulletSpeed;
    private float range;
    private Vector3 startPos;
    private Vector2 heldVelocity;

    // Variable to hold the bullet damage
    private int bulletDamage;
    public int BulletDamage
    {
        get { return bulletDamage; }
        set { bulletDamage = value; }
    }

    private bool isShotgunBullet; // Determines if bullet is for shotgun boss
    private float angleOffset;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        // Set bullet damage and speed based on current wave
        bulletDamage = 30 + (GameSettings.waveNumber / 5) * 5;
        bulletSpeed = 5 + (GameSettings.waveNumber / 5);
        range = 20f + (GameSettings.waveNumber / 5) * 20;

        // Generate a random angle and calculate direction
        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // Set the bullet's velocity based on the random direction
        rb.velocity = direction.normalized * bulletSpeed;
        heldVelocity = rb.velocity;
    }

    public void InitializeBullet(GameObject player, int damage, bool isShotgun, float angleOffset = 0f)
    {
        // You can leave player as null since it's no longer needed
        this.bulletDamage = damage;
        this.isShotgunBullet = isShotgun;
        this.angleOffset = angleOffset;
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
        else if (GameSettings.gameState == GameState.InGame && rb.velocity == Vector2.zero)
        {
            rb.velocity = heldVelocity;
        }

        // Destroy bullet after exceeding range
        float distTravelled = Vector3.Distance(startPos, transform.position);
        if (distTravelled > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(bulletDamage, originEnemy);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges") || other.gameObject.CompareTag("Decoy"))
        {
            Destroy(gameObject);
        }
    }
}