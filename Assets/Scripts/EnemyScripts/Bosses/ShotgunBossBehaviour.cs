using System.Collections;
using UnityEngine;

public class ShotgunBossBehaviour : EnemyBase
{
    public GameObject player;
    private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shadowPrefab; // Reference to the shadow prefab
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private float minJumpAttackInterval = 1f; // Minimum interval between jump attacks
    [SerializeField] private float maxJumpAttackInterval = 3f; // Maximum interval between jump attacks
    [SerializeField] private float jumpAttackCooldown = 2f; // Cooldown period after each jump attack

    private float attackCooldown;
    private float jumpAttackTimer;
    private float jumpAttackCooldownTimer;
    private bool isJumping = false;
    private GameObject currentShadow; // Reference to the current shadow instance
    private SpriteRenderer bossSpriteRenderer; // Reference to the SpriteRenderer component of the Sprite child

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attackCooldown = 0;
        jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval);
        jumpAttackCooldownTimer = 0; // Initialize cooldown timer

        // Initialize the SpriteRenderer component
        Transform spriteChild = transform.Find("Sprite");
        if (spriteChild != null)
        {
            bossSpriteRenderer = spriteChild.GetComponent<SpriteRenderer>();
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
            if (attackCooldown <= 0 && !isJumping)
            {
                ShotgunShoot();
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }

        // Handle jump attack timing
        if (jumpAttackCooldownTimer <= 0)
        {
            jumpAttackTimer -= Time.deltaTime;
            if (jumpAttackTimer <= 0 && !isJumping)
            {
                JumpAttack();
                jumpAttackCooldownTimer = jumpAttackCooldown; // Set cooldown after a jump attack
                jumpAttackTimer = Random.Range(minJumpAttackInterval, maxJumpAttackInterval); // Reset jump attack timer
            }
        }
        else
        {
            jumpAttackCooldownTimer -= Time.deltaTime; // Decrease cooldown timer
        }

        // Update the visibility of the boss sprite based on the shadow presence
        UpdateBossVisibility();

        Bleed();
    }

    private void UpdateBossVisibility()
    {
        if (currentShadow != null)
        {
            if (bossSpriteRenderer != null)
            {
                bossSpriteRenderer.enabled = false; // Hide the boss sprite while shadow exists
            }
        }
        else
        {
            if (bossSpriteRenderer != null)
            {
                bossSpriteRenderer.enabled = true; // Show the boss sprite when shadow is not present
            }
        }
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

        // Instantiate the shadow prefab at the boss's position
        if (shadowPrefab != null)
        {
            currentShadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);

            // Set the ShotgunBossBehaviour reference in the shadow prefab
            ShadowAttack shadowAttack = currentShadow.GetComponent<ShadowAttack>();
            if (shadowAttack != null)
            {
                shadowAttack.SetShotgunBossBehaviour(this);
                shadowAttack.SetPlayer(player); // Set the player reference
                shadowAttack.SetBossSpriteRenderer(bossSpriteRenderer); // Pass the boss's SpriteRenderer
                Debug.Log("ShadowAttack component set with ShotgunBossBehaviour, player, and boss SpriteRenderer.");
            }
            else
            {
                Debug.LogError("ShadowAttack component not found on shadowPrefab.");
            }
        }
        else
        {
            Debug.LogError("Shadow prefab is not assigned.");
        }
    }

    public void ResetJumpState()
    {
        Debug.Log("ResetJumpState");
        isJumping = false;
        currentShadow = null; // Clear the reference to the shadow
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
