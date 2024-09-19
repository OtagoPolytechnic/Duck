using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

//This script will be deleted once bosses are changed to inherit from enemyBase. This script should not be updated.
public class EnemyHealth : MonoBehaviour
{
    public const float BLEED_INTERVAL = 1f;
    public GameObject damageText;
    public GameObject critText;
    [SerializeField] private int health;
    public int Health
    {
        get {return health;}
        set {health = value;}
    }
    public float bleedTick = 1f;
    public float bleedInterval = 1f;
    public bool bleeding;
    public static int bleedAmount = 0;
    [SerializeField] private int points;


    void Update()
    {

        if (health <= 0)
        {
            SFXManager.Instance.PlaySFX("EnemyDie");
            ScoreManager.Instance.IncreasePoints(points);
            EnemySpawner.Instance.currentEnemies.Remove(gameObject);
            Destroy(gameObject);
        }
        if (GameSettings.gameState != GameState.InGame) { return; }
        Bleed();
    }
    void Bleed()
    {
        if (!bleeding || WeaponStats.Instance.BleedDamage == 0) { return; } //If the enemy is not bleeding, return. This means there is a 1 second interval before the first bleed tick
        bleedTick -= Time.deltaTime;
        if (bleedTick <= 0)
        {
            bleedTick = BLEED_INTERVAL;
            ReceiveDamage(Math.Max((Health * WeaponStats.Instance.BleedDamage) / 100, 1), false);
        }
    }
    public void ReceiveDamage(int damageTaken, bool critTrue)
    {
        //Add a small random offset to the damage text number position so they don't all stack on top of each other
        float randomOffset = UnityEngine.Random.Range(-0.3f, 0.3f);
        if (!bleeding && WeaponStats.Instance.BleedDamage > 0)
        {
            bleeding = true;
        }
        if (critTrue)
        {
            GameObject critTextInst = Instantiate(critText, new Vector3(transform.position.x + randomOffset, transform.position.y + 1 + randomOffset, transform.position.z), Quaternion.identity);
            critTextInst.GetComponent<TextMeshPro>().text = damageTaken.ToString() + "!";
            health -= damageTaken;
        }
        else
        {
            GameObject damageTextInst = Instantiate(damageText, new Vector3(transform.position.x + randomOffset, transform.position.y + 1 + randomOffset, transform.position.z), Quaternion.identity);
            damageTextInst.GetComponent<TextMeshPro>().text = damageTaken.ToString();
            health -= damageTaken;
        }

    }
}