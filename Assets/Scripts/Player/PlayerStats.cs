using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    //health vars
    private int maxHealth = 100;
    public float MaxHealth
    {
        get {return maxHealth;}
        set
        {
            //When health is increased
            if(value > maxHealth)
            {
                currentHealth += (int)value - maxHealth;
            }

            maxHealth = (int)value;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
    private int currentHealth;
    public float CurrentHealth
    {
        get {return currentHealth;}
        set
        {
            currentHealth = (int)value;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
    private float regenTick = 3f;
    private float regenInterval = 3f;
    private float regenAmount = 0;
    public float RegenAmount
    {
        get {return regenAmount;}
        set {regenAmount = value;}
    }
    private bool regenTrue = false;
    public bool RegenTrue
    {
        get {return regenTrue;}
        set {regenTrue = value;}
    }
    private float lifestealAmount = 0;
    public float LifestealAmount
    {
        get {return lifestealAmount;}
        set {lifestealAmount = value;}
    }
    
    //other vars
    [SerializeField] private GameObject damageText;
    public List<GameObject> lifeEggs;
    public UnityEvent onPlayerRespawn = new UnityEvent();
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        currentHealth = maxHealth;
    }

    void Update()
    {
        Regen();
        if (currentHealth <= 0 && GameSettings.gameState == GameState.InGame) //if the player dies
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
    void Regen()
    {
        if (GameSettings.gameState != GameState.InGame){return;}
        regenTick -= Time.deltaTime;
        if (regenTick <= 0 && regenTrue && currentHealth < maxHealth) //only works if the player is missing health
        {
            regenTick = regenInterval;
            CurrentHealth += regenAmount;
            Debug.Log($"Regen: {currentHealth}");
        }
    }

    void Respawn()
    {
        //This event currently has no listeners, it is here for future use 
        onPlayerRespawn?.Invoke();

        if (lifeEggs.Count > 0) //This should never run if there are no eggs, but this is here just in case
        {
            gameObject.transform.position = lifeEggs[lifeEggs.Count - 1].transform.position;
            Destroy(lifeEggs[lifeEggs.Count - 1]);
            lifeEggs.Remove(lifeEggs[lifeEggs.Count - 1]);
        }
        //Debug.Log("Player health before collisions turned off: " + currentHealth);
        currentHealth = maxHealth;
        StartCoroutine(DisableCollisionForDuration(2f));
    }

    public IEnumerator DisableCollisionForDuration(float duration)
    {
        // Set the collision matrix to ignore collisions between the player layer and enemy attacks for the specified duration
        Physics2D.IgnoreLayerCollision(7, 9, true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Re-enable collisions between the player layer and itself
        Physics2D.IgnoreLayerCollision(7, 9, false);
    }
    public void ReceiveDamage(int damageTaken)
    {
        GameObject damageTextInst = Instantiate(damageText, gameObject.transform);
        damageTextInst.GetComponent<TextMeshPro>().text = damageTaken.ToString();
        currentHealth -= damageTaken;
    }
}