using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum WeaponType
{
    Pistol,
    Shotgun,
    Sniper,
    MachineGun,
    DualPistol,
    Sword,
    RocketLauncher
}

[System.Serializable]
public class Weapons
{
    public WeaponType weaponType;
    public GameObject Weapon;
}

public class WeaponStats : MonoBehaviour
{
    public static WeaponStats Instance;
    private WeaponType currentWeapon;

    [SerializeField] private Weapons[] weapons;

    public WeaponType CurrentWeapon
    {
        get { return currentWeapon; }
        set
        {
            currentWeapon = value;
            //When adding a new weapon, define its stats here when weapon type is set
            //If making adjustments to an unused stat please shift it up to the changed section instead of the base section
            swapWeapon(value);
            switch (value)
            {
                case WeaponType.Shotgun:
                    //Shotgun values
                    WeaponDamage = 50; //50% damage
                    WeaponRange = 33; //33% range
                    WeaponSpread = 30; //30 flat spread
                    WeaponExtraBullets = 6; //6 extra bullets
                    WeaponBulletSpeed = 150; //150% bullet speed
                    WeaponFireDelay = 150; //150% fire delay

                    //Other stats set to base values
                    weaponCritChancePercentage = 100;
                    WeaponCritChanceFlat = 0;
                    WeaponCritDamage = 100;
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    WeaponCameraSize = 0;
                    break;

                case WeaponType.Sniper:
                    //Sniper values
                    WeaponDamage = 300; //300% damage
                    weaponCritChancePercentage = 200; //200% crit chance scaling
                    WeaponCritChanceFlat = 5; //5% extra starting crit chance. This is added to the base crit chance of 1% and doubled by the sniper scaling. The sniper starts with 12% crit chance total
                    WeaponCritDamage = 200; //200% crit damage
                    WeaponRange = 200; //200% range
                    WeaponFireDelay = 200; //200% fire delay
                    WeaponBulletSpeed = 300; //300% bullet speed
                    WeaponCameraSize = 3; //3 extra camera size

                    //Other stats set to base values
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    break;

                case WeaponType.MachineGun:
                    //Machine gun values
                    WeaponDamage = 33; //33% damage
                    WeaponFireDelay = 20; //20% fire delay
                    WeaponBulletSpeed = 300; //300% bullet speed

                    //Other stats set to base values
                    weaponCritChancePercentage = 100;
                    WeaponCritChanceFlat = 0;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0; //Note: We may want to add spread to the machine gun?
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    WeaponCameraSize = 0;
                    break;

                case WeaponType.DualPistol:
                    //Dual pistol values
                    WeaponFireDelay = 50; //50% fire delay

                    //Other stats set to base values
                    WeaponDamage = 100;
                    weaponCritChancePercentage = 100;
                    WeaponCritChanceFlat = 0;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponBulletSpeed = 100;
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    WeaponCameraSize = 0;
                    break;

                case WeaponType.Pistol: //Defining the stats for the pistol so it can be swapped to and away from when testing
                    //All stats set to base values (mostly 100%)
                    WeaponDamage = 100;
                    weaponCritChancePercentage = 100;
                    WeaponCritChanceFlat = 0;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponFireDelay = 100;
                    WeaponBulletSpeed = 100;
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    WeaponCameraSize = 0;
                    break;

                case WeaponType.Sword:
                    //Sword Values
                    WeaponDamage = 150; //200% damage
                    WeaponFireDelay = 50; //50% fire delay

                    //Other stats set to base values
                    weaponCritChancePercentage = 100; //Possible extra crit chance or damage? Could be too powerful
                    WeaponCritChanceFlat = 0;
                    WeaponCritDamage = 100;
                    WeaponBleedDamage = 0;
                    WeaponCameraSize = 0;

                    //These stats are never used by the sword
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    WeaponExtraBullets = 0;
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponRange = 5;
                    WeaponSpread = 100;
                    WeaponBulletSpeed = 200; 
                    break;

                case WeaponType.RocketLauncher: //Placeholder for the rocket launcher
                    //Testing values for the Rocket Launcher. Focussing on getting it implemented. Can balance it later
                    WeaponExplosiveBullets = true; //Rocket launcher has explosive bullets
                    WeaponExplosionSize = 5; //Explosion size is 10
                    WeaponExplosionDamage = 100; //100% of the weapon's damage is dealt as explosion damage
                    WeaponFireDelay = 300; //300% fire delay
                    WeaponRange = 150; //150% range

                    //Other stats set to base values
                    WeaponDamage = 100;
                    weaponCritChancePercentage = 100;
                    WeaponCritChanceFlat = 0;
                    WeaponCritDamage = 100;
                    WeaponBulletSpeed = 100;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                    WeaponPiercing = false;
                    WeaponPierceAmount = 0;
                    WeaponCameraSize = 0;
                    break;

                default:
                    Debug.Log("Weapon Type does not exist");
                    break;
            }
        }
    }

