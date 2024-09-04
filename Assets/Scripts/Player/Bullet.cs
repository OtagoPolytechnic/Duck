using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private Vector3 startPos;
    private bool crit = false;

    void Start()
    {
        startPos = transform.position;

        //Calculates critical roll
        if (UnityEngine.Random.Range(0, 100) < WeaponStats.Instance.CritChance)
        {
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
                PlayerStats.Instance.CurrentHealth += Math.Max((WeaponStats.Instance.Damage * PlayerStats.Instance.LifestealPercentage) / 100, 1); //Heals at least 1 health
            }
            //TODO: Explosive bullet fix
            // if (WeaponStats.Instance.ExplosiveBullets)
            // {
            //     transform.localScale = new Vector3(transform.localScale.x * (2 + 0.2f * WeaponStats.Instance.ExplosionSize), transform.localScale.y * (2 + 0.2f * WeaponStats.Instance.ExplosionSize), 1);
            // }
            if (crit)
            {
                if (other.gameObject.GetComponent<EnemyBase>()) //Workaround for bosses not inheriting from this yet
                {
                    other.gameObject.GetComponent<EnemyBase>().ReceiveDamage((WeaponStats.Instance.Damage * WeaponStats.Instance.CritDamage) / 100, true);
                }
                else
                {
                    other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage((WeaponStats.Instance.Damage * WeaponStats.Instance.CritDamage) / 100, true);
                }
            }
            else
            {
                if (other.gameObject.GetComponent<EnemyBase>()) //Workaround for bosses not inheriting from this yet
                {
                    other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.Damage, false);
                }
                else
                {
                    other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage(WeaponStats.Instance.Damage, false);
                }
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
