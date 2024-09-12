using System.Collections;
using UnityEngine;

public class ShadowAttack : MonoBehaviour
{
    public GameObject player;
    public float followSpeed = 5.0f; // Speed at which the shadow follows the player
    private int shadowDamage;
    private int shadowSize;
    private ShotgunBossBehaviour shotgunBossBehaviour; // Reference to the ShotgunBossBehaviour component
    private SpriteRenderer bossSpriteRenderer; // Reference to the boss's SpriteRenderer

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
        // No SpriteRenderer lookup needed here
    }

    void Start()
    {
        Debug.Log("ShadowAttack Start called.");
        ShadowSize = 6;

        // Make the shadow visible immediately
        StartCoroutine(ShadowLifetime(5.0f)); // Shadow will exist for 10 seconds
    }

    private IEnumerator ShadowLifetime(float duration)
    {
        Debug.Log("ShadowLifetime coroutine started.");

        // Hide the boss sprite
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.enabled = false;
            Debug.Log("Boss sprite hidden.");
        }
        else
        {
            Debug.LogError("Boss SpriteRenderer is null in ShadowLifetime coroutine.");
        }

        yield return new WaitForSeconds(duration - 1.0f); // Wait for the duration minus the time needed to make the sprite visible

        // Ensure the boss sprite is visible
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.enabled = true;
            Debug.Log("Boss sprite set to visible before destruction.");
        }
        else
        {
            Debug.LogError("Boss SpriteRenderer is null in ShadowLifetime coroutine.");
        }

        yield return new WaitForSeconds(1.0f); // Wait a bit longer to ensure visibility before destruction

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

        Debug.Log("ShadowUpdate called.");
        // Smoothly move the shadow towards the player's position
        Vector3 targetPosition = player.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
