using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBase
{
    public GameObject player;

    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;


    private float attackCooldown;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
    }
    void Update()
    {
        if (Health <= 0)
        {

            Die();
        }
        if (GameSettings.gameState != GameState.InGame) { return; }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        //turns enemy towards player
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);

        if (distance >= attackRange)
        {
            Move();
        }
        else
        {
            if (attackCooldown <= 0 && GameObject.Find("ShotgunBoss(Clone)") == null)
            {
                Debug.Log("Normal shoot");
                Shoot();
            }
            else if (attackCooldown <= 0 && GameObject.Find("ShotgunBoss(Clone)") != null)
            {
                Debug.Log("Shotgun shoot");
                ShotgunShoot();
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
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (Speed * tileSpeedModifier) * Time.deltaTime);
    }
    public override void Die()
    {
        SFXManager.Instance.EnemyDieSound();
        ScoreManager.Instance.IncreasePoints(Points);
        EnemySpawner.Instance.currentEnemies.Remove(gameObject);
        Destroy(gameObject);

    }
    void Shoot()
    {
        // Normal boss shooting
        GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, false); // Pass false for shotgun
        SFXManager.Instance.EnemyShootSound();
        attackCooldown = attackInterval;
    }

    void ShotgunShoot()
    {
        // Shotgun boss shooting
        for (int i = 0; i < 3; i++) // Shoot 3 bullets
        {
            GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
            float angleOffset = 10f * (i - 1); // Adjust angle offsets as needed
            newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, true, angleOffset); // Pass true for shotgun and angle offset
        }
        SFXManager.Instance.EnemyShootSound();
        attackCooldown = attackInterval;
    }
}