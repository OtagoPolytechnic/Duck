using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sword : MonoBehaviour
{
    private bool crit;
    public bool Crit
    {
        get {return crit;}
        set {crit = value;}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {            
            if (PlayerStats.Instance.LifestealPercentage > 0)
            {
                PlayerStats.Instance.CurrentHealth += Math.Max((WeaponStats.Instance.Damage * PlayerStats.Instance.LifestealPercentage) / 100, 1); //Heals at least 1 health
            }
            if (crit)
            {
                other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.CritDamage, true);
            }
            else
            {
                other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.Damage, false);
            }
        }
        else if (WeaponStats.Instance.HasReflector && other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
