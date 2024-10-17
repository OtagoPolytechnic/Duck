using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBossBehaviour : EnemyBase
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
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
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

        if (distance >= attackRange)
        {
            Move();
        }
        else
        {
            if (attackCooldown <= 0)
            {
                if (SkillEffects.Instance.vanishActive) { return; }
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
        // Normal boss shooting
        GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, false); // Pass false for shotgun
        newBullet.GetComponent<BossBullet>().originEnemy = this;
        SFXManager.Instance.PlaySFX("EnemyShoot");
        attackCooldown = attackInterval;
    }
}