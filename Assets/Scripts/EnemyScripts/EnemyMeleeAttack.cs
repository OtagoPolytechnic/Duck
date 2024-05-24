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
            PlayerHealth.currentHealth -= damage; 
            gameObject.GetComponent<BoxCollider2D>().enabled = false; //Disable collider after dealing damage, so that each attack can only damage once
        }
    }
}
