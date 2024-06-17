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
                PlayerHealth.damage += 10;
                Debug.Log($"Damage: {PlayerHealth.damage}");
            break;
            case 01:
                float current = PlayerHealth.maxHealth;
                PlayerHealth.maxHealth *= 1.10f;
                Mathf.RoundToInt(PlayerHealth.maxHealth);
                PlayerHealth.currentHealth += PlayerHealth.maxHealth - current;
                Debug.Log($"Max health: {PlayerHealth.maxHealth}");
            break;
            case 02:
                TopDownMovement.moveSpeed *= 1.05f;
                Debug.Log($"Speed: {TopDownMovement.moveSpeed}");           
            break;
            case 03:
                PlayerHealth.regenAmount += 1f;
                PlayerHealth.regenTrue = true;
                Debug.Log($"Regen amount: {PlayerHealth.regenAmount}"); 
            break;
            case 04:
                Shooting.firerate *= 0.9f;
                Debug.Log($"Firerate: {Shooting.firerate}"); 
            break;
            case 05:
                EnemyHealth.bleedAmount += 5;
                PlayerHealth.bleedTrue = true;
                Debug.Log($"Bleed amount: {EnemyHealth.bleedAmount}"); 
            break;
            case 06:
                PlayerHealth.lifestealAmount += 1f;
                Debug.Log($"Lifesteal amount: {PlayerHealth.lifestealAmount}"); 
            break;
            case 07:
                PlayerHealth.explosiveBullets = true;
                PlayerHealth.explosionSize +=1;
                Debug.Log($"Explosion size: {PlayerHealth.explosionSize}");
            break;     
            case 08:
                GameObject newEgg = Instantiate(eggPrefab,  new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Nest").transform);
                newEgg.transform.localScale = new Vector3(0.3333333f,0.3333333f,0.3333333f);
            break;
            case 09:
                PlayerHealth.critChance += 0.07f;
                if (PlayerHealth.critChance >= 1)
                {
                    PlayerHealth.critChance = 1;
                }
                Debug.Log($"Crit Chance: {PlayerHealth.critChance}");
            break;
            case 10:
                PlayerHealth.maxHealth /= 2f;
                if (PlayerHealth.maxHealth <= PlayerHealth.currentHealth)
                {
                    PlayerHealth.currentHealth = PlayerHealth.maxHealth;
                }
                else
                {
                    PlayerHealth.currentHealth /= 2f;
                }
                PlayerHealth.damage *= 2;
                Debug.Log($"Players max health as been cut in half to:{PlayerHealth.maxHealth}. Their current health is: {PlayerHealth.currentHealth}. Their damage has been doubled to: {PlayerHealth.damage}");
            break;
            case 11:
                PlayerHealth.hasShotgun = true;
                PlayerHealth.bulletAmount += 2;
                Debug.Log($"Shotgun bullets: {PlayerHealth.bulletAmount}");
            break;
            case 12:
                for (int i = 0; i < 2; i++)
                {
                    int randomRoll = UnityEngine.Random.Range(0, 4);
                    if (randomRoll == 0)
                    {
                        PlayerHealth.damage += 5;
                        Debug.Log($"Damage: {PlayerHealth.damage}");
                    }
                    else if (randomRoll == 1)
                    {
                        float current2 = PlayerHealth.maxHealth;
                        PlayerHealth.maxHealth *= 1.05f;
                        Mathf.RoundToInt(PlayerHealth.maxHealth);
                        PlayerHealth.currentHealth += PlayerHealth.maxHealth - current2;
                        Debug.Log($"Max health: {PlayerHealth.maxHealth}");
                    }
                    else if (randomRoll == 2)
                    {
                        TopDownMovement.moveSpeed *= 1.025f;
                        Debug.Log($"Speed: {TopDownMovement.moveSpeed}"); 
                    }
                    else if (randomRoll == 3)
                    {
                        Shooting.firerate *= 0.95f;
                        Debug.Log($"Firerate: {Shooting.firerate}"); 
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
