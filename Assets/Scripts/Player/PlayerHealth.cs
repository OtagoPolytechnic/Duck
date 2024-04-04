using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;

    [HideInInspector]
    public float currentHealth;

    public int lives;

    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            if (lives > 1)
            {
                Respawn();
            }
            else
            {
                FindObjectOfType<GameManager>().GameOver();
            }
        }
    }

    void Respawn()
    {
        Debug.Log("respawned");
        lives -= 1;
        gameObject.transform.position = new Vector3(0,0,0); //Will use position of nest gameObject once it is added
        currentHealth = maxHealth;
    }
}
