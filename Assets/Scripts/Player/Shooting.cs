using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;

    public GameObject bullet;
    public Transform sprite;
    public Transform firePoint;
    private float bulletSpeed = 50;

    private float lastShot = 0;
    private bool held = false;
    private float firerate = 0.5f;
    public float Firerate
    {
        get {return firerate;}
        set {firerate = value;}
    }

    Vector2 lookDirection;
    float lookAngle;

    void Start()
    {
        //This lets the player shoot immediately when the game starts
        lastShot = Time.time - firerate;
    }

    public void ShootingTest()
    {
        // Capture mouse position and calculate shooting direction
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = new Vector2(lookDirection.x - transform.position.x, lookDirection.y - transform.position.y);
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Rotate firePoint towards mouse position
        firePoint.rotation = Quaternion.Euler(0, 0, lookAngle);

        // Instantiate bullet clone and set its position and rotation
        GameObject bulletClone = Instantiate(bullet);
        bulletClone.transform.position = firePoint.position;
        bulletClone.transform.rotation = Quaternion.Euler(0, 0, lookAngle);

        // Apply velocity to the bullet clone
        bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * bulletSpeed;
    }

     void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame){return;}
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = new Vector2(lookDirection.x - transform.position.x, lookDirection.y - transform.position.y);
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        sprite.rotation = Quaternion.Euler(0, 0, lookAngle);
        if (held && Time.time - lastShot > firerate)
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
        if (PlayerStats.Instance.HasShotgun)
        {
            //shoot 1+stacks(2) bullets in a cone infront of the player
            float shotAngle = (PlayerStats.Instance.BulletAmount / 2) * 10;
            for (int i = 0; i < PlayerStats.Instance.BulletAmount + 1; i++)
            {
                firePoint.rotation = Quaternion.Euler(0, 0, lookAngle + shotAngle);
                GameObject bulletClone = Instantiate(bullet, firePoint.position, firePoint.rotation);
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