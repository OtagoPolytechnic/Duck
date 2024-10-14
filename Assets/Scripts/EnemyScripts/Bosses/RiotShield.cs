using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShield : MonoBehaviour
{
    public int shieldHealth=5;
    public int maxShieldHealth = 5; 


    private void Update()
    {
        if (shieldHealth <= 0)
        { 
            Die(); 
        }
    }
    void Awake()
    {
        shieldHealth = maxShieldHealth;
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
