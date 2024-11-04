using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    public float bulletSpeed = 2;
    private float range = 20f;
    private Vector2 heldVelocity;
    private Vector3 startPos;

    // Original color of the sprite
    private Color originalColor;

    // Original size of the collider
    private float originalColliderRadius;

    // Reference to the child GameObject (Explosion)
    public GameObject explosionPrefab;

    // Original scale of the explosion child
    private Vector3 originalScale;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Save the original color and size of the sprite and collider
        originalColor = spriteRenderer.color;
        originalColliderRadius = circleCollider.radius;
        heldVelocity = rb.velocity;
        // Start the random explosion coroutine
        StartCoroutine(RandomExplodeCoroutine());
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame && rb.velocity != Vector2.zero) 
        {
            rb.velocity = Vector2.zero;
        }
        else if (GameSettings.gameState == GameState.InGame && rb.velocity == Vector2.zero)
        {
            rb.velocity = heldVelocity;
        }
        // Destroy bullet after range
        float distTravelled = Vector3.Distance(startPos, transform.position);

        if (distTravelled > range)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator RandomExplodeCoroutine()
    {
        // Wait for a random time between 0,5 and 2 seconds
        float waitTime = Random.Range(.5f, 2f);
        float elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            elapsedTime += .1f;
            yield return new WaitForSeconds(.1f);
            while (GameSettings.gameState != GameState.InGame)
            {
                yield return null;
            }
        }
        // Call the method to handle explosion logic
        StartCoroutine(FlashEffectCoroutine());
    }

    private IEnumerator FlashEffectCoroutine()
    {
        // Number of flashes
        int flashCount = 5;
        float flashDuration = 0.1f; // Duration of each flash (on and off)

        // Flashing loop
        for (int i = 0; i < flashCount; i++)
        {
            // Flash red
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            
            while (GameSettings.gameState != GameState.InGame)
            {
                yield return null;
            }

            // Flash back to original color
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
            while (GameSettings.gameState != GameState.InGame)
            {
                yield return null;
            }
        
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        // Destroy the object after flashing
        Destroy(gameObject);
    }

}
