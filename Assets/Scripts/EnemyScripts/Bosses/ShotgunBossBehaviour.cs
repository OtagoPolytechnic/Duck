using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBossBehaviour : EnemyBase
{
    public GameObject player;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shadow;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private float jumpAttackMinInterval = 5f;
    [SerializeField] private float jumpAttackMaxInterval = 10f;

    private float attackCooldown;
    private float jumpAttackCooldown;
    private float jumpAttackTimer;
    private bool isJumping = false;
    private Renderer bossRenderer; // Reference to the boss's Renderer component
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component of the Sprite child

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
        jumpAttackCooldown = Random.Range(jumpAttackMinInterval, jumpAttackMaxInterval);
        jumpAttackTimer = 0;

        // Initialize the Renderer component
        bossRenderer = GetComponent<Renderer>();

        // Initialize the SpriteRenderer component
        Transform spriteChild = transform.Find("Sprite");
        if (spriteChild != null)
        {
            spriteRenderer = spriteChild.GetComponent<SpriteRenderer>();
        }
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

        // Turns enemy towards player
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);

        if (distance >= attackRange)
        {
            Move();
        }
        else
        {
            if (attackCooldown <= 0 && isJumping == false)
            {
                ShotgunShoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }

        // Handle jump attack timing
        jumpAttackTimer += Time.deltaTime;
        if (jumpAttackTimer >= jumpAttackCooldown)
        {
            JumpAttack();
            jumpAttackTimer = 0;
            jumpAttackCooldown = Random.Range(jumpAttackMinInterval, jumpAttackMaxInterval);
        }

        Bleed();
    }

    public override void Move()
    {
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (Speed * tileSpeedModifier) * Time.deltaTime);
    }

    void JumpAttack()
    {
        isJumping = true;
        Debug.Log("JUMPATTACK started");

        // Make the boss invisible
        if (bossRenderer != null)
        {
            bossRenderer.enabled = false;
        }

        // Make the SpriteRenderer invisible
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        StartCoroutine(ResetJumpStateAfterDelay(3f));
    }

    private IEnumerator ResetJumpStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Make the boss visible again
        if (bossRenderer != null)
        {
            bossRenderer.enabled = true;
        }

        // Make the SpriteRenderer visible again
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isJumping = false;
        Debug.Log("JUMPATTACK ended");
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

        Debug.Log($"ShotgunShoot executed with attackInterval = {attackInterval}");
    }
}
