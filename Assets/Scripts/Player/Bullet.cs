using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 startPos;
    private bool crit = false;

    void Start()
    {
        startPos = transform.position;

        //Calculates critical roll
        if (Random.Range(0, 100) < WeaponStats.Instance.CritChance)
        {
            critDamage += Mathf.RoundToInt(WeaponStats.Instance.Damage * 1.50f);
            crit = true;
            //Change to critical sprite
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    
    void Update()
    {
        //Destroys bullet after it reaches max range
        if (Vector3.Distance(startPos, transform.position) > (float)WeaponStats.Instance.Range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Lifesteal by percentage of damage dealt
            if (PlayerStats.Instance.LifestealPercentage > 0)
            {
                PlayerStats.Instance.CurrentHealth += WeaponStats.Instance.Damage * PlayerStats.Instance.LifestealPercentage;
            }
            //TODO: Explosive bullet fix
            // if (WeaponStats.Instance.ExplosiveBullets)
            // {
            //     transform.localScale = new Vector3(transform.localScale.x * (2 + 0.2f * WeaponStats.Instance.ExplosionSize), transform.localScale.y * (2 + 0.2f * WeaponStats.Instance.ExplosionSize), 1);
            // }
            if (crit)
            {
                other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage((WeaponStats.Instance.Damage * CritDamage) / 100, true);
            }
            else
            {
                other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage(WeaponStats.Instance.Damage, false);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
