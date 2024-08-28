using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public const float BLEED_INTERVAL = 1f;
    public GameObject damageText;
    public GameObject critText;
    public int baseHealth;
    [HideInInspector] public int health;
    public float bleedTick = 1f;
    public bool bleeding = false;


    void Update()
    {
       
        if (health <= 0)
        {
            SFXManager.Instance.EnemyDieSound();
            ScoreManager.Instance.IncreasePoints(10);
            EnemySpawner.currentEnemies.Remove(gameObject);
            Destroy(gameObject);
        } 
        if (GameSettings.gameState != GameState.InGame){return;}
        Bleed();
    }
    void Bleed()
    {
        if (!bleeding || WeaponStats.Instance.BleedDamage == 0){return;} //If the enemy is not bleeding, return. This means there is a 1 second interval before the first bleed tick
        bleedTick -= Time.deltaTime;
        if (bleedTick <= 0)
        {
            bleedTick = bleedInterval;
            ReceiveDamage((baseHealth * WeaponStats.Instance.BleedDamage) / 100, false);
        }
    }
    public void ReceiveDamage(int damageTaken, bool critTrue)
    {
        if (!bleeding) //Always applies bleeding. It just does no damage if the weapon doesn't have bleed damage
        {
            bleeding = true;
        }
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
