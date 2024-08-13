using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 startPos;
    private int critDamage = 0;
    private bool critTrue = false;

    void Start()
    {
        startPos = transform.position;
        float critRoll = Random.Range(0f,1f);
        
        //crit damage calculation
        if (critRoll < WeaponStats.Instance.CritChance)
        {
            critDamage += Mathf.RoundToInt(WeaponStats.Instance.Damage * 1.50f);
            critTrue = true;
            //Change to critical sprite
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    
    void Update()
    {
        //destroys bullet after range
        float distTravelled = Vector3.Distance(startPos, transform.position);

        if (distTravelled > WeaponStats.Instance.Range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Enemy"))
        {
            //lifesteal addition and cap
            PlayerStats.Instance.CurrentHealth += PlayerStats.Instance.LifestealAmount;
            //explosive bullets size calculation
            if (WeaponStats.Instance.ExplosiveBullets)
            {
                transform.localScale = new Vector3(transform.localScale.x * (2 + 0.2f * WeaponStats.Instance.ExplosionSize), transform.localScale.y * (2 + 0.2f * WeaponStats.Instance.ExplosionSize), 1);
            }
            if (critTrue)
            {
                other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage(WeaponStats.Instance.Damage + critDamage, true);
            }
            else
            {
                other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage(WeaponStats.Instance.Damage, false);
            }
            Destroy(gameObject);
        }
    }
}
