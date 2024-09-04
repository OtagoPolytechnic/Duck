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

        //Vector3 direction = player.transform.position - transform.position;
        //rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        //bulletSpeed = 2 + (GameSettings.waveNumber / 5) * 5; // Increase speed by 5 for every 5 levels

        // Save the original color and size of the sprite and collider
        originalColor = spriteRenderer.color;
        originalColliderRadius = circleCollider.radius;

        // Start the random explosion coroutine
        StartCoroutine(RandomExplodeCoroutine());
    }

    void Update()
    {
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
        yield return new WaitForSeconds(waitTime);

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

            // Flash back to original color
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);

        
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Debug.LogError("Instantiated explosion");
        // Destroy the object after flashing
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Destroy bullet on hit with player and lower health
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
          
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
