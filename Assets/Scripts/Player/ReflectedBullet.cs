using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedBullet : MonoBehaviour
{
    private Vector3 startPos;
    private int damage;
    public int Damage
    {
        get {return damage;}
        set {damage = value;}
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //Destroys bullet after it reaches max range
        if (Vector3.Distance(startPos, transform.position) > 20f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(Damage, false);
            Destroy(gameObject);
        }
    }
}
