using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private int damage; //Damage has been halved because of double damage bug

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().currentHealth -= damage; //can be simplified once player current health is made public
            gameObject.GetComponent<BoxCollider2D>().enabled = false; //Disable collider after dealing damage, so that each attack can only damage once
        }
    }
}
