using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossShield : MonoBehaviour
{
  
    public float pushDistance = 2f;

    void Start()
    {
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;

            other.transform.position += (Vector3)direction * pushDistance;
        }
    }

    public void TakeDamage()
    {
      
    }

    void Update()
    {
       
    }
}
