using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public GameObject player;
    private int shockwaveDamage;
    public int ShockwaveDamage
    {
        set { shockwaveDamage = value; }
    }
    private int shockwaveSize;
    public int ShockwaveSize
    {
        set
        {
            shockwaveSize = value;
            transform.localScale = new Vector3(shockwaveSize, shockwaveSize, shockwaveSize);
        }
    }

    void Start()
    {

        ShockwaveSize = 15;

        StartCoroutine(DestroyExplosion());
    }
    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            DestroyExplosion();
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(40);


        }
    }
}