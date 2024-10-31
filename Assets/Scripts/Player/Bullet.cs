using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

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
    private int ricochetCount;
    public RiotShield riotShield;

    void Start()
    {
        startPos = transform.position;

        //Calculates critical roll
        if (UnityEngine.Random.Range(0, 100) < WeaponStats.Instance.CritChance)
        {
            crit = true;
        }

        //Set sprite
        if (crit)
        {
            if (WeaponStats.Instance.CurrentWeapon == WeaponType.RocketLauncher)
            {
                transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            if (WeaponStats.Instance.CurrentWeapon == WeaponType.RocketLauncher)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (WeaponStats.Instance.Piercing)
        {
            pierceCount = WeaponStats.Instance.PierceAmount; //NOTE: If this is -1, it will pierce infinitely. Otherwise, it will pierce the number of times specified
        }
        bulletCollider = GetComponent<Collider2D>();
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletDirection = bulletRigidbody.velocity;
        bulletSpeed = bulletRigidbody.velocity.magnitude;
        ricochetCount = WeaponStats.Instance.RicochetCount;
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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Shield"))
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
            if (other.gameObject.CompareTag("Shield"))
            {
                other.gameObject.GetComponent<RiotShield>().TakeDamage();
                Destroy(gameObject);
            }
            //Making the rocket launcher not deal base bullet damage, only explosion damage
            else if (WeaponStats.Instance.CurrentWeapon != WeaponType.RocketLauncher)
            {
                if (crit)
                {
                    other.gameObject.GetComponent<EnemyBase>().ReceiveDamage((WeaponStats.Instance.Damage * WeaponStats.Instance.CritDamage) / 100, true);
                }
                else
                {
                    other.gameObject.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.Damage, false);
                }
                //If the piercing isn't infinite, check if it should pierce
                if (pierceCount != -1)
                {
                    if (pierceCount == 0) //If count isn't zero then reduce the pierce count
                    {
                        if (ricochetCount > 0) //If the bullet should ricochet and hasn't already ricocheted
                        {
                            //Ricochet in a random direction
                            bulletDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                            ricochetCount--;
                        }
                        else //Otherwise destroy the bullet
                        {
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        pierceCount--;
                    }
                }
                //Disables collisions with the collided
                Physics2D.IgnoreCollision(bulletCollider, other.collider);
                //Set bullet to old (or new for ricochet) speed and direction
                bulletRigidbody.velocity = bulletDirection;
                bulletRigidbody.velocity = bulletRigidbody.velocity.normalized * bulletSpeed;
                transform.right = bulletRigidbody.velocity;
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

        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
