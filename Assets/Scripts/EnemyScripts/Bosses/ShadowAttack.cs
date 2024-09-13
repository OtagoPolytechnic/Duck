using System.Collections;
using UnityEngine;

public class ShadowAttack : MonoBehaviour
{
    public GameObject player;
    public float followSpeed = 5.0f; // Speed at which the shadow follows the player
    public float scaleUpDuration = 3.0f; // Duration over which the shadow will scale up
    public GameObject shadowShockwavePrefab; // Reference to the ShadowShockwave prefab

    private int shadowDamage;
    private int shadowSize;
    private ShotgunBossBehaviour shotgunBossBehaviour; // Reference to the ShotgunBossBehaviour component
    private SpriteRenderer bossSpriteRenderer; // Reference to the boss's SpriteRenderer
    private bool isFinalPhase = false; // Flag to control final movement
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float scaleUpTimer = 0f;

    public int ShadowDamage
    {
        set { shadowDamage = value; }
    }

    public int ShadowSize
    {
        set
        {
            shadowSize = value;
            transform.localScale = new Vector3(shadowSize, shadowSize, shadowSize);
        }
    }

    private void Awake()
    {
        Debug.Log("ShadowAttack Awake called.");
    }

    void Start()
    {
        Debug.Log("ShadowAttack Start called.");
        initialScale = transform.localScale; // Capture the initial scale
        ShadowSize = 6;

        // Make the shadow visible immediately
        StartCoroutine(ShadowLifetime(5.0f)); // Shadow will exist for 5 seconds
    }

    private IEnumerator ShadowLifetime(float duration)
    {
        Debug.Log("ShadowLifetime coroutine started.");

        // Start the final phase countdown
        yield return new WaitForSeconds(duration - 3.0f); // Wait for the duration minus the time needed for the final phase

        // Stop shadow movement and make boss sprite visible
        isFinalPhase = true;
        targetScale = initialScale * 5; // Five times the original size

        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.enabled = true; // Make the boss sprite visible
            Debug.Log("Boss sprite set to visible.");
        }
        else
        {
            Debug.LogError("Boss SpriteRenderer is null in ShadowLifetime coroutine.");
        }

        if (shotgunBossBehaviour != null)
        {
            shotgunBossBehaviour.transform.position = transform.position; // Move the boss to shadow's position
            Debug.Log("ShotgunBossBehaviour moved to shadow's position.");
        }
        else
        {
            Debug.LogError("ShotgunBossBehaviour is null in ShadowLifetime coroutine.");
        }

        // Continue to scale up until the duration has passed
        scaleUpTimer = 0f;

        // Wait for 2 seconds before destruction
        yield return new WaitForSeconds(0.5f);

        // Instantiate the ShadowShockwave before destroying the shadow
        if (shadowShockwavePrefab != null)
        {
            Instantiate(shadowShockwavePrefab, transform.position, Quaternion.identity);
            Debug.Log("ShadowShockwave instantiated.");
        }
        else
        {
            Debug.LogError("ShadowShockwave prefab is not assigned.");
        }

        Destroy(gameObject);
        Debug.Log("Shadow destroyed.");

        if (shotgunBossBehaviour != null)
        {
            shotgunBossBehaviour.ResetJumpState(); // Notify the boss to continue shooting
            Debug.Log("ShotgunBossBehaviour notified to reset jump state.");
        }
        else
        {
            Debug.LogError("ShotgunBossBehaviour is null in ShadowLifetime coroutine.");
        }
    }

    public void SetShotgunBossBehaviour(ShotgunBossBehaviour bossBehaviour)
    {
        shotgunBossBehaviour = bossBehaviour;
        Debug.Log("ShotgunBossBehaviour set.");
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        Debug.Log("Player reference set.");
    }

    public void SetBossSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.bossSpriteRenderer = spriteRenderer;
        Debug.Log("Boss SpriteRenderer reference set.");
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in Update!");
            return;
        }

        if (isFinalPhase)
        {
            // In final phase, stop moving and scale up
            if (scaleUpTimer < scaleUpDuration)
            {
                scaleUpTimer += Time.deltaTime;
                float scaleLerp = Mathf.Clamp01(scaleUpTimer / scaleUpDuration);
                transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleLerp);
            }
            Debug.Log("Shadow is in final phase and scaling up.");
            return;
        }

        Debug.Log("ShadowUpdate called.");
        // Smoothly move the shadow towards the player's position
        Vector3 targetPosition = player.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
