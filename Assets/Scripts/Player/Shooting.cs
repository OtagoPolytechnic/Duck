using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform sprite;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform dualFirePoint;
    private bool dualShot = false;
    private float lastShot = 0;
    private bool held = false;

    Vector2 lookDirection;
    float lookAngle;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        //This lets the player shoot immediately when the game starts
        lastShot = Time.time - WeaponStats.Instance.Firerate;
    }


    void Update()
    {
        if (GameSettings.gameState != GameState.InGame){return;}
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = new Vector2(lookDirection.x - transform.position.x, lookDirection.y - transform.position.y);
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        sprite.rotation = Quaternion.Euler(0, 0, lookAngle);
        if (held && Time.time - lastShot > WeaponStats.Instance.Firerate)
        {
            lastShot = Time.time;
            Shoot();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            held = true;
        }
        else if (context.canceled)
        {
            held = false;
        }
    }

    private void Shoot()
    {
        if (WeaponStats.Instance.Spread > 0)
        {
            //shoot 1+stacks(2) bullets in a cone infront of the player
            float shotAngle = -(WeaponStats.Instance.Spread / 2);
            for (int i = 0; i < WeaponStats.Instance.ExtraBullets + 1; i++)
            {
                firePoint.rotation = Quaternion.Euler(0, 0, lookAngle + shotAngle);
                GameObject bulletClone = Instantiate(bullet, firePoint.position, firePoint.rotation);
                bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * WeaponStats.Instance.BulletSpeed;
                shotAngle += WeaponStats.Instance.Spread/WeaponStats.Instance.ExtraBullets; 
            }
        }
        else if (WeaponStats.Instance.CurrentWeapon == WeaponType.DualPistol)
        {
            if (dualShot)
            {
                GameObject bulletClone = Instantiate(bullet, dualFirePoint.position, Quaternion.Euler(0, 0, lookAngle));
                bulletClone.GetComponent<Rigidbody2D>().velocity = dualFirePoint.right * WeaponStats.Instance.BulletSpeed;
            }
            else
            {
                GameObject bulletClone = Instantiate(bullet, firePoint.position, Quaternion.Euler(0, 0, lookAngle));
                bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * WeaponStats.Instance.BulletSpeed;
            }

            dualShot = !dualShot;
        }
        else
        {
            GameObject bulletClone = Instantiate(bullet, firePoint.position, Quaternion.Euler(0, 0, lookAngle));
            bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * WeaponStats.Instance.BulletSpeed;
        }
        // Play the duck shooting sound
        SFXManager.Instance.DuckShootSound(); 
    }
}