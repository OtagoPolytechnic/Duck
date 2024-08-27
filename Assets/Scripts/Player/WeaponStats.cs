using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
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
public class WeaponStats : MonoBehaviour
{
    public static WeaponStats Instance;
    private WeaponType currentWeapon;
    public WeaponType CurrentWeapon
    {
        get {return currentWeapon;}
        set
        {
            currentWeapon = value;
            //When adding a new weapon, define its stats here when weapon type is set
            //If making adjustments to an unused stat please shift it up to the changed section instead of the base section
            switch(value)
            {
                case WeaponType.Shotgun:
                    weaponSprites[0].SetActive(true);

                    //Shotgun values
                    WeaponDamage = 50; //50% damage
                    WeaponRange = 33; //33% range
                    WeaponSpread = 30; //30 flat spread
                    WeaponExtraBullets = 6; //6 extra bullets
                    WeaponBulletSpeed = 150; //150% bullet speed
                    WeaponFireDelay= 150; //150% fire delay

                    //Other stats set to base values
                    WeaponCritChance = 100;
                    WeaponCritDamage = 100;
                    WeaponExplosiveBullets = false;
                    WeaponBleed = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                break;
                
                case WeaponType.Sniper:
                    weaponSprites[1].SetActive(true);

                    //Sniper values
                    WeaponDamage = 300; //300% damage
                    WeaponCritChance = 200; //200% crit chance
                    WeaponCritDamage = 200; //200% crit damage
                    WeaponRange = 200; //200% range
                    WeaponFireDelay = 200; //200% fire delay
                    WeaponBulletSpeed = 300; //300% bullet speed

                    //Other stats set to base values
                    WeaponExplosiveBullets = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleed = false;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                break;

                case WeaponType.MachineGun:
                    weaponSprites[2].SetActive(true);

                    //Machine gun values
                    WeaponDamage = 33; //33% damage
                    WeaponFireDelay = 20; //20% fire delay
                    WeaponBulletSpeed = 300; //300% bullet speed

                    //Other stats set to base values
                    WeaponCritChance = 100;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponExplosiveBullets = false;
                    WeaponBleed = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0; //Note: We may want to add spread to the machine gun?
                break;

                case WeaponType.DualPistol:
                    weaponSprites[3].SetActive(true);

                    //Dual pistol values
                    WeaponFireDelay = 50; //50% fire delay

                    //Other stats set to base values
                    WeaponDamage = 100;
                    WeaponCritChance = 100;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponBulletSpeed = 100;
                    WeaponExplosiveBullets = false;
                    WeaponBleed = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                break;

                case WeaponType.Pistol: //Defining the stats for the pistol so it can be swapped to and away from when testing
                    weaponSprites[4].SetActive(true);
                    //All stats set to base values (mostly 100%)
                    WeaponDamage = 100;
                    WeaponCritChance = 100;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponFireDelay = 100;
                    WeaponBulletSpeed = 100;
                    WeaponExplosiveBullets = false;
                    WeaponBleed = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                break;

                case WeaponType.Sword: //Placeholder for the sword
                    weaponSprites[5].SetActive(true);

                    //Setting all stats to base values until we figure out how to implement the sword
                    WeaponDamage = 100;
                    WeaponCritChance = 100;
                    WeaponCritDamage = 100;
                    WeaponRange = 100;
                    WeaponFireDelay = 100;
                    WeaponBulletSpeed = 100;
                    WeaponExplosiveBullets = false;
                    WeaponBleed = false;
                    WeaponExplosionSize = 0;
                    WeaponExplosionDamage = 0;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                break;

                case WeaponType.RocketLauncher: //Placeholder for the rocket launcher
                    weaponSprites[6].SetActive(true);
                    //Testing values for the Rocket Launcher. Focussing on getting it implemented. Can balance it later
                    WeaponExplosiveBullets = true; //Rocket launcher has explosive bullets
                    WeaponExplosionSize = 100; //Explosion size is 100
                    WeaponExplosionDamage = 50; //50% of the weapon's damage is dealt as explosion damage
                    WeaponFireDelay = 300; //300% fire delay
                    WeaponRange = 200; //200% range

                    //Other stats set to base values
                    WeaponDamage = 100;
                    WeaponCritChance = 100;
                    WeaponCritDamage = 100;
                    WeaponBulletSpeed = 100;
                    WeaponBleed = false;
                    WeaponBleedDamage = 0;
                    WeaponExtraBullets = 0;
                    WeaponSpread = 0;
                break;

                default:
                    Debug.Log("Weapon Type does not exist");
                break;
            }
        }
    }
    [SerializeField] private GameObject[] weaponSprites;

    //All stats have a const base that doesn't change, a flat value that is changed by flat increases or decreases,
    //a percentage value that is changed by percentage increases or decreases, and a weapon percentage that is changed by the weapon's stats
    //The final value is calculated by adding the flat value to the base value and multiplying by the percentage values
    //It is only a getter, as only the components should be set