    //All stats have a const base that doesn't change, a flat value that is changed by flat increases or decreases,
    //a percentage value that is changed by percentage increases or decreases, and a weapon percentage that is changed by the weapon's stats
    //The final value is calculated by adding the flat value to the base value and multiplying by the percentage values
    //It is only a getter, as only the components should be set

    //Damage
    private const int BASE_DAMAGE = 20; //Base character damage
    private int flatDamage = 0; //Flat damage increase
    public int FlatDamage
    {
        get { return flatDamage; }
        set { flatDamage = value; }
    }
    private int percentageDamage = 100; //Percentage starts at 100 (100%)
    public int PercentageDamage
    {
        get { return percentageDamage; }
        set { percentageDamage = value; }
    }
    private int weaponDamage = 100; //Weapon percentage starts at 100 (100%)
    public int WeaponDamage
    {
        get { return weaponDamage; }
        set { weaponDamage = value; }
    }
    public int Damage //Damage is only a getter, as only the components should be set
    {
        get { return ((BASE_DAMAGE + FlatDamage) * PercentageDamage * WeaponDamage) / 10000; } //Final damage calculation
    }

    //Crit Chance
    private const int BASE_CRIT_CHANCE = 1; //Base 1% crit chance
    private int flatCritChance = 0;
    public int FlatCritChance
    {
        get { return flatCritChance; }
        set { flatCritChance = value; }
    }
    private int percentageCritChance = 100;
    public int PercentageCritChance
    {
        get { return percentageCritChance; }
        set { percentageCritChance = value; }
    }
    private int weaponCritChanceFlat = 0;
    public int WeaponCritChanceFlat
    {
        get { return weaponCritChanceFlat; }
        set { weaponCritChanceFlat = value; }
    }
    private int weaponCritChancePercentage = 100;
    public int WeaponCritChance
    {
        get { return weaponCritChancePercentage; }
        set { weaponCritChancePercentage = value; }
    }
    public int CritChance
    {
        get { return ((BASE_CRIT_CHANCE + FlatCritChance + weaponCritChanceFlat) * PercentageCritChance * weaponCritChancePercentage) / 10000; }
    }

    private int excessCritChance() => Math.Max(CritChance - 100, 0); //Returns the amount of crit chance over 100

    //Crit Damage. Not currently changing but will likely be added to an item at some point
    private const int BASE_CRIT_DAMAGE = 150; //Base 150% crit damage (1.5x)
    private int flatCritDamage = 0;
    public int FlatCritDamage
    {
        get { return flatCritDamage; }
        set { flatCritDamage = value; }
    }
    private int percentageCritDamage = 100;
    public int PercentageCritDamage
    {
        get { return percentageCritDamage; }
        set { percentageCritDamage = value; }
    }
    private int weaponCritDamage = 100;
    public int WeaponCritDamage
    {
        get { return weaponCritDamage; }
        set { weaponCritDamage = value; }
    }
    public int CritDamage //Returns a percentage of the weapon's damage. 150 at base
    {
        get { return ((BASE_CRIT_DAMAGE + flatCritDamage + (excessCritChance() / 2)) * PercentageCritDamage * WeaponCritDamage) / 10000; } //Adds half of the excess crit chance to the crit damage
    }

    //Range
    private const int BASE_RANGE = 20;
    private int flatRange = 0;
    public int FlatRange
    {
        get { return flatRange; }
        set { flatRange = value; }
    }
    private int percentageRange = 100;
    public int PercentageRange
    {
        get { return percentageRange; }
        set { percentageRange = value; }
    }
    private int weaponRange = 100;
    public int WeaponRange
    {
        get { return weaponRange; }
        set { weaponRange = value; }
    }
    public int Range
    {
        get { return ((BASE_RANGE + FlatRange) * PercentageRange * WeaponRange) / 10000; }
    }

