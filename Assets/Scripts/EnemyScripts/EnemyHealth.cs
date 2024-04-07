using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    //public UnityEvent OnEnemyDeath = new UnityEvent();
    public int health;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //OnEnemyDeath?.Invoke();
            ScoreManager.Instance.IncreasePoints(10);
            Destroy(gameObject);
        }
    }
    
}
