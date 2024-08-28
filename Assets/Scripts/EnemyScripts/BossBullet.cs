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

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Set bullet damage based on current wave
        bulletDamage = 30 + (GameSettings.waveNumber / 5) * 5; // Increase damage by 5 for every 5 levels
        bulletSpeed = 5+(GameSettings.waveNumber / 5) * 5; // Increase speed by 5 for every 5 levels
        range = 20f + (GameSettings.waveNumber / 5) * 20; // Increase range of enemy bullets by 20 for every 5 levels
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
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
        // Destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerStats>().ReceiveDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
