using System.Collections;
using UnityEngine;

public class ShotgunBossBehaviour : EnemyBase
{
    public GameObject player;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shadowPrefab; // Reference to the shadow prefab
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private float minJumpAttackInterval = 1f;
    [SerializeField] private float maxJumpAttackInterval = 3f;
    [SerializeField] private float jumpAttackCooldown = 2f;

    private float attackCooldown;
    private float jumpAttackTimer;
    private float jumpAttackCooldownTimer;
    private bool isJumping;
    private GameObject currentShadow;
    private SpriteRenderer bossSpriteRenderer;
    private Collider2D bossCollider;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
        jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval);

        Transform spriteChild = transform.Find("Sprite");
        bossSpriteRenderer = spriteChild ? spriteChild.GetComponent<SpriteRenderer>() : null;

        // Find and store the collider component
        bossCollider = GetComponent<Collider2D>();
        if (bossCollider == null)
        {
            Debug.LogError("No Collider2D component found on the boss prefab.");
        }
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Die();
            return;
        }

        if (GameSettings.gameState != GameState.InGame) return;

        HandleMovement();
        HandleAttack();
        UpdateBossVisibility();
        Bleed();
    }

    private void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle);

        if (distance >= attackRange)
        {
            Move();
        }
        else if (attackCooldown <= 0 && !isJumping)
        {
            ShotgunShoot();
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void HandleAttack()
    {
        if (jumpAttackCooldownTimer <= 0)
        {
            jumpAttackTimer -= Time.deltaTime;
            if (jumpAttackTimer <= 0 && !isJumping)
            {
                JumpAttack();
                jumpAttackCooldownTimer = jumpAttackCooldown;
                jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval);
            }
        }
        else
        {
            jumpAttackCooldownTimer -= Time.deltaTime;
        }
    }

    private void UpdateBossVisibility()
    {
        if (bossSpriteRenderer)
        {
            bossSpriteRenderer.enabled = currentShadow == null;
        }
    }

    public override void Move()
    {
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Speed * tileSpeedModifier * Time.deltaTime);
    }

    private void JumpAttack()
    {
        if (!shadowPrefab)
        {
            Debug.LogError("Shadow prefab is not assigned.");
            return;
        }

        // Disable the boss collider
        if (bossCollider)
        {
            bossCollider.enabled = false;
        }

        isJumping = true;
        currentShadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        ShadowAttack shadowAttack = currentShadow.GetComponent<ShadowAttack>();

        if (shadowAttack)
        {
            shadowAttack.SetShotgunBossBehaviour(this);
            shadowAttack.SetPlayer(player);
            shadowAttack.SetBossSpriteRenderer(bossSpriteRenderer);
        }
        else
        {
            Debug.LogError("ShadowAttack component not found on shadowPrefab.");
        }
    }

    public void ResetJumpState()
    {
        isJumping = false;
        currentShadow = null;

        // Re-enable the boss collider
        if (bossCollider)
        {
            bossCollider.enabled = true;
        }
    }

    private void ShotgunShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject newBullet = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
            float angleOffset = 10f * (i - 1);
            newBullet.GetComponent<BossBullet>().InitializeBullet(player, Damage, true, angleOffset);
        }
        SFXManager.Instance.EnemyShootSound();
        attackCooldown = attackInterval;
    }
}
