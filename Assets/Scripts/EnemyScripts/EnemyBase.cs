using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    private Sprite graveSprite;
    public const float BLEED_INTERVAL = 1f;
    public GameObject damageText;
    public GameObject critText;
    private int maxHealth; 
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
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
    private bool dying;
    public bool Dying
    {
        get {return dying;}
        set {dying = value;}
    }
    public MapManager mapManager;
    public float bleedTick = 1f;
    public float bleedInterval = 1f;
    public bool bleeding;
    public static int bleedAmount = 0;
    public static float endlessScalar = 1f;
    public const float DEATHTIMEOUT = 0.5f;
    public const float BOSSDEATHTIMEOUT = 2f;
    public const float FINALBOSSDEATHTIMEOUT = 3f;
    protected bool isImmune = false;
    private bool isBoss = false;
    public bool IsBoss
    {
        get {return isBoss;}
        set {isBoss = value;}
    }
    void Start()
    {
        graveSprite = Resources.Load<Sprite>("Grave");
    }

    public void Bleed() //this function needs to be reworked to be able to stack bleed on the target
    {
        if (!bleeding || WeaponStats.Instance.BleedDamage == 0) { return; } //If the enemy is not bleeding, return. This means there is a 1 second interval before the first bleed tick
        bleedTick -= Time.deltaTime;
        if (bleedTick <= 0)
        {
            bleedTick = BLEED_INTERVAL;
            ReceiveDamage(Math.Max((maxHealth * WeaponStats.Instance.BleedDamage) / 100, 1), false);
        }
    }
    public void ReceiveDamage(int damageTaken, bool critTrue)
    {
        if (isImmune)
        {
            return;
        }
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
        //Lifesteal by percentage of damage dealt
        if (PlayerStats.Instance.LifestealPercentage > 0 && damageTaken > 5 && GameSettings.gameState != GameState.Dead)
        {
            PlayerStats.Instance.CurrentHealth += Math.Max((damageTaken * PlayerStats.Instance.LifestealPercentage) / 100, 1); //Heals at least 1 health
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
            Dying = true;
        }
        
    }
    public abstract void Move();
    public virtual IEnumerator Die()
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        GetComponent<Collider2D>().enabled = false;

        SFXManager.Instance.PlaySFX("EnemyDie");
        ScoreManager.Instance.IncreasePoints(Points);
        
        if (sprite != null)
        {
            sprite.enabled = true;
            sprite.color = new Color32(255, 255, 255, 225);
            sprite.transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(1,1,0);
            if (isBoss)
            {
                sprite.transform.localScale = new Vector3(5f,5f,0);
            }
            else
            {
                sprite.transform.localScale = new Vector3(1.5f,1.5f,0);
            }
            sprite.sprite = graveSprite;

            for (int i=0; i < sprite.gameObject.transform.childCount; i++) //Destroy all children of sprite to prevent things such as childed attacks from appearing beside tombstone.
            {
                Destroy(sprite.gameObject.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            Debug.LogError("Did not get sprite, make sure enemy hierarchy has a sprite render");
        }

        if (isBoss)
        {
            yield return new WaitForSeconds(BOSSDEATHTIMEOUT);
        }
        else if(GameSettings.waveNumber % 25 == 0 && isBoss)
        {
            yield return new WaitForSeconds(FINALBOSSDEATHTIMEOUT);
        }
        else
        {
            yield return new WaitForSeconds(DEATHTIMEOUT);
        }

        EnemySpawner.Instance.currentEnemies.Remove(gameObject);
        Destroy(gameObject);
    }
    public void ScaleStats()
    {
        baseHealth = (int)Math.Round(BaseHealth * endlessScalar);
        maxHealth = baseHealth;
        damage = (int)Math.Round(Damage * endlessScalar);
        points = (int)Math.Round(Points * endlessScalar);
    }
}
