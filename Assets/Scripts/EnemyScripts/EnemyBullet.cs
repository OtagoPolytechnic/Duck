using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float bulletSpeed = 10;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>().ReceiveDamage(20);
            PlayerHealth.currentHealth -= 20;
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
