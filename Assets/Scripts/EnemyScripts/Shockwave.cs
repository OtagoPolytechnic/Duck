using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public GameObject player;
    private int shockwaveDamage;
    public bool playerHit;
      public int ShockwaveDamage
    {
        get { return shockwaveDamage; }
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

    // Initializes the shockwave properties, including damage, size, and starts the destruction coroutine.
    void Start()
    {

        shockwaveDamage = 30 + GameSettings.waveNumber;
        player = GameObject.FindGameObjectWithTag("Player");
        ShockwaveSize = 12;

        StartCoroutine(DestroyExplosion());
    }


    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Handles collision with other GameObjects, applying damage to the player if hit.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerHit == false)
        {

            //DestroyExplosion();
            player.GetComponent<PlayerStats>().ReceiveDamage(shockwaveDamage);


        }
        }
    }
