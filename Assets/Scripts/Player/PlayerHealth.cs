using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;

    [HideInInspector]
    public float currentHealth;
    public List<GameObject> lifeEggs;
    
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
            if (lifeEggs.Count > 0)
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

        if (lifeEggs.Count > 0) //This should never run if there are no eggs, but this is here just in case
        {
            Destroy(lifeEggs[lifeEggs.Count -1]);
            lifeEggs.Remove(lifeEggs[lifeEggs.Count -1]);
        }

        gameObject.transform.position = new Vector3(0,0,0); //use position of egg
        currentHealth = maxHealth;
    }
}