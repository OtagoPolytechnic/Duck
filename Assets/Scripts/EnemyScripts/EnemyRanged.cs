using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    private float attackCooldown;
    private void Awake()
    {
        ScaleStats();
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
        Health = BaseHealth;
    }

    void Update()
    {
        
        if (GameSettings.gameState != GameState.InGame || Dying) {return;}

        if (Health <= 0) 
        { 
            StartCoroutine(Die());
            Dying = true;
        }
        Bleed();

        if (SkillEffects.Instance.decoyActive && !stopCheck)
        {
            player = GameObject.FindGameObjectWithTag("Decoy");
            stopCheck = true;
        }
        else if (!SkillEffects.Instance.decoyActive && stopCheck)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            stopCheck = false;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        //turns enemy towards player
        if (SkillEffects.Instance.vanishActive) { return; }
        else
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        

        if (distance >= attackRange)
        { 
            Move();
        }
        else
        {
            if( attackCooldown <= 0)
            {
                if (SkillEffects.Instance.vanishActive) { return; }
                Shoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
        
    }
    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        newBullet.GetComponent<EnemyBullet>().Damage = Damage;
        newBullet.GetComponent<EnemyBullet>().originEnemy = this;
        attackCooldown = attackInterval;

        // Play the enemy shooting sound
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX("EnemyShoot");
        }
        else
        {
            Debug.LogError("SFXManager instance is null in EnemyRanged.Shoot().");
        }

        attackCooldown = attackInterval;
    }

    public override void Move()
    {
        if (SkillEffects.Instance.vanishActive) { return; }
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (Speed * tileSpeedModifier) * Time.deltaTime);
    }
    
    // public override void Die()
    // {
    //     SFXManager.Instance.PlaySFX("EnemyDie");
    //     ScoreManager.Instance.IncreasePoints(Points);
    //     EnemySpawner.Instance.currentEnemies.Remove(gameObject);
    //     Destroy(gameObject);
    // }
}