    //Fire delay. Note: .5f is a half second delay between shots. Increasing the fire delay will make the weapon shoot slower
    private const float BASE_FIRE_DELAY = 0.5f;
    private float flatFireDelay = 0;
    public float FlatFireDelay
    {
        get { return flatFireDelay; }
        set { flatFireDelay = value; }
    }
    private float percentageFireDelay = 100f;
    public float PercentageFireDelay
    {
        get { return percentageFireDelay; }
        set { percentageFireDelay = value; }
    }
    private float weaponFireDelay = 100f;
    public float WeaponFireDelay
    {
        get { return weaponFireDelay; }
        set { weaponFireDelay = value; }
    }
    public float FireDelay
    {
        get { return Math.Max(((BASE_FIRE_DELAY + FlatFireDelay) * PercentageFireDelay * WeaponFireDelay) / 10000, 0.1f); }
        //This isn't allowed to be any quicker than 0.1 seconds per shot. Can change value?
    }

    //Bullet speed
    private const float BASE_BULLET_SPEED = 50f;
    private float flatBulletSpeed = 0;
    public float FlatBulletSpeed
    {
        get { return flatBulletSpeed; }
        set { flatBulletSpeed = value; }
    }
    private float percentageBulletSpeed = 100;
    public float PercentageBulletSpeed
    {
        get { return percentageBulletSpeed; }
        set { percentageBulletSpeed = value; }
    }
    private float weaponBulletSpeed = 100;
    public float WeaponBulletSpeed
    {
        get { return weaponBulletSpeed; }
        set { weaponBulletSpeed = value; }
    }
    public float BulletSpeed
    {
        get { return ((BASE_BULLET_SPEED + FlatBulletSpeed) * PercentageBulletSpeed * WeaponBulletSpeed) / 10000; }
    }

    //The explosive bullets item and the rocket launcher are going to have the same effect with different values so I will combine them
    //The explosion will have a bool true, a size, and a damage value. The damage value will be a percentage of the bullet's damage

    //Explosive bullets
    private bool itemExplosiveBullets = false;
    public bool ItemExplosiveBullets
    {
        get { return itemExplosiveBullets; }
        set { itemExplosiveBullets = value; }
    }
    private bool weaponExplosiveBullets = false;
    public bool WeaponExplosiveBullets
    {
        get { return weaponExplosiveBullets; }
        set { weaponExplosiveBullets = value; }
    }
    public bool ExplosiveBullets
    {
        get { return ItemExplosiveBullets || WeaponExplosiveBullets; } //This will return true if either the weapon or an item has explosive bullets
    }
    private bool selfDamageExplosions = false;
    public bool SelfDamageExplosions
    {
        get {return selfDamageExplosions;}
        set {selfDamageExplosions = value;}
    }

    //Explosion size
    private int itemExplosionSize = 0;
    public int ItemExplosionSize
    {
        get { return itemExplosionSize; }
        set { itemExplosionSize = value; }
    }
    private int weaponExplosionSize = 0;
    public int WeaponExplosionSize
    {
        get { return weaponExplosionSize; }
        set { weaponExplosionSize = value; }
    }
    private int percentageExplosionSize = 100;
    public int PercentageExplosionSize
    {
        get { return percentageExplosionSize; }
        set { percentageExplosionSize = value; }
    }
    public int ExplosionSize
    {
        get { return ((ItemExplosionSize + WeaponExplosionSize) * percentageExplosionSize) / 100; } //This will return the sum of the item and weapon explosion sizes
    }

    //Explosion damage
    private int itemExplosionDamage = 0; //Percentage of the weapon damage to be dealt as explosion damage
    public int ItemExplosionDamage
    {
        get { return itemExplosionDamage; }
        set { itemExplosionDamage = value; }
    }
    private int weaponExplosionDamage = 0; //Percentage of the weapon damage to be dealt as explosion damage
    public int WeaponExplosionDamage
    {
        get { return weaponExplosionDamage; }
        set { weaponExplosionDamage = value; }
    }
    public int ExplosionDamage
    {
        get { return (Damage * (ItemExplosionDamage + WeaponExplosionDamage)) / 100; } //This gives explosion damage as a percentage of the weapon's damage equal to the item % and the weapon % added together
    }

    //Radioactive bombs
    private bool itemRadioactive = false;
    public bool ItemRadioactive
    {
        get {return itemRadioactive;}
        set {itemRadioactive = true;}
    }
    private int radiationDamage = 0;
    public int RadiationDamagePercentage
    {
        get {return radiationDamage;}
        set {radiationDamage = value;}
    }
    public int RadiationDamage
    {
        get {return (Damage * RadiationDamagePercentage) / 100;} //Radiation damage is a % of weapon damage that increases when the item is taken
    }