    //Damage
    private const int BASE_DAMAGE = 20; //Base character damage
    private int flatDamage = 0; //Flat damage increase
    public int FlatDamage
    {
        get {return flatDamage;}
        set {flatDamage = value;}
    }
    private int percentageDamage = 100; //Percentage starts at 100 (100%)
    public int PercentageDamage
    {
        get {return percentageDamage;}
        set {percentageDamage = value;}
    }
    private int weaponDamage = 100; //Weapon percentage starts at 100 (100%)
    public int WeaponDamage
    {
        get {return weaponDamage;}
        set {weaponDamage = value;}
    }
    public int Damage //Damage is only a getter, as only the components should be set
    {
        get {return (BASE_DAMAGE + FlatDamage) * (PercentageDamage / 100) * (WeaponDamage / 100);} //Final damage calculation
    }

    //Crit Chance
    private const int BASE_CRIT_CHANCE = 1; //Base 1% crit chance
    private int flatCritChance = 0;
    public int FlatCritChance
    {
        get {return flatCritChance;}
        set {flatCritChance = value;}
    }
    private int percentageCritChance = 100;
    public int PercentageCritChance
    {
        get {return percentageCritChance;}
        set {percentageCritChance = value;}
    }
    private int weaponCritChance = 100;
    public int WeaponCritChance
    {
        get {return weaponCritChance;}
        set {weaponCritChance = value;}
    }
    public int CritChance
    {
        get {return (BASE_CRIT_CHANCE + FlatCritChance) * (PercentageCritChance / 100) * (WeaponPercentage / 100);}
    }

    //Crit Damage. Not currently changing but will likely be added to an item at some point
    private const int BASE_CRIT_DAMAGE = 150; //Base 150% crit damage (1.5x)
    private int flatCritDamage = 0;
    public int FlatCritDamage
    {
        get {return flatCritDamage;}
        set {flatCritDamage = value;}
    }
    private int percentageCritDamage = 100;
    public int PercentageCritDamage
    {
        get {return percentageCritDamage;}
        set {percentageCritDamage = value;}
    }
    private int weaponCritDamage = 100;
    public int WeaponCritDamage
    {
        get {return weaponCritDamage;}
        set {weaponCritDamage = value;}
    }
    public int CritDamage //Returns a percentage of the weapon's damage. 150 at base
    {
        get {return (BASE_CRIT_DAMAGE + flatCritDamage) * (PercentageCritDamage / 100) * (WeaponCritDamage / 100);}
    }

    //Range
    private const int BASE_RANGE = 20;
    private int flatRange = 0;
    public int FlatRange
    {
        get {return flatRange;}
        set {flatRange = value;}
    }
    private int percentageRange = 100;
    public int PercentageRange
    {
        get {return percentageRange;}
        set {percentageRange = value;}
    }
    private int weaponRange = 100;
    public int WeaponRange
    {
        get {return weaponRange;}
        set {weaponRange = value;}
    }
    public int Range
    {
        get {return (BASE_RANGE + FlatRange) * (PercentageRange / 100) * (WeaponRange / 100);}
    }

    //Fire delay. Note: .5f is a half second delay between shots. Increasing the fire delay will make the weapon shoot slower
    private const float BASE_FIRE_DELAY = 0.5f;
    private float flatFireDelay = 0;
    public float FlatFireDelay
    {
        get {return flatFireDelay;}
        set {flatFireDelay = value;}
    }
    private float percentageFireDelay = 100;
    public float PercentageFireDelay
    {
        get {return percentageFireDelay;}
        set {percentageFireDelay = value;}
    }
    private float weaponFireDelay = 100;
    public float WeaponFireDelay
    {
        get {return weaponFireDelay;}
        set {weaponFireDelay = value;}
    }
    public float FireDelay
    {
        get {return (BASE_FIRE_DELAY + FlatFireDelay) * (PercentageFireDelay / 100) * (WeaponFireDelay / 100);}
    }
    
    //Bullet speed
    private const float BASE_BULLET_SPEED = 50f;
    private float flatBulletSpeed = 0;
    public float FlatBulletSpeed
    {
        get {return flatBulletSpeed;}
        set {flatBulletSpeed = value;}
    }
    private float percentageBulletSpeed = 100;
    public float PercentageBulletSpeed
    {
        get {return percentageBulletSpeed;}
        set {percentageBulletSpeed = value;}
    }
    private float weaponBulletSpeed = 100;
    public float WeaponBulletSpeed
    {
        get {return weaponBulletSpeed;}
        set {weaponBulletSpeed = value;}
    }
    public float BulletSpeed
    {
        get {return (BASE_BULLET_SPEED + FlatBulletSpeed) * (PercentageBulletSpeed / 100) * (WeaponBulletSpeed / 100);}
    }

