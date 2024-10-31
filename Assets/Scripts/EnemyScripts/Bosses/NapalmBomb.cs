using System.Collections;
using UnityEngine;

public class NapalmBomb : MonoBehaviour
{
    [SerializeField] private GameObject napalmFirePrefab;
    private float moveSpeed = 8f;
    private Rigidbody2D rb;
    private GameObject player;
    private float minDurationBeforeStop = .5f;
    private float maxDurationBeforeStop = 2f;
    private int minFirePrefabs = 4;
    private int maxFirePrefabs = 10;
    private float spreadRadius = 3f;
    private float randomDuration;
    private Vector2 heldVelocity;

    private void Start()
    {
        if (SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Decoy");
        }
        else if (!SkillEffects.Instance.decoyActive)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
        randomDuration = Random.Range(minDurationBeforeStop, maxDurationBeforeStop);
        heldVelocity = rb.velocity;
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
        randomDuration -= Time.deltaTime;
        if (randomDuration <= 0)
        {
            rb.velocity = Vector2.zero;
            Explode();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Edges") || other.CompareTag("Decoy"))
        {
            Destroy(gameObject);
        }
    }

    //Instantiates a random amount of fire prefabs upon destruction
    private void Explode()
    {
        int fireCount = Random.Range(minFirePrefabs, maxFirePrefabs + 1);
        for (int i = 0; i < fireCount; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * spreadRadius;
            if (napalmFirePrefab != null)
            {
                Instantiate(napalmFirePrefab, randomPosition, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }
}
