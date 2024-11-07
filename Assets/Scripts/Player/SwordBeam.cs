using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordBeam : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float range;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Destroys bullet after it reaches max range
        if (Vector3.Distance(startPos, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.GetComponent<RiotShield>().TakeDamage();
            Destroy(gameObject);
        }
        //destroys bullet on hit with player and lowers health
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyBase>())
            {
                other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(Mathf.RoundToInt(WeaponStats.Instance.Damage * WeaponStats.Instance.SwordBeamMultiplier), false); //Deals half non-crit damage
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}