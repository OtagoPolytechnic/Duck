using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Bullet bullet;
    
    //if you change something in this list you need to change it in InventoryPage.cs's list named itemlist
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
                Debug.Log($"Speed: {PlayerHealth.maxHealth}");
            break;
            case "Speed Increase":
                TopDownMovement.moveSpeed *= 1.05f;
                Debug.Log($"Speed: {TopDownMovement.moveSpeed}");           
            break;
            default:
                Debug.LogError("No item was picked, either there is a new item added that hasn't been mirrored here or an item's name is incorrect.");
            break;
        }
    }
}
