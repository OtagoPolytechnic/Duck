using System.Collections;
using UnityEngine;

public class Napalm : MonoBehaviour
{
    public int napalmDamage;
    public float duration = 20f;
    public float damageInterval = 2f;
    private PlayerStats playerStats;
    private Coroutine damageCoroutine;
    private float damageTick;
  
    void Start()
    {
        napalmDamage = 1;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame) { return; }

        damageTick -= Time.deltaTime;
        if (damageTick <= 0)
        {
            DealDamage();
            damageTick = damageInterval;
        }
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }
  

    private void DealDamage()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);
        foreach (Collider2D c in overlaps)
        {
            if (c.CompareTag("Player"))
            {
                playerStats.ReceiveDamage(napalmDamage);
            }
        }   
    }
}