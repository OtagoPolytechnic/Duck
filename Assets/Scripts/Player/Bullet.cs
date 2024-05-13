using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timer;
    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //destroys bullet after time elapsed
        timer += Time.deltaTime;

        if (timer >10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerHealth.currentHealth += PlayerHealth.lifestealAmount;
            if (PlayerHealth.currentHealth > PlayerHealth.maxHealth)
            {
                PlayerHealth.currentHealth = PlayerHealth.maxHealth;
            }
            if (PlayerHealth.explosiveBullets)
            {
                 transform.localScale = new Vector3(transform.localScale.x * (2 + 0.2f * PlayerHealth.explosionAmount), transform.localScale.y * (2 + 0.2f * PlayerHealth.explosionAmount), 1);
            }
            other.gameObject.GetComponent<EnemyHealth>().health -= PlayerHealth.damage;
            Destroy(gameObject);
        }
    }
}
