using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShield : MonoBehaviour
{
    public int maxShieldHealth = 5;
    public int shieldHealth = 5;
    public static RiotShield Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        shieldHealth--;
        if (shieldHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
