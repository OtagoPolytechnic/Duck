using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
[System.Serializable]
public class Item
{
    public string name;
    public string desc;
    public rarity rarity;
}

public enum rarity{
    Common,
    Uncommon,
    Rare,
    Epic
}
public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    private int index;
    private string randomItemName;
    private string randomItemDesc;
    private rarity randomItemRarity;
    public rarity roll;    
    //if you change something in this list you need to change it in ItemController.cs's method ItemPicked()
    public static List<Item> itemList = new List<Item>{ //char limit of 99 in description 
        new() { name = "Damage Increase", desc = "Increases damage you deal", rarity = rarity.Common },
        new() { name = "Health Increase", desc = "Gives you more max health", rarity = rarity.Common },
        new() { name = "Speed Increase", desc = "Increases your speed", rarity = rarity.Common },
        new() { name = "Extra Life", desc = "You gain an extra life", rarity = rarity.Rare },
        new() { name = "Bleed", desc = "Your hits bleed enemies", rarity = rarity.Uncommon },
        new() { name = "Lifesteal", desc = "Your hits heal you", rarity = rarity.Uncommon},
        new() { name = "Regen", desc = "Your health slowly regenerates over time", rarity = rarity.Uncommon },
        new() { name = "Shotgun", desc = "You shoot a spread of bullets instead of one", rarity = rarity.Rare }, //wip
        new() { name = "Glass Cannon", desc = "Halves your health to double your damage", rarity = rarity.Epic },
        new() { name = "Firerate Increase", desc = "You shoot faster", rarity = rarity.Common },
        new() { name = "Explosive Bullets", desc = "Your bullets explode on impact", rarity = rarity.Uncommon },
        new() { name = "Crit Chance", desc = "You have an increased chance to deal critical damage" , rarity = rarity.Uncommon },
    };

    List<InventoryItem> preGenItems = new List<InventoryItem>();

    public rarity GetWeightedRarity() {
        // Define some thresholds for different item rarities. (between 0 and 1)
        float commonRoll = 0.4f;
        float uncommonRoll = 0.65f;
        float rareRoll = 0.8f;
        float epicRoll = 0.95f;    
        // generate a random value (0->1)
        float genRarity = UnityEngine.Random.value;
        // Figure out which threshold this falls under.
        if (genRarity < commonRoll)
        {
            roll = rarity.Common;
        }
        else if (genRarity < uncommonRoll)
        {
            roll = rarity.Uncommon;
        }
        else if (genRarity < rareRoll)
        {
            roll = rarity.Rare;
        }
        else if (genRarity < epicRoll)
        {
            roll = rarity.Epic;
        }
        // Return the rarity associated with that threshold.
        return roll;
    }

    public void InitializeInventoryUI(int inventorySize) //this is called every time the item ui pops up
    { 
        List<Item> tempItems = new List<Item>(itemList);
        List<Item> generatedRarityList = new List<Item>();
        
        for (int i = preGenItems.Count - 1; i >= 0; i--) //makes sure the items previously generated are cleared to not bunch up on the inventory window
        {
            Destroy(preGenItems[i].gameObject); //removes item
            preGenItems.RemoveAt(i); //removes floating null pointer
        } 

        for (int i = 0; i < inventorySize; i++) //because of the new weight code, this now has dupelicates back
        {
            // Get a random rarity.
            randomItemRarity = GetWeightedRarity();
            // Create a list of all the available items of that rarity.
            foreach (Item rarity in tempItems)
            {
                Debug.Log(rarity.name);
                if (rarity.rarity == randomItemRarity)
                {
                    generatedRarityList.Add(rarity);
                }
            }

            // then pick a random index from that subset and use that as the item.
            index = UnityEngine.Random.Range(0, generatedRarityList.Count); 

            randomItemName = generatedRarityList[index].name;
            randomItemDesc = generatedRarityList[index].desc;
            randomItemRarity = generatedRarityList[index].rarity;

            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {randomItemName}");
            generatedRarityList.RemoveAt(index);
            tempItems.RemoveAt(index);
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);            
            preGenItems.Add(item);

            item.GetComponent<InventoryItem>().itemName = randomItemName;
            item.GetComponent<InventoryItem>().itemDesc = randomItemDesc;
            item.GetComponent<InventoryItem>().itemRarity = randomItemRarity;
            item.transform.SetParent(contentPanel);
            item.transform.localScale = new Vector3(1, 1, 1); //this is to fix the parent scale issue. See https://github.com/BIT-Studio-4/Duck-Game/issues/65 for context
        }
    }
                
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
