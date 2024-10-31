using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemEffectTable : MonoBehaviour
{
    //If you add a new item in the items.json file you need to add the functionality in here
    [SerializeField]
    private GameObject eggPrefab;
    public void ItemPicked(Item item)
    {
        int id;
        if (item == null)
        {
            id = -1;
        }
        else
        {
            id = item.id;
        }        
        Debug.Log(id);
        switch(id) 
        {
            case -1:
                Debug.Log("Item choice skipped");
                break;
            case 0: //Sharpened Talons
                WeaponStats.Instance.FlatDamage += 10;
                Debug.Log($"Damage: {WeaponStats.Instance.Damage}");
                break;
            case 01: //Oats
                PlayerStats.Instance.PercentBonusHealth += 10;
                Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}");
                break;
            case 02: //Boots
                TopDownMovement.Instance.PercentBonusSpeed += 5;
                Debug.Log($"Speed: {TopDownMovement.Instance.MoveSpeed}");
                break;
            case 03: //Band-Aid
                PlayerStats.Instance.FlatRegenerationPercentage += 2;
                Debug.Log($"Regeneration Percentage: {PlayerStats.Instance.RegenerationPercentage}");
                break;
            case 04:
                WeaponStats.Instance.PercentageFireDelay *= .9f; //This makes it shoot 10% faster
                Debug.Log($"Fire delay: {WeaponStats.Instance.FireDelay}");
                break;
            case 05: //Chompers
                WeaponStats.Instance.ItemBleedDamage += 1; //2% max health bleed per second
                Debug.Log($"Bleed amount: {WeaponStats.Instance.BleedDamage}");
                break;
            case 06: //Leech
                PlayerStats.Instance.FlatLifestealPercentage += 1;
                Debug.Log($"Lifesteal percentage: {PlayerStats.Instance.LifestealPercentage}");
                break;
            case 07: //Explosive Bullets
                WeaponStats.Instance.ItemExplosiveBullets = true;
                WeaponStats.Instance.ItemExplosionSize += 2;
                WeaponStats.Instance.ItemExplosionDamage += 50; //50% of the weapon damage as an explosion. Unsure about the balance 
                Debug.Log($"Explosion size: {WeaponStats.Instance.ExplosionSize}");
                Debug.Log($"Explosion damage: {WeaponStats.Instance.ExplosionDamage}");
                break;
            case 08: //Egg
                //This is going to be changed to not being actual items on the ground
                GameObject newEgg = Instantiate(eggPrefab,  new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Nest").transform);
                newEgg.transform.localScale = new Vector3(1f/3f, 1f/3f, 1f/3f); //Properly setting the scale to one third
                break;
            case 09: //Lucky Feather
                WeaponStats.Instance.FlatCritChance += 8; //8% crit chance
                Debug.Log($"Crit Chance: {WeaponStats.Instance.CritChance}");
                break;
            case 10:
                WeaponStats.Instance.PercentageFireDelay *= .7f; //This makes it shoot 30% faster
                Debug.Log($"Fire delay: {WeaponStats.Instance.FireDelay}");
                break;
            case 11: //Eye of the Eagle
                WeaponStats.Instance.FlatCritChance += 24; //24% crit chance
                Debug.Log($"Crit Chance: {WeaponStats.Instance.CritChance}");
                break;
            case 12: //Lucky Dive
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
                            WeaponStats.Instance.PercentageFireDelay *= .94f;
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
            case 13: //Shotgun
                WeaponStats.Instance.CurrentWeapon = WeaponType.Shotgun;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 14://Sniper
                WeaponStats.Instance.CurrentWeapon = WeaponType.Sniper;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 15: //Machine Gun
                WeaponStats.Instance.CurrentWeapon = WeaponType.MachineGun;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 16: //Dual Pistol
                WeaponStats.Instance.CurrentWeapon = WeaponType.DualPistol;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 17: //Rocket Launcher
                WeaponStats.Instance.CurrentWeapon = WeaponType.RocketLauncher;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 18: //Sword
                WeaponStats.Instance.CurrentWeapon = WeaponType.Sword;
                Debug.Log($"Current Weapon: {WeaponStats.Instance.CurrentWeapon}");
                break;
            case 19: //Ricochet
                WeaponStats.Instance.RicochetCount += 1;
                Debug.Log($"Ricochet count: {WeaponStats.Instance.RicochetCount}");
                break;
            case 20: //Piercing Bullets
                WeaponStats.Instance.ItemPiercing = true;
                WeaponStats.Instance.ItemPierceAmount += 1;
                Debug.Log($"Piercing: {WeaponStats.Instance.PierceAmount}");
                break;
            case 21: //Bigger Barrel
                WeaponStats.Instance.ItemExtraBullets += 4;
                Debug.Log($"Extra bullets: {WeaponStats.Instance.ExtraBullets}");
                break;
            case 22: //Spine Plate
                TopDownMovement.Instance.PercentBonusSpeed *= 0.75f; //3/4 the speed
                PlayerStats.Instance.SpinePlate = true;
                PlayerStats.Instance.SpinePercent += 200; //200% of damage taken is dealt to the enemy per stack
                Debug.Log($"Speed: {TopDownMovement.Instance.MoveSpeed}");
                break;
            case 23: //Glass Cannon
                PlayerStats.Instance.PercentBonusHealth /= 2; //Half the current max health. If this is picked multiple times it will keep halving the max health
                WeaponStats.Instance.PercentageDamage *= 2; //Double the damage. If this is picked multiple times it will keep doubling the damage 
                Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}. Damage: {WeaponStats.Instance.Damage}");
                break;
            case 24: //Bloodletter's Curse
                //This gives 30% of your damage as lifesteal and then doubles all your lifesteal
                //This means it gives 60% total and doubles the effectiveness of all other lifesteal
                PlayerStats.Instance.FlatLifestealPercentage += 30;
                PlayerStats.Instance.PercentLifestealPercentage *= 2;
                PlayerStats.Instance.DotTick = 0.1f; //Tick rate for the dot
                PlayerStats.Instance.DotDamage += 1; //Damage per tick
                Debug.Log($"Lifesteal percentage: {PlayerStats.Instance.LifestealPercentage}");
                Debug.Log($"Dot tick: {PlayerStats.Instance.DotDamage}");
                break;
            case 25: //Weak Spots
                WeaponStats.Instance.FlatCritDamage += 8; //48% crit damage
                Debug.Log($"Crit Damage: {WeaponStats.Instance.CritDamage}");
                break;
            case 26: //Stone Wall
                PlayerStats.Instance.PercentBonusHealth *= 2; //Double the current max health
                WeaponStats.Instance.PercentageDamage /= 2; //Half the damage
                Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}. Damage: {WeaponStats.Instance.Damage}");
                break;
            case 27: //Radiating Bombs
                WeaponStats.Instance.ItemRadioactive = true;
                WeaponStats.Instance.RadiationDamagePercentage += 10;
                Debug.Log($"Radiation damage: {WeaponStats.Instance.RadiationDamage}");
                break;
            case 28: //MOAB
                WeaponStats.Instance.PercentageExplosionSize += 100;
                WeaponStats.Instance.ItemExplosionDamage += 50;
                Debug.Log($"Explosion size: {WeaponStats.Instance.ExplosionSize}");
                Debug.Log($"Explosion damage: {WeaponStats.Instance.ExplosionDamage}");
                break;
            case 29: //Sword of the Master
                if (!WeaponStats.Instance.HasSwordBeam)
                {
                    WeaponStats.Instance.HasSwordBeam = true;
                    Debug.Log($"Given player sword beam");

                }
                else
                {
                    WeaponStats.Instance.SwordBeamMultiplier += 0.1f;
                    Debug.Log($"Player already has sword beam");
                }
                break;
            case 30: //Overheat - UNIMPLEMENTED
                Debug.Log("Unimplemented Overheating item");
                break;
            case 31: //Rocket boots
                TopDownMovement.Instance.PercentBonusSpeed += 15;
                Debug.Log($"Speed: {TopDownMovement.Instance.MoveSpeed}");
                break;
            case 32: //Full Breakfast
                PlayerStats.Instance.PercentBonusHealth += 30;
                Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}");
                break;
            case 33: //Expert Aim
                WeaponStats.Instance.FlatCritDamage += 24; //24% crit damage
                Debug.Log($"Crit Damage: {WeaponStats.Instance.CritDamage}");
                break;
            case 34: //Hollow-point bullets
                WeaponStats.Instance.FlatDamage += 30;
                Debug.Log($"Damage: {WeaponStats.Instance.Damage}");
                break;
            case 35: //Reflector
                if (!WeaponStats.Instance.HasReflector)
                {
                    Debug.Log($"Gained reflector");
                    WeaponStats.Instance.HasReflector = true;
                }
                else
                {
                    WeaponStats.Instance.ReflectCooldown -= 0.5f;
                    Debug.Log($"Reflector cooldown reduced to {WeaponStats.Instance.ReflectCooldown}");
                }
                break;
            case 36: //Claymore
                WeaponStats.Instance.PercentageDamage *= 4;
                WeaponStats.Instance.PercentageFireDelay *= 3f;
                break;
            case 37:
                SkillEffects.Instance.cooldownModifier *= 0.9f;
            break;
            case 38:
                SkillEffects.Instance.durationModifier += 0.1f;
            break;
            case 39:
                SkillEffects.Instance.activeSkillIcon.style.backgroundImage = Resources.Load<Texture2D>("Dash-Corrupted");
                SkillEffects.Instance.cursedDash = true;
                SkillEffects.Instance.cooldownModifier -= 0.5f;
                item.single = true;
            break;
            case 40:
                SkillEffects.Instance.activeSkillIcon.style.backgroundImage = Resources.Load<Texture2D>("Vanish-Corrupted");
                SkillEffects.Instance.cursedVanish = true;
                SkillEffects.Instance.cooldownModifier += SkillEffects.Instance.cooldownModifier;
                SkillEffects.Instance.durationModifier -= 0.5f;
                item.single = true;
            break;
            case 41:
                SkillEffects.Instance.activeSkillIcon.style.backgroundImage = Resources.Load<Texture2D>("Decoy-Corrupted");
                SkillEffects.Instance.cursedDecoy = true;
                SkillEffects.Instance.cooldownModifier += 0.25f;
                item.single = true;
            break;
            case 42:
                PlayerStats.Instance.deathsDance = true;
                item.single = true;
            break;
            default:
                Debug.Log($"The ID: {item.id} has not been given a case in the item effect table.");
            break;
        }
    }
}
