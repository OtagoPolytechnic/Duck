using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerHealth playerHealth;
    void Start()
    {
        
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.lifeEggs.Add(gameObject);
    }

}
