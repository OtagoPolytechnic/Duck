using System.Collections;
using UnityEngine;

public class ShotgunBossBehaviour : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private Transform bulletPosition;
    private float attackRange = 10;
    private float attackInterval = 1.5f;
    private float[] minIntervalRange = { 10f, 8f, 7f, 6f };
    private float minJumpAttackInterval = 10f;
    private float[] maxIntervalRange = { 15f, 13f, 11f, 10f };
    private float maxJumpAttackInterval = 15f;
    private float attackResumptionDelay = 2f; // New variable for delay after shadow attack
    private float initialShootingDelay = 2f; // New variable for initial delay

    private float attackCooldown;
    private float jumpAttackTimer;
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
        switch (GameSettings.waveNumber)
        {
            case 5:
                minJumpAttackInterval = minIntervalRange[0];
                maxJumpAttackInterval = maxIntervalRange[0];
                break;
            case 10:
                minJumpAttackInterval = minIntervalRange[1];
                maxJumpAttackInterval = maxIntervalRange[1];
                break;
            case 15:
                minJumpAttackInterval = minIntervalRange[2];
                maxJumpAttackInterval = maxIntervalRange[2];
                break;
            default:
                minJumpAttackInterval = minIntervalRange[3];
                maxJumpAttackInterval = maxIntervalRange[3];
                break;
        }

        bossCollider = GetComponent<Collider2D>();
        if (bossCollider == null)
        {
            Debug.LogError("No Collider2D component found on the boss prefab.");
        }
    }

    private void Update()
    {

        if (GameSettings.gameState != GameState.InGame || Dying) return;
        if (SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Decoy");

        }
        else if (!SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }
        if (!isJumping)
        {
        HandleMovement();
        }
        // Movement-related updates
        HandleAttack();    // Attack-related updates
        UpdateBossVisibility();
        Bleed();
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
        jumpAttackTimer -= Time.deltaTime;
        if (jumpAttackTimer <= 0 && !isJumping)
        {
            if (SkillEffects.Instance.vanishActive) { return; }
            JumpAttack();
            jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval);
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
        currentShadow.transform.SetParent(gameObject.transform);
        ShadowAttack shadowAttack = currentShadow.GetComponent<ShadowAttack>();
        shadowAttack.GetComponent<ShadowAttack>().originBoss = this;

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
            newBullet.GetComponent<BossBullet>().originEnemy = this;
        }
        SFXManager.Instance.PlaySFX("EnemyShoot");
        attackCooldown = attackInterval;
    }
}
