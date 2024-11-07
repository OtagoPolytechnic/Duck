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

    private int bulletDamage;
    public int BulletDamage
    {
        get { return bulletDamage; }
        set { bulletDamage = value; }
    }

    private bool isShotgunBullet; 
    private float angleOffset;
    private static float currentAngle = 0f;
    private const float angleIncrement = 40f; 

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        bulletDamage = 30 + (GameSettings.waveNumber / 5) * 5;
        bulletSpeed = 5 + (GameSettings.waveNumber / 5);
        range = 20f + (GameSettings.waveNumber / 5) * 20;

        Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

        rb.velocity = direction.normalized * bulletSpeed;
        heldVelocity = rb.velocity;
        currentAngle += angleIncrement;

        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }
    }

    public void InitializeBullet(GameObject player, int damage, bool isShotgun, float angleOffset = 0f)
    {
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
