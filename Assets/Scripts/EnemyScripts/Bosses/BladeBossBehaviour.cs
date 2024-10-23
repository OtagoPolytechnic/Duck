using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeBossBehaviour : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject bladePrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject bladeCenterPrefab;

    private float attackRange = 50f;
    private float attackInterval = 1.5f;
    private float attackCooldown;

    [SerializeField] private int numberOfBlades = 30;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float spacing = 0.5f;

    private bool isCharging = false;
    private float chargeDuration = 1f;
    private float chargeTimer = 0f; 

    private float chargeCooldown; 
    private float chargeCooldownMin = 1f; 
    private float chargeCooldownMax = 2f; 

    private Vector2 targetPosition; 

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = attackInterval;
        chargeCooldown = Random.Range(chargeCooldownMin, chargeCooldownMax); 
        SpawnBlades();
    }

    private void SpawnBlades()
    {
        float angleIncrement = 360f / numberOfBlades;
        float adjustedRadius = radius + (spacing * 5.5f);
        Vector2 centerPosition = Vector2.zero;

        for (int i = 0; i < numberOfBlades; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            Vector2 bladePosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * adjustedRadius + (Vector2)player.transform.position;

            Instantiate(bladePrefab, bladePosition, Quaternion.identity);
            centerPosition += bladePosition;
        }

        centerPosition /= numberOfBlades;
        Instantiate(bladeCenterPrefab, centerPosition, Quaternion.identity);
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame || Dying) { return; }

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

        if (SkillEffects.Instance.vanishActive) { return; }
        else
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);
        }

 
        if (isCharging)
        {
            Charge();
        }
        else
        {
            if (distance >= attackRange)
            {
                Move(); 
            }
            else if (GameSettings.gameState == GameState.InGame)
            {
          
                chargeCooldown -= Time.deltaTime;

                // Check if it's time to charge
                if (chargeCooldown <= 0)
                {
                    StartCharging();
                    chargeCooldown = Random.Range(chargeCooldownMin, chargeCooldownMax); 
                }
                else if (attackCooldown <= 0)
                {
                    Shoot();
                    attackCooldown = attackInterval; 
                }
                else
                {
                    attackCooldown -= Time.deltaTime; 
                }
            }
        }

        Bleed();
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0f; 
        targetPosition = player.transform.position; 
        Debug.Log("BladeBoss is charging towards position: " + targetPosition);
    }

    private void Charge()
    {
      
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * 10 * Time.deltaTime);

      
        chargeTimer += Time.deltaTime;

     
        if (chargeTimer >= chargeDuration)
        {
            Debug.Log("BladeBoss has finished charging.");
            isCharging = false;
            attackCooldown = attackInterval; 
        }
    }

    public override void Move()
    {
        if (SkillEffects.Instance.vanishActive || isCharging) { return; }
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (Speed * tileSpeedModifier) * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, false);
        SFXManager.Instance.PlaySFX("EnemyShoot");
    }
}
