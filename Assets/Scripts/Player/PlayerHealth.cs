using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;

    [HideInInspector]
    public float currentHealth;
    public List<GameObject> lifeEggs;
    public UnityEvent onPlayerRespawn = new UnityEvent();
    
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
        //This event currently has no listeners, it is here for futire use 
        onPlayerRespawn?.Invoke();

        if (lifeEggs.Count > 0) //This should never run if there are no eggs, but this is here just in case
        {
            gameObject.transform.position = lifeEggs[lifeEggs.Count -1].transform.position;
            Destroy(lifeEggs[lifeEggs.Count -1]);
            lifeEggs.Remove(lifeEggs[lifeEggs.Count -1]);
        }

        currentHealth = maxHealth;
    }
}