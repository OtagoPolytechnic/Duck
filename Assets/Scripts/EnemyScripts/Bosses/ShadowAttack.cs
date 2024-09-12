using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAttack : MonoBehaviour
{
    public GameObject player;
    private int shadowDamage;
    public int ShadowDamage
    {
        set { shadowDamage = value; }
    }
    private int shadowSize;
    public int ShadowSize
    {
        set
        {
            shadowSize = value;
            transform.localScale = new Vector3(shadowSize, shadowSize, shadowSize);
        }
    }

    void Start()
    {

        ShadowSize = 6;

        StartCoroutine(DestroyShadow());
    }
    private IEnumerator DestroyShadow()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            DestroyShadow();
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(10);


        }
    }
}