using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float bulletSpeed;
    private float range;
    private Vector3 startPos;

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
        if (SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Decoy");

        }
        else if (!SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }

        // Set bullet damage based on current wave
        bulletDamage = 30 + (GameSettings.waveNumber / 5) * 5;
        bulletSpeed = 5 + (GameSettings.waveNumber / 5) * 3;
        range = 20f + (GameSettings.waveNumber / 5) * 20;

        // Calculate direction based on whether it's a shotgun bullet
        Vector3 direction;
        if (isShotgunBullet)
        {
            direction = Quaternion.Euler(0, 0, angleOffset) * (player.transform.position - transform.position);
        }
        else
        {
            direction = player.transform.position - transform.position;
        }

        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
    }

    public void InitializeBullet(GameObject player, int damage, bool isShotgun, float angleOffset = 0f)
    {
        this.player = player;
        this.bulletDamage = damage;
        this.isShotgunBullet = isShotgun;
        this.angleOffset = angleOffset;
    }

    void Update()
    {
        // Destroys bullet after range
        float distTravelled = Vector3.Distance(startPos, transform.position);
        if (distTravelled > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            if (SkillEffects.Instance.decoyActive)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerStats>().ReceiveDamage(bulletDamage);
                player = GameObject.FindGameObjectWithTag("Decoy");
            }
            else
            {
                player.GetComponent<PlayerStats>().ReceiveDamage(bulletDamage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges") || other.gameObject.CompareTag("Decoy"))
        {
            Destroy(gameObject);
        }
    }
}
