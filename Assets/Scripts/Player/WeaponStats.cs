using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    DualPistol,
    Shotgun,
    Sniper,
    Machine,

}
public class WeaponStats : MonoBehaviour
{
    public static WeaponStats Instance;
    private WeaponType currentWeapon = WeaponType.Pistol;
    public WeaponType CurrentWeapon
    {
        get {return currentWeapon;}
        set
        {
            currentWeapon = value;
            //When adding a new weapon, define its stats here when weapon type is set
            if (currentWeapon == WeaponType.Shotgun)
            {
                shotgunSprite.SetActive(true);
                ExtraBullets += 2;
                Spread = 45;
            }
        }
    }
    [SerializeField] private GameObject shotgunSprite;
    private int damage = 20;
    public int Damage
    {
        get {return damage;}
        set {damage = value;}
    }
    private int explosionSize = 0;
     public int ExplosionSize
    {
        get {return explosionSize;}
        set {explosionSize = value;}
    }
    private bool explosiveBullets = false;
    public bool ExplosiveBullets
    {
        get {return explosiveBullets;}
        set {explosiveBullets = value;}
    }
    private bool bleedTrue = false;
    public bool BleedTrue
    {
        get {return bleedTrue;}
        set {bleedTrue = value;}
    }
    private float critChance = 0.01f;
    public float CritChance
    {
        get {return critChance;}
        set {critChance = value;}
    }
    private int extraBullets = 0; //this is for the extra bullets spawned by the shotgun item - it should always be even
    public int ExtraBullets
    {
        get {return extraBullets;}
        set {extraBullets = value;}
    }
    private float spread = 0;
    public float Spread
    {
        get {return spread;}
        set {spread = value;}
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
}
