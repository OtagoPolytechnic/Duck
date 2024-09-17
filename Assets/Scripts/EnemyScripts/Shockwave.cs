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

        shockwaveDamage = 30 + (GameSettings.waveNumber / 5) * 5;
        player = GameObject.FindGameObjectWithTag("Player");
        ShockwaveSize = 12;

        StartCoroutine(DestroyExplosion());
    }

    // Initializes the shockwave with specific player reference and damage values.
    public void InitializeBullet(GameObject player, int damage, bool isShotgun, float angleOffset = 0f)
    {
        this.player = player;
        this.shockwaveDamage = damage;

    }
    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(1.5f);
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
        else if (other.gameObject.CompareTag("Player") && playerHit == true)
        {

            //DestroyExplosion();
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(0);


        }
    }
}