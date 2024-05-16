using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    //if you change something in this list you need to change it in InventoryPage.cs's list named itemlist
    public GameObject eggPrefab;
    public void ItemPicked(string textName)
    {
        Debug.Log(textName);

        switch(textName) 
        {
            case "Damage Increase": 
                PlayerHealth.damage += 10;
                Debug.Log($"Damage: {PlayerHealth.damage}");
            break;
            case "Health Increase":
                PlayerHealth.maxHealth *= 1.10f;
                Math.Round(PlayerHealth.maxHealth, 0, MidpointRounding.AwayFromZero);
                Debug.Log($"Max health: {PlayerHealth.maxHealth}");
            break;
            case "Speed Increase":
                TopDownMovement.moveSpeed *= 1.05f;
                Debug.Log($"Speed: {TopDownMovement.moveSpeed}");           
            break;
            case "Regen":
                PlayerHealth.regenAmount += 5f;
                PlayerHealth.regenTrue = true;
                Debug.Log($"Regen amount: {PlayerHealth.regenAmount}"); 
            break;
            case "Firerate Increase":
                shooting.firerate *= 0.9f;
                Debug.Log($"Firerate: {shooting.firerate}"); 
            break;
            case "Bleed":
                EnemyHealth.bleedAmount = 5f;
                EnemyHealth.bleedTrue = true;
                Debug.Log($"Bleed amount: {EnemyHealth.bleedAmount}"); 
            break;
            case "Lifesteal":
                PlayerHealth.lifestealAmount = 5f;
                Debug.Log($"Lifesteal amount: {PlayerHealth.lifestealAmount}"); 
            break;
            case "Explosive Bullets":
                PlayerHealth.explosiveBullets = true;
                PlayerHealth.explosionAmount +=1;
            break;     
            case "Extra Life":
                Instantiate(eggPrefab,  new Vector3(0,0,0), Quaternion.identity, GameObject.Find("Nest").transform);
            break;
            case "Crit Chance":
                PlayerHealth.critChance += 0.15f;
                if (PlayerHealth.critChance >= 1)
                {
                    PlayerHealth.critChance = 1;
                }
            break;
            case "Glass Cannon":
                PlayerHealth.maxHealth /= 0.50f;
                if (PlayerHealth.maxHealth <= PlayerHealth.currentHealth)
                {
                    PlayerHealth.currentHealth = PlayerHealth.maxHealth;
                }
                else
                {
                    PlayerHealth.currentHealth /= 0.50f;
                }
                PlayerHealth.damage *= 2;
            break;
            case "Shotgun":
                PlayerHealth.hasShotgun = true;
                PlayerHealth.bulletAmount += 2;
            break;
            default:
                Debug.LogError("No item was picked, either there is a new item added that hasn't been mirrored here or an item's name is incorrect.");
            break;
        }
    }
}
