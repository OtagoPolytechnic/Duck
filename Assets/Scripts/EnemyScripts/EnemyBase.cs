using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    public const float BLEED_INTERVAL = 1f;
    public GameObject damageText;
    public GameObject critText;
    [SerializeField] private int baseHealth;
    public int BaseHealth
    {
        get { return baseHealth;}
    }
    private int health;
    public int Health
    {
        get {return health;}
        set {health = value;}
    }
    [SerializeField] private int damage;
    public int Damage
    {
        get {return damage;}
    }
    [SerializeField] private float speed;
    public float Speed
    {
        get {return speed;}
    }
    [SerializeField] private int points;
    public int Points
    {
        get {return points;}
    }
    public MapManager mapManager;
    public float bleedTick = 1f;
    public float bleedInterval = 1f;
    public bool bleedTrue;
    public static int bleedAmount = 0;
    public static float endlessScalar = 1f;

    public void Bleed() //this function needs to be reworked to be able to stack bleed on the target
    {
        if (!bleedTrue || WeaponStats.Instance.BleedDamage == 0){return;} //If the enemy is not bleeding, return. This means there is a 1 second interval before the first bleed tick
        bleedTick -= Time.fixedDeltaTime;
        if (bleedTick <= 0)
        {
            bleedTick = BLEED_INTERVAL;
            ReceiveDamage((Health * WeaponStats.Instance.BleedDamage) / 100, false);
        }
    }
    public void ReceiveDamage(int damageTaken, bool critTrue)
    {
        if (!bleedTrue)
        {
            bleedTrue = true;
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
    public abstract void Move();
    public abstract void Die();
    public void ScaleStats()
    {
        baseHealth = (int)Math.Round(BaseHealth * endlessScalar);
        damage = (int)Math.Round(Damage * endlessScalar);
        points = (int)Math.Round(Points * endlessScalar);
    }
}
