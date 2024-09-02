using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemEffectTable : MonoBehaviour
{
    //If you add a new item in the items.json file you need to add the functionality in here
    [SerializeField]
    private GameObject eggPrefab;
    public void ItemPicked(int itemID)
    {
        Debug.Log(itemID);

        switch(itemID) 
        {
            case -1:
                Debug.Log("Item choice skipped");
                break;
            case 0:
                WeaponStats.Instance.FlatDamage += 10;
                Debug.Log($"Damage: {WeaponStats.Instance.Damage}");
                break;
            case 01:
                PlayerStats.Instance.PercentBonusHealth += 10;
                Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}");
                break;
            case 02:
                TopDownMovement.Instance.PercentBonusSpeed += 5;
                Debug.Log($"Speed: {TopDownMovement.Instance.MoveSpeed}");           
                break;
            case 03:
                PlayerStats.Instance.FlatRegenerationPercentage += 1;
                Debug.Log($"Regeneration Percentage: {PlayerStats.Instance.RegenerationPercentage}"); 
                break;
            case 04:
                WeaponStats.Instance.PercentageFireDelay -= 10; //This makes it shoot 10% faster
                Debug.Log($"Fire delay: {WeaponStats.Instance.FireDelay}"); 
                break;
            case 05:
                WeaponStats.Instance.ItemBleedDamage += 2; //2% max health bleed per second
                Debug.Log($"Bleed amount: {WeaponStats.Instance.BleedDamage}"); 
                break;
            case 06:
                PlayerStats.Instance.FlatLifestealPercentage += 1;
                Debug.Log($"Lifesteal percentage: {PlayerStats.Instance.LifestealPercentage}"); 
                break;
            case 07:
                WeaponStats.Instance.ItemExplosiveBullets = true;
                WeaponStats.Instance.ItemExplosionSize += 2;
                WeaponStats.Instance.ItemExplosionDamage += 50; //50% of the weapon damage as an explosion. Unsure about the balance 
                Debug.Log($"Explosion size: {WeaponStats.Instance.ExplosionSize}");
                Debug.Log($"Explosion damage: {WeaponStats.Instance.ExplosionDamage}");
                break;     
            case 08:
                //This is going to be changed to not being actual items on the ground
                GameObject newEgg = Instantiate(eggPrefab,  new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Nest").transform);
                newEgg.transform.localScale = new Vector3(1f/3f, 1f/3f, 1f/3f); //Properly setting the scale to one third
                break;
            case 09:
                WeaponStats.Instance.FlatCritChance += 8; //8% crit chance
                Debug.Log($"Crit Chance: {WeaponStats.Instance.CritChance}");
                break;
            case 10:
                Debug.Log($"WIP Epic item");
                break;
            case 11:
                Debug.Log($"WIP Epic item");
                break;
            case 12:
                List<int> randomStats = new List<int> {0, 1, 2, 3, 4, 5};
                // Shuffle the list using LINQ
                randomStats = randomStats.OrderBy(x => UnityEngine.Random.value).ToList();
                for (int i = 0; i < 2; i++)
                {
                    //Takes the first two stats from the shuffled list
                    switch(randomStats[i])
                    {
                        case 0:
                            WeaponStats.Instance.FlatDamage += 6;
                            Debug.Log($"Damage: {WeaponStats.Instance.Damage}");
                            break;
                        case 1:
                            PlayerStats.Instance.PercentBonusHealth += 6;
                            Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}");
                            break;
                        case 2:
                            TopDownMovement.Instance.PercentBonusSpeed += 3;
                            Debug.Log($"Speed: {TopDownMovement.Instance.MoveSpeed}"); 
                            break;
                        case 3:
                            WeaponStats.Instance.PercentageFireDelay -= 6;
                            Debug.Log($"Fire Delay: {WeaponStats.Instance.FireDelay}"); 
                            break;
                        case 4:
                            WeaponStats.Instance.FlatCritChance += 5;
                            Debug.Log($"Crit Chance: {WeaponStats.Instance.CritChance}");
                            break;
                        case 5:
                            WeaponStats.Instance.FlatCritDamage += 3;
                            Debug.Log($"Crit Damage: {WeaponStats.Instance.FlatCritDamage}");
                            break;
                    }
                }
                break;
            case 13:
                WeaponStats.Instance.CurrentWeapon = WeaponType.Shotgun;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 14:
                WeaponStats.Instance.CurrentWeapon = WeaponType.Sniper;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 15:
                WeaponStats.Instance.CurrentWeapon = WeaponType.MachineGun;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");  
                break;
            case 16:
                WeaponStats.Instance.CurrentWeapon = WeaponType.DualPistol;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 17:
                WeaponStats.Instance.CurrentWeapon = WeaponType.RocketLauncher;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 18:
                WeaponStats.Instance.CurrentWeapon = WeaponType.Sword;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 19:
                Debug.Log($"Overheat (WIP)");
                break;
            case 20:
                Debug.Log($"Piercing (WIP)");
                break;
            case 21:
                Debug.Log($"More Pellets (WIP)");
                break;
            case 22:
                Debug.Log($"Midas Touch (WIP)");
                break;
            case 23:
                PlayerStats.Instance.PercentBonusHealth /= 2; //Half the current max health. If this is picked multiple times it will keep halving the max health
                WeaponStats.Instance.PercentageDamage *= 2; //Double the damage. If this is picked multiple times it will keep doubling the damage 
                Debug.Log($"Players max health as been cut in half to:{PlayerStats.Instance.MaxHealth}. Their current health is: {PlayerStats.Instance.CurrentHealth}. Their damage has been doubled to: {WeaponStats.Instance.Damage}");
                break;
            case 24:
                Debug.Log($"Blood letter's Curse(WIP)");
                break;
            case 25:
                WeaponStats.Instance.FlatCritDamage += 4; //4% crit damage
                Debug.Log($"Crit Damage: {WeaponStats.Instance.CritDamage}");
                break;
            default:
                Debug.Log($"The ID: {itemID} has not been given a case in the item effect table.");
            break;
        }
    }
}
