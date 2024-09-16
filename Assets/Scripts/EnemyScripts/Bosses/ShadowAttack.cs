using System.Collections;
using UnityEngine;

public class ShadowAttack : MonoBehaviour
{
    public GameObject player;
    public float followSpeed = 5.0f; // Speed at which the shadow follows the player
    public float scaleUpDuration = 3.0f; // Duration over which the shadow will scale up
    public GameObject shadowShockwavePrefab; // Reference to the ShadowShockwave prefab

    private int shadowDamage;
    private int shadowSize = 6; // Default size
    private ShotgunBossBehaviour shotgunBossBehaviour;
    private SpriteRenderer bossSpriteRenderer;
    private bool isFinalPhase;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float scaleUpTimer;

    public int ShadowDamage
    {
        set => shadowDamage = value;
    }

    public int ShadowSize
    {
        set
        {
            shadowSize = value;
            transform.localScale = Vector3.one * shadowSize;
        }
    }

    private void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.one * shadowSize;
        StartCoroutine(ShadowLifetime(5.0f));
    }

    private IEnumerator ShadowLifetime(float duration)
    {
        yield return new WaitForSeconds(duration - 3.0f);
        BeginFinalPhase();
        yield return new WaitForSeconds(0.5f);

        if (shadowShockwavePrefab)
            Instantiate(shadowShockwavePrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
        shotgunBossBehaviour?.ResetJumpState();
    }

    private void BeginFinalPhase()
    {
        isFinalPhase = true;
        targetScale = initialScale * 5;

        if (bossSpriteRenderer)
            bossSpriteRenderer.enabled = true;


        if (shotgunBossBehaviour)
            shotgunBossBehaviour.transform.position = transform.position;
    }

    public void SetShotgunBossBehaviour(ShotgunBossBehaviour bossBehaviour)
    {
        shotgunBossBehaviour = bossBehaviour;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetBossSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.bossSpriteRenderer = spriteRenderer;

    }

    private void Update()
    {
        if (player == null) return;

        if (isFinalPhase)
        {
            if (scaleUpTimer < scaleUpDuration)
            {
                scaleUpTimer += Time.deltaTime;
                float scaleLerp = Mathf.Clamp01(scaleUpTimer / scaleUpDuration);
                transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleLerp);
            }
            return;
        }

        // Smoothly move the shadow towards the player's position
        transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);
    }
}
