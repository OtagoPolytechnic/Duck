using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject swordAttack;
    [SerializeField] private GameObject swordBeam;
    [SerializeField] private Transform sprite;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform dualFirePoint;
    private bool dualShot = false;
    private float lastShot = 0;
    private float reflectCooldown;
    private bool toggled = false;
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
        lastShot = Time.time - WeaponStats.Instance.FireDelay;
    }


    void Update()
    {
        if (GameSettings.gameState != GameState.InGame){return;}

        if(GameSettings.controlType == controlType.Keyboard)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            lookDirection = new Vector2(worldPosition.x - transform.position.x, worldPosition.y - transform.position.y);
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        }

        sprite.rotation = Quaternion.Euler(0, 0, lookAngle);
        if (Time.time - lastShot > WeaponStats.Instance.FireDelay)
        {
            if ((!GameSettings.toggleShoot && held) //If toggle shoot off and held is true
                || (GameSettings.toggleShoot && toggled)) //If toggle shoot on and toggled is true
            {
                lastShot = Time.time;
                Shoot();
            }
        }

        if (reflectCooldown > 0)
        {
            reflectCooldown -= Time.deltaTime;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (GameSettings.controlType != controlType.Controller)
        {
            return;
        }
        if (context.performed)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            if (lookInput.sqrMagnitude > 0.01f)
            {
                lookAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
            }
        }
    }

    public void OnArcadeLook(InputAction.CallbackContext context)
    {
        if (GameSettings.controlType != controlType.Arcade)
        {
            return;
        }
        if (context.performed)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            if (lookInput.sqrMagnitude > 0.01f)
            {
                lookAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (GameSettings.toggleShoot)
        {
            if (context.performed)
            {
                toggled = !toggled;
            }
            return;
        }
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
        if (WeaponStats.Instance.CurrentWeapon == WeaponType.Sword)
        {
            if (WeaponStats.Instance.HasSwordBeam && PlayerStats.Instance.CurrentHealth == PlayerStats.Instance.MaxHealth)
            {
                GameObject swordBeamClone = Instantiate(swordBeam, swordAttack.transform.position, Quaternion.Euler(0, 0, lookAngle));
                swordBeamClone.GetComponent<Rigidbody2D>().velocity = lookDirection * WeaponStats.Instance.BulletSpeed;
            }
            if (Random.Range(0, 100) < WeaponStats.Instance.CritChance)
            {
                swordAttack.GetComponent<Sword>().Crit = true;
            }
            else
            {
                swordAttack.GetComponent<Sword>().Crit = false;
            }
            StartCoroutine(SwordAttack());
            return;
        }
        if (WeaponStats.Instance.Spread > 0)
        {
            //shoot 1+stacks(2) bullets in a cone infront of the player
            float shotAngle = -(WeaponStats.Instance.Spread / 2);
            for (int i = 0; i < WeaponStats.Instance.ExtraBullets + 1; i++)
            {
                firePoint.rotation = Quaternion.Euler(0, 0, lookAngle + shotAngle);    
                FireBullet(firePoint);
                shotAngle += WeaponStats.Instance.Spread/WeaponStats.Instance.ExtraBullets; 
            }
        }
        else if (WeaponStats.Instance.CurrentWeapon == WeaponType.DualPistol)
        {
            if (dualShot)
            {
                FireBullet(dualFirePoint);
            }
            else
            {
                FireBullet(firePoint);
            }

            dualShot = !dualShot;
        }
        else
        {
            FireBullet(firePoint);
        }
        // Play the duck shooting sound
        SFXManager.Instance.PlaySFX("DuckShooting");
    }

    private void FireBullet(Transform bulletFirePoint)
    {
        GameObject bulletClone = Instantiate(bullet, bulletFirePoint.position, Quaternion.Euler(0, 0, lookAngle));
        bulletClone.GetComponent<Rigidbody2D>().velocity = bulletFirePoint.right * WeaponStats.Instance.BulletSpeed;
    }

    IEnumerator SwordAttack()
    {
        swordAttack.SetActive(true);
        if (WeaponStats.Instance.HasReflector)
        {
            if (reflectCooldown <= 0)
            {
                swordAttack.GetComponent<Sword>().Reflecting = true;
            }
        }

        if (swordAttack.GetComponent<Sword>().Crit) //Activate either crit or non crit sprite
        {
            swordAttack.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            swordAttack.transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(WeaponStats.Instance.FireDelay/2);

        swordAttack.SetActive(false);

        swordAttack.transform.GetChild(0).gameObject.SetActive(false); //Set sprites not active
        swordAttack.transform.GetChild(1).gameObject.SetActive(false);


        if (swordAttack.GetComponent<Sword>().Reflecting)
        {
            swordAttack.GetComponent<Sword>().Reflecting = false;
            reflectCooldown = WeaponStats.Instance.ReflectCooldown;
        }
        StopCoroutine(SwordAttack());
    }
}