using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private Vector3 startPos;
    private bool crit = false;
    private int pierceCount = 0;
    private Collider2D bulletCollider;
    private Rigidbody2D bulletRigidbody;
    private Vector2 bulletDirection;
    private float bulletSpeed;
    public GameObject ExplosionPrefab;
    private GameObject Explosion;

    void Start()
    {
        startPos = transform.position;

        //Calculates critical roll
        if (UnityEngine.Random.Range(0, 100) < WeaponStats.Instance.CritChance)
        {
            crit = true;
            //Change to critical sprite
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (WeaponStats.Instance.Piercing)
        {
            pierceCount = WeaponStats.Instance.PierceAmount; //NOTE: If this is -1, it will pierce infinitely. Otherwise, it will pierce the number of times specified
        }
        bulletCollider = GetComponent<Collider2D>();
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletDirection = bulletRigidbody.velocity;
        bulletSpeed = bulletRigidbody.velocity.magnitude;

    }

    void Update()
    {
        //Destroys bullet after it reaches max range
        if (Vector3.Distance(startPos, transform.position) > (float)WeaponStats.Instance.Range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //destroys bullet on hit with player and lowers health
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (WeaponStats.Instance.ExplosiveBullets)
            {
                Explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                PlayerExplosion explosionScript = Explosion.GetComponent<PlayerExplosion>();
                explosionScript.ExplosionSize = WeaponStats.Instance.ExplosionSize;
                explosionScript.Crit = crit;
                if (crit)
                {
                    explosionScript.ExplosionDamage = (WeaponStats.Instance.ExplosionDamage * WeaponStats.Instance.CritDamage) / 100;
                }
                else
                {
                    explosionScript.ExplosionDamage = WeaponStats.Instance.ExplosionDamage;
                }
            }
            //Making the rocket launcher not deal base bullet damage, only explosion damage
            if (WeaponStats.Instance.CurrentWeapon != WeaponType.RocketLauncher)
            {
                //Lifesteal by percentage of damage dealt
                if (PlayerStats.Instance.LifestealPercentage > 0)
                {
                    PlayerStats.Instance.CurrentHealth += Math.Max((WeaponStats.Instance.Damage * PlayerStats.Instance.LifestealPercentage) / 100, 1); //Heals at least 1 health
                }
                if (crit)
                {
                    other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage((WeaponStats.Instance.Damage * WeaponStats.Instance.CritDamage) / 100, true);
                }
                else
                {
                    other.gameObject.GetComponent<EnemyHealth>().ReceiveDamage(WeaponStats.Instance.Damage, false);
                }
                if (pierceCount != -1)
                {
                    if (pierceCount == 0)
                    {
                        Destroy(gameObject);
                    }
                    pierceCount--;
                }
                //Disables collisions with the collided
                Physics2D.IgnoreCollision(bulletCollider, other.collider);
                //Set bullet to old speed and direction
                bulletRigidbody.velocity = bulletDirection;
                bulletRigidbody.velocity = bulletRigidbody.velocity.normalized * bulletSpeed;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Edges"))
        {
            Destroy(gameObject);
        }
    }
}
