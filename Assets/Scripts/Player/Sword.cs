using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (crit)
            {
                other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.CritDamage, true);
            }
            else
            {
                other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.Damage, false);
            }
        }
    }
}
