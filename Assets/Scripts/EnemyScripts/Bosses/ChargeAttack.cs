using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    private int damage;
    public GameObject player;
    public EnemyBase originEnemy;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damage = gameObject.GetComponentInParent<EnemyBase>().Damage;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerStats>().ReceiveDamage(damage, originEnemy);
            gameObject.GetComponent<BoxCollider2D>().enabled = false; //Disable collider after dealing damage, so that each attack can only damage once
        }
    }
}
