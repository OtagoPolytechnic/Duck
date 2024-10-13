using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShield : MonoBehaviour
{
 
    void Start()
    {
   
        StartCoroutine(DestroyShieldAfterRandomTime());
    }

    private IEnumerator DestroyShieldAfterRandomTime()
    {
     
        float randomTime = Random.Range(5f, 15f);
     
        yield return new WaitForSeconds(randomTime);
    
        Destroy(gameObject);
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        Debug.Log($"Collided with: {collision.gameObject.name}");

     
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit the shield!");

          
            Destroy(collision.gameObject);

         
            Debug.Log($"{collision.gameObject.name} was destroyed.");
        }
    }
}
