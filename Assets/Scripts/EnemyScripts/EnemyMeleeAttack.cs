using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private int damage; 
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerStats>().ReceiveDamage(damage);
            gameObject.GetComponent<BoxCollider2D>().enabled = false; //Disable collider after dealing damage, so that each attack can only damage once
        }
    }
}
