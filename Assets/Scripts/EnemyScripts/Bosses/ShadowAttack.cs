using System.Collections;
using UnityEngine;

public class ShadowAttack : MonoBehaviour
{
    public GameObject player;
    public float followSpeedModifier = 0.7f; // A modifier to adjust follow speed relative to player speed
    public float scaleUpDuration = 3.0f; // Duration over which the shadow will scale up
    public GameObject shadowShockwavePrefab; // Reference to the ShadowShockwave prefab
    public EnemyBase originBoss;

    private float followSpeed;
    private int shadowSize = 6; // Default size
    private ShotgunBossBehaviour shotgunBossBehaviour;
    private SpriteRenderer bossSpriteRenderer;
    private bool isFinalPhase;
    private Vector3 initialScale;
    private float scaleUpTimer;
    private float scaleUpRate;

    private float duration = 3.3f;

    // Sets the shadow size and updates its scale accordingly.
    public int ShadowSize
    {
        set
        {
            shadowSize = value;
            transform.localScale = Vector3.one * shadowSize;
        }
    }

    // Initializes the shadow's initial properties, including its scale and scaling rate. Also starts the coroutine to handle the shadow's lifetime.
    private void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.one * shadowSize;
        scaleUpRate = (20 - shadowSize) / scaleUpDuration; // Calculate the scaling rate per second
    }

    // Sets the reference to the ShotgunBossBehaviour component.
    public void SetShotgunBossBehaviour(ShotgunBossBehaviour bossBehaviour)
    {
        shotgunBossBehaviour = bossBehaviour;
    }

    // Sets the reference to the player GameObject.
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    // Sets the reference to the SpriteRenderer component of the boss.
    public void SetBossSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.bossSpriteRenderer = spriteRenderer;
    }

    // Updates the shadow's position to follow the player and scales the shadow up gradually over time.
    private void Update()
    {
        if (GameSettings.gameState != GameState.InGame || originBoss.Dying)
        {
            return;
        }

        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Instantiate(shadowShockwavePrefab, transform.position, Quaternion.identity);
            bossSpriteRenderer.enabled = true;
            shotgunBossBehaviour.transform.position = transform.position;
            shotgunBossBehaviour?.ResetJumpState();
            Destroy(gameObject);
        }

        if (duration >= .5)
        {
            // Dynamically calculate the follow speed based on the player's move speed
            float playerSpeed = TopDownMovement.Instance.MoveSpeed;
            followSpeed = playerSpeed * followSpeedModifier;

            // Smoothly move the shadow towards the player's position
            transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);
        }

        // Gradually increase the shadow's size
        if (scaleUpTimer < scaleUpDuration)
        {
            scaleUpTimer += Time.deltaTime;
            float scaleLerp = Mathf.Clamp01(scaleUpTimer / scaleUpDuration);
            float newScale = Mathf.Lerp(shadowSize, 90, scaleLerp);
            transform.localScale = Vector3.one * newScale;
        }
    }
}
