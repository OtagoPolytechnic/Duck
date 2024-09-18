using System.Collections;
using UnityEngine;

public class ShotgunBossBehaviour : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private float minJumpAttackInterval = 1f;
    [SerializeField] private float maxJumpAttackInterval = 3f;
    [SerializeField] private float jumpAttackCooldown = 2f;
    [SerializeField] private float attackResumptionDelay = 2f; // New variable for delay after shadow attack
    [SerializeField] private float initialShootingDelay = 2f; // New variable for initial delay

    private float attackCooldown;
    private float jumpAttackTimer;
    private float jumpAttackCooldownTimer;
    private float resumptionDelayTimer; // New timer for delay
    private float initialShootingDelayTimer; // New timer for initial shooting delay
    private bool isJumping;
    private GameObject currentShadow;
    private SpriteRenderer bossSpriteRenderer;
    private Collider2D bossCollider;

    // Initializes the boss properties, including player reference, timers, and components.
    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
        jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval);
        resumptionDelayTimer = 0; // Initialize the resumption delay timer
        initialShootingDelayTimer = initialShootingDelay; // Initialize the initial shooting delay timer

        Transform spriteChild = transform.Find("Sprite");
        bossSpriteRenderer = spriteChild ? spriteChild.GetComponent<SpriteRenderer>() : null;

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
        if (SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Decoy");

        }
        else if (!SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }

        HandleMovement();  // Movement-related updates
        HandleAttack();    // Attack-related updates
        UpdateBossVisibility();
        Bleed();
    }

    private void HandleMovement()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (SkillEffects.Instance.vanishActive) { return; }
        else
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        // Move if not in attack cooldown and there's no delay
        if (distance >= attackRange && initialShootingDelayTimer <= 0)
        {
            Move();
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void HandleAttack()
    {
        // Handle jump attack
        if (jumpAttackCooldownTimer <= 0)
        {
            jumpAttackTimer -= Time.deltaTime;
            if (jumpAttackTimer <= 0 && !isJumping)
            {
                if (SkillEffects.Instance.vanishActive) { return; }
                JumpAttack();
                jumpAttackCooldownTimer = jumpAttackCooldown;
                jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval);
            }
        }
        else
        {
            jumpAttackCooldownTimer -= Time.deltaTime;
        }

        // Handle shooting if cooldown allows and there's no delay
        if (attackCooldown <= 0 && !isJumping && resumptionDelayTimer <= 0 && initialShootingDelayTimer <= 0)
        {
            ShotgunShoot();
        }
    }


    // Updates the visibility of the boss based on the presence of a current shadow attack.
    private void UpdateBossVisibility()
    {
        if (bossSpriteRenderer)
        {
            bossSpriteRenderer.enabled = currentShadow == null;
        }
    }

    // Moves the boss towards the player, adjusting movement speed based on the tile type.
    public override void Move()
    {
        if (SkillEffects.Instance.vanishActive) { return; }
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Speed * tileSpeedModifier * Time.deltaTime);
    }

    // Initiates a jump attack by creating a shadow attack and disabling the boss's collider.
    private void JumpAttack()
    {
        if (!shadowPrefab)
        {
            Debug.LogError("Shadow prefab is not assigned.");
            return;
        }

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

    // Resets the jump attack state, re-enables the collider, and starts the resumption delay timer.
    public void ResetJumpState()
    {
        isJumping = false;
        currentShadow = null;

        if (bossCollider)
        {
            bossCollider.enabled = true;
        }

        // Start the resumption delay timer
        resumptionDelayTimer = attackResumptionDelay;
    }

    // Creates and fires a set of bullets at the player, and handles the shooting sound effect.
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

    // Manages the timers for resumption delay and initial shooting delay.
    private void LateUpdate()
    {
        // Manage the resumption delay timer
        if (resumptionDelayTimer > 0)
        {
            resumptionDelayTimer -= Time.deltaTime;
        }

        // Manage the initial shooting delay timer
        if (initialShootingDelayTimer > 0)
        {
            initialShootingDelayTimer -= Time.deltaTime;
        }
    }
}
