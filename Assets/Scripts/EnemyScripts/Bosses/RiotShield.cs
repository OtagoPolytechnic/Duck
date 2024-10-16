using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShield : MonoBehaviour
{
    private int shieldHealth = 5;

    public void TakeDamage()
    {
        shieldHealth--;
        if (shieldHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
