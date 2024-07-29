using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform sprite;
    public Transform firePoint;
    public float bulletSpeed = 50;

    public static float firerate = 0.5f;
    public float shootingInterval = 0;

    Vector2 lookDirection;
    float lookAngle;

    void Update()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = new Vector2(lookDirection.x - transform.position.x, lookDirection.y - transform.position.y);
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        sprite.rotation = Quaternion.Euler(0, 0, lookAngle);
        HandleShooting();
    }
    void HandleShooting()
    {
        shootingInterval -= Time.deltaTime;
        if (shootingInterval <= 0 && Input.GetMouseButton(0))
        {
            shootingInterval = firerate; 
            Shoot();
        }
    }

    private void Shoot()
    {
        if (PlayerHealth.hasShotgun)
        {
            //shoot 1+stacks(2) bullets in a cone infront of the player
            float shotAngle = 10f;
            for (int i = 0; i < PlayerHealth.bulletAmount + 1; i++)
            {
                firePoint.rotation = Quaternion.Euler(0, 0, lookAngle + shotAngle);
                GameObject bulletClone = Instantiate(bullet, firePoint.position, Quaternion.Euler(0, 0, lookAngle));
                bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * bulletSpeed;
                shotAngle -= 10f;
            }
        }
        else
        {
            GameObject bulletClone = Instantiate(bullet, firePoint.position, Quaternion.Euler(0, 0, lookAngle));
            bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * bulletSpeed;
        }
        // Play the duck shooting sound
        SFXManager.Instance.DuckShootSound(); 
    }
}