    //The explosive bullets item and the rocket launcher are going to have the same effect with different values so I will combine them
    //The explosion will have a bool true, a size, and a damage value. The damage value will be a percentage of the bullet's damage
    
    //Explosive bullets
    private bool itemExplosiveBullets = false;
    public bool ItemExplosiveBullets
    {
        get {return itemExplosiveBullets;}
        set {itemExplosiveBullets = value;}
    }
    private bool weaponExplosiveBullets = false;
    public bool WeaponExplosiveBullets
    {
        get {return weaponExplosiveBullets;}
        set {weaponExplosiveBullets = value;}
    }
    public bool ExplosiveBullets
    {
        get {return ItemExplosiveBullets || WeaponExplosiveBullets;} //This will return true if either the weapon or an item has explosive bullets
    }

    //Explosion size
    private int itemExplosionSize = 0;
    public int ItemExplosionSize
    {
        get {return itemExplosionSize;}
        set {itemExplosionSize = value;}
    }
    private int weaponExplosionSize = 0;
    public int WeaponExplosionSize
    {
        get {return weaponExplosionSize;}
        set {weaponExplosionSize = value;}
    }
    public int ExplosionSize
    {
        get {return ItemExplosionSize + WeaponExplosionSize;} //This will return the sum of the item and weapon explosion sizes
    }

    //Explosion damage
    private int itemExplosionDamage = 0; //Percentage of the weapon damage to be dealt as explosion damage
    public int ItemExplosionDamage
    {
        get {return itemExplosionDamage;}
        set {itemExplosionDamage = value;}
    }
    private int weaponExplosionDamage = 0; //Percentage of the weapon damage to be dealt as explosion damage
    public int WeaponExplosionDamage
    {
        get {return weaponExplosionDamage;}
        set {weaponExplosionDamage = value;}
    }
    public int ExplosionDamage
    {
        get {return (Damage * ((ItemExplosionDamage + WeaponExplosionDamage) / 100));} //This gives explosion damage as a percentage of the weapon's damage equal to the item % and the weapon % added together
    }

    //Bleed
    private bool itemBleed = false;
    public bool ItemBleed
    {
        get {return itemBleed;}
        set {itemBleed = value;}
    }
    private bool weaponBleed = false; //Adding this in case we want a weapon to have bleed at some point
    public bool WeaponBleed
    {
        get {return weaponBleed;}
        set {weaponBleed = value;}
    }
    public bool Bleed
    {
        get {return ItemBleed || WeaponBleed;} //This will return true if either the weapon or an item has bleed
    }

    //Bleed damage
    private int itemBleedDamage = 0; //Percentage of the weapon damage to be dealt as bleed damage
    private int ItemBleedDamage
    {
        get {return itemBleedDamage;}
        set {itemBleedDamage = value;}
    }
    private int weaponBleedDamage = 0;
    private int WeaponBleedDamage
    {
        get {return weaponBleedDamage;} //Adding this in case we want a weapon to have bleed damage up at some point
        set {weaponBleedDamage = value;}
    }
    public int BleedDamage
    {
        get {return (Damage * ((ItemBleedDamage + WeaponBleedDamage) / 100));} //This gives bleed damage as a percentage of the weapon's damage equal to the item % and the weapon % added together
    }
    
    //Extra bullets - At the moment this is just changed for the shotgun and not any items but having it here for future proofing in case it changes in an upgrade
    //Bullets need to always return an even number so it rounds up
    private int itemExtraBullets = 0;
    public int ItemExtraBullets
    {
        get {return itemExtraBullets;}
        set {itemExtraBullets = value;}
    }
    private int weaponExtraBullets = 0;
    public int WeaponExtraBullets
    {
        get {return weaponExtraBullets;}
        set {weaponExtraBullets = value;}
    }
    public int ExtraBullets
    {
        get {return (ItemExtraBullets + wWeaponExtraBullets) + ((ItemExtraBullets + WeaponExtraBullets) % 2);} //Sum of the item and weapon extra bullets plus 1 if its an odd number
    }

    //Spread - This is only used for the shotgun. Can be changed by items
    private float itemSpread = 0;
    public float ItemSpread
    {
        get {return itemSpread;}
        set {itemSpread = value;}
    }
    private float weaponSpread = 0;
    public float WeaponSpread
    {
        get {return weaponSpread;}
        set 
        { weaponSpread = value; }
    }
    public float Spread
    {
        get 
        {
            //If the weapon is not a shotgun it shouldn't have spread
            //Can be changed if we let other weapons have spread at some point
            //Maybe the machine gun? We would likely need to change how the spread works if we did
            if (CurrentWeapon != WeaponType.Shotgun)
            {
                return 0;
            }
            return ItemSpread + WeaponSpread;
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        currentWeapon = WeaponType.Pistol;
    }
}
