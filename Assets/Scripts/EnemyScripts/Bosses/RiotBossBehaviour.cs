using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotBossBehaviour : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject napalmBombPrefab;
    [SerializeField] private GameObject shieldPrefab;  
    [SerializeField] private Transform shotPoint;    
    [SerializeField] private Transform shieldPoint;    

    private float attackRange = 10f;
    private float attackInterval = 1.5f; 
    private float minNapalmInterval = 1f;
    private float maxNapalmInterval = 4f;
 
    private GameObject shieldInstance;                 
    private float minShieldInterval = 7f;
    private float maxShieldInterval = 15f;
    //private float shieldOffset = 5f;

    private float attackCooldown;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = attackInterval;
        StartCoroutine(SpawnNapalmBomb());
        StartCoroutine(SpawnShield());  
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame || Dying) { return; }

        if (shieldInstance == null && transform.GetChild(0).gameObject.activeSelf == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }

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
            transform.GetChild(1).rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        if (distance >= attackRange)
        {
            Move();
        }
        else if (GameSettings.gameState ==GameState.InGame && shieldInstance == null) 
        {
            if (attackCooldown <= 0)
            {
                Shoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime; 
            }
        }

        Bleed();
    }

    public override void Move()
    {
        if (SkillEffects.Instance.vanishActive) { return; }
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (Speed * tileSpeedModifier) * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, false);
        SFXManager.Instance.PlaySFX("EnemyShoot");
        attackCooldown = attackInterval; 
        newBullet.GetComponent<BossBullet>().originEnemy = this;
    }

    //Spawns napalm bomb at random intervals between set times
    private IEnumerator SpawnNapalmBomb()
    {
        while (true)
        {
            float randomInterval = Random.Range(minNapalmInterval, maxNapalmInterval);
            yield return new WaitForSeconds(randomInterval);
            if (GameSettings.gameState == GameState.InGame)
            {
                 GameObject napalmBomb = Instantiate(napalmBombPrefab, bulletPosition.position, bulletPosition.rotation);
            }
        }

    }
    //Spawns shield at random intervals between set times
    private IEnumerator SpawnShield()
    {
        while (true)
        {
         
            float randomInterval = Random.Range(minShieldInterval, maxShieldInterval);
            yield return new WaitForSeconds(randomInterval); 

            if (shieldInstance == null && GameSettings.gameState == GameState.InGame)
            {
              
                //Vector2 shieldPosition = (Vector2)shotPoint.position + (Vector2)(shotPoint.right * shieldOffset);     
                shieldInstance = Instantiate(shieldPrefab, shieldPoint.position, shieldPoint.rotation);             
                shieldInstance.transform.SetParent(shieldPoint);
                shieldInstance.transform.localPosition = Vector3.zero;

                //Activate shielding sprite
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);

                yield return new WaitUntil(() => shieldInstance == null);
            }
        }
    }
}
