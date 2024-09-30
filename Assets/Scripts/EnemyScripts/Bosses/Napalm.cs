using System.Collections;
using UnityEngine;

public class Napalm : MonoBehaviour
{
    public int napalmDamage;
    public float duration = 20f;
    public float damageInterval = 2f;
    private PlayerStats playerStats;
    private Coroutine damageCoroutine;

    void Start()
    {
        napalmDamage = 1 + GameSettings.waveNumber;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        StartCoroutine(DestroyNapalm());
    }

    private IEnumerator DestroyNapalm()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(DealDamage());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DealDamage()
    {
        while (true)
        {
            playerStats.ReceiveDamage(napalmDamage);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
