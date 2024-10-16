using System.Collections;
using UnityEngine;

public class NapalmBomb : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Rigidbody2D rb;
    private GameObject player;
    public float minDurationBeforeStop = .5f;
    public float maxDurationBeforeStop = 2f;
    public GameObject napalmFirePrefab;
    public int minFirePrefabs = 4;
    public int maxFirePrefabs = 10;
    public float spreadRadius = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            StartCoroutine(StopMovementAndExplode());
        }
        else
        {
            Debug.LogWarning("Player not found in the scene!");
            Destroy(gameObject);
        }
        float randomDuration = Random.Range(minDurationBeforeStop, maxDurationBeforeStop);
    }

    void Update()
    {
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
            else
            {
                Debug.LogWarning("Napalm fire prefab is not assigned!");
            }
        }

        Debug.Log("Napalm Bomb exploded!");
        Destroy(gameObject);
    }
}
