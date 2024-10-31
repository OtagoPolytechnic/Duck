using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject player;
    private int explosionDamage;
    public int ExplosionDamage
    {
        set { explosionDamage = value; }
    }
    private int explosionSize;
    public int ExplosionSize
    {
        set
        {
            explosionSize = value;
            transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
        }
    }
     
    void Start()
    {
        SFXManager.Instance.PlaySFX("Explosion");
        ExplosionSize = 6;
 
        StartCoroutine(DestroyExplosion());
    }
    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           
           DestroyExplosion();
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(10);
          
           
        }
    }
}