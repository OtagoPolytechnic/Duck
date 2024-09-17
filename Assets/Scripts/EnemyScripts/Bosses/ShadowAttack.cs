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
    private float scaleUpRate;

 
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
        scaleUpRate = (20 - shadowSize) / scaleUpDuration; // Calculate the scaling rate per second
        StartCoroutine(ShadowLifetime(5.0f));
    }
  
    private IEnumerator ShadowLifetime(float duration)
    {
        yield return new WaitForSeconds(duration - 3.0f);
        //BeginFinalPhase();
        yield return new WaitForSeconds(0.3f);

        if (shadowShockwavePrefab)
            Instantiate(shadowShockwavePrefab, transform.position, Quaternion.identity);
        if (bossSpriteRenderer)
            bossSpriteRenderer.enabled = true;

        if (shotgunBossBehaviour)
            shotgunBossBehaviour.transform.position = transform.position;
       
        Destroy(gameObject);
        shotgunBossBehaviour?.ResetJumpState();
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

        // Smoothly move the shadow towards the player's position
        transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);

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
