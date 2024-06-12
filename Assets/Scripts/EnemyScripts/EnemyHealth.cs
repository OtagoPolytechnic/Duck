using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public GameObject damageText;
    public GameObject critText;
    public int baseHealth;
    [HideInInspector] public int health;
    public float bleedTick = 1f;
    public float bleedInterval = 1f;
    public static bool bleedTrue;
    public static int bleedAmount = 0;

    void Update()
    {
        Bleed();
        if (health <= 0)
        {
            ScoreManager.Instance.IncreasePoints(10);
            EnemySpawner.currentEnemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    void Bleed() //this function needs to be reworked to be able to stack bleed on the target
    {
        bleedTick -= Time.deltaTime;
        if (bleedTick <= 0 && bleedTrue)
        {
            bleedTick = bleedInterval;
            health -= bleedAmount; 
        }
    }
    public void ReceiveDamage(int damageTaken, bool critTrue)
    {
        if (critTrue)
        {
            GameObject critTextInst = Instantiate(critText, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            critTextInst.GetComponent<TextMeshPro>().text = damageTaken.ToString() + "!";
            health -= damageTaken;
        }
        else
        {
            GameObject damageTextInst = Instantiate(damageText, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            damageTextInst.GetComponent<TextMeshPro>().text = damageTaken.ToString();
            health -= damageTaken;
        }
        
    }
}
