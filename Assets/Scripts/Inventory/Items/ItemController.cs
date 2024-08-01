using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    //addition of a new item in the inventory page script, requires its functionailty in here
    public GameObject eggPrefab;
    public void ItemPicked(int itemID)
    {
        Debug.Log(itemID);

        switch(itemID) 
        {
            case 0:
                PlayerStats.Instance.Damage += 10;
                Debug.Log($"Damage: {PlayerStats.Instance.Damage}");
            break;
            case 01:
                PlayerStats.Instance.MaxHealth *= 1.10f;
                Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}");
            break;
            case 02:
                TopDownMovement.moveSpeed *= 1.05f;
                Debug.Log($"Speed: {TopDownMovement.moveSpeed}");           
            break;
            case 03:
                PlayerStats.Instance.RegenAmount += 1f;
                PlayerStats.Instance.RegenTrue = true;
                Debug.Log($"Regen amount: {PlayerStats.Instance.RegenAmount}"); 
            break;
            case 04:
                Shooting.Instance.Firerate *= 0.9f;
                Debug.Log($"Firerate: {Shooting.Instance.Firerate}"); 
            break;
            case 05:
                EnemyHealth.bleedAmount += 5;
                PlayerStats.Instance.BleedTrue = true;
                Debug.Log($"Bleed amount: {EnemyHealth.bleedAmount}"); 
            break;
            case 06:
                PlayerStats.Instance.LifestealAmount += 1f;
                Debug.Log($"Lifesteal amount: {PlayerStats.Instance.LifestealAmount}"); 
            break;
            case 07:
                PlayerStats.Instance.ExplosiveBullets = true;
                PlayerStats.Instance.ExplosionSize +=1;
                Debug.Log($"Explosion size: {PlayerStats.Instance.ExplosionSize}");
            break;     
            case 08:
                GameObject newEgg = Instantiate(eggPrefab,  new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Nest").transform);
                newEgg.transform.localScale = new Vector3(0.3333333f,0.3333333f,0.3333333f);
            break;
            case 09:
                PlayerStats.Instance.CritChance += 0.07f;
                if (PlayerStats.Instance.CritChance >= 1)
                {
                    PlayerStats.Instance.CritChance = 1;
                }
                Debug.Log($"Crit Chance: {PlayerStats.Instance.CritChance}");
            break;
            case 10:
                PlayerStats.Instance.MaxHealth /= 2f;
                PlayerStats.Instance.Damage *= 2;
                Debug.Log($"Players max health as been cut in half to:{PlayerStats.Instance.MaxHealth}. Their current health is: {PlayerStats.Instance.CurrentHealth}. Their damage has been doubled to: {PlayerStats.Instance.Damage}");
            break;
            case 11:
                PlayerStats.Instance.HasShotgun = true;
                PlayerStats.Instance.BulletAmount += 2;
                Debug.Log($"Shotgun bullets: {PlayerStats.Instance.BulletAmount}");
            break;
            case 12:
                for (int i = 0; i < 2; i++)
                {
                    int randomRoll = UnityEngine.Random.Range(0, 4);
                    if (randomRoll == 0)
                    {
                        PlayerStats.Instance.Damage += 5;
                        Debug.Log($"Damage: {PlayerStats.Instance.Damage}");
                    }
                    else if (randomRoll == 1)
                    {
                        PlayerStats.Instance.MaxHealth *= 1.05f;
                        Debug.Log($"Max health: {PlayerStats.Instance.MaxHealth}");
                    }
                    else if (randomRoll == 2)
                    {
                        TopDownMovement.moveSpeed *= 1.025f;
                        Debug.Log($"Speed: {TopDownMovement.moveSpeed}"); 
                    }
                    else if (randomRoll == 3)
                    {
                        Shooting.Instance.Firerate *= 0.95f;
                        Debug.Log($"Firerate: {Shooting.Instance.Firerate}"); 
                    }
                }
                
                break;
            default:
                Debug.LogError("No item was given to the player, either, the item added to the list was not given a case, or the id does not match a current case.");
            break;
        }
        InventoryPage.itemList[itemID].stacks += 1;
    }
}
