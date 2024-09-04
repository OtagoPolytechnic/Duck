using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject player;
    public EnemyBase originEnemy;
    private Rigidbody2D rb;
    public float bulletSpeed = 10;
    private float range = 20f;
    private Vector3 startPos;
    private int damage;
    private Vector2 heldVelocity;
    public int Damage
    {
        set {damage = value;}
    }
    
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        heldVelocity = rb.velocity;
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
        //destroys bullet after range
        float distTravelled = Vector3.Distance(startPos, transform.position);

        if (distTravelled > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerStats>().ReceiveDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