    //Bleed damage
    private int itemBleedDamage = 0; //Percentage of the weapon damage to be dealt as bleed damage
    public int ItemBleedDamage
    {
        get { return itemBleedDamage; }
        set { itemBleedDamage = value; }
    }
    private int weaponBleedDamage = 0;
    public int WeaponBleedDamage
    {
        get { return weaponBleedDamage; } //Adding this in case we want a weapon to have bleed damage up at some point
        set { weaponBleedDamage = value; }
    }
    public int BleedDamage
    {
        get { return ItemBleedDamage + WeaponBleedDamage; } //This gives bleed damage as a percentage of the enemy health equal to the item % and the weapon % added together
    }

    //Extra bullets - At the moment this is just changed for the shotgun and not any items but having it here for future proofing in case it changes in an upgrade
    //Bullets need to always return an even number so it rounds up
    private int itemExtraBullets = 0;
    public int ItemExtraBullets
    {
        get { return itemExtraBullets; }
        set { itemExtraBullets = value; }
    }
    private int weaponExtraBullets = 0;
    public int WeaponExtraBullets
    {
        get { return weaponExtraBullets; }
        set { weaponExtraBullets = value; }
    }
    public int ExtraBullets
    {
        get { return (ItemExtraBullets + WeaponExtraBullets) + ((ItemExtraBullets + WeaponExtraBullets) % 2); } //Sum of the item and weapon extra bullets plus 1 if its an odd number
    }

    //Spread - This is only used for the shotgun. Can be changed by items
    private float itemSpread = 0;
    public float ItemSpread
    {
        get { return itemSpread; }
        set { itemSpread = value; }
    }
    private float weaponSpread = 0;
    public float WeaponSpread
    {
        get { return weaponSpread; }
        set
        { weaponSpread = value; }
    }
    public float Spread
    {
        get
        {
            return ItemSpread + WeaponSpread;
        }
    }

    //Piercing
    private bool itemPiercing = false;
    public bool ItemPiercing
    {
        get { return itemPiercing; }
        set { itemPiercing = value; }
    }
    private bool weaponPiercing = false;
    public bool WeaponPiercing
    {
        get { return weaponPiercing; }
        set { weaponPiercing = value; }
    }
    public bool Piercing
    {
        get { return ItemPiercing || WeaponPiercing; }
    }

    //Pierce amount. Each point of pierce amount allows the bullet to pierce one enemy. If it is -1 then it pierces all enemies
    private int itemPierceAmount = 0;
    public int ItemPierceAmount
    {
        get { return itemPierceAmount; }
        set { itemPierceAmount = value; }
    }
    private int weaponPierceAmount = 0;
    public int WeaponPierceAmount
    {
        get { return weaponPierceAmount; }
        set { weaponPierceAmount = value; }
    }
    public int PierceAmount
    {
        get
        {
            if (WeaponPierceAmount == -1 || ItemPierceAmount == -1)
            {
                return -1;
            }
            return ItemPierceAmount + WeaponPierceAmount;
        }
    }

    private int ricochetCount = 0;
    public int RicochetCount
    {
        get { return ricochetCount; }
        set { ricochetCount = value; }
    }

    private float BASE_CAMERA_SIZE = 10f;
    private float weaponCameraSize = 0;
    public float WeaponCameraSize
    {
        get { return weaponCameraSize; }
        set 
        { 
            weaponCameraSize = value;
            StartCoroutine(CameraResize(CameraSize, 1f));
        }
    }
    public float CameraSize
    {
        get { return BASE_CAMERA_SIZE + weaponCameraSize; }
    }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        StartCoroutine(CameraResize(CameraSize));
    }

    void Start()
    {
        CurrentWeapon = WeaponType.Pistol; //Sets the weapon to the pistol by default
    }

    private void swapWeapon(WeaponType newWeapon)
    {
        foreach (Weapons weapon in weapons)
        {
            if (weapon.weaponType == newWeapon)
            {
                weapon.Weapon.SetActive(true);
            }
            else
            {
                weapon.Weapon.SetActive(false);
            }
        }
    }

    public IEnumerator CameraResize(float newSize, float time = 0f) //Default is instant
    {
        float elapsedTime = 0;
        float startingSize = Camera.main.orthographicSize;
        while (elapsedTime < time)
        {
            Camera.main.orthographicSize = Mathf.Lerp(startingSize, newSize, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.orthographicSize = newSize;
    }
}
