using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    //if you change something in this list you need to change it in ItemController.cs's method ItemPicked()
    public static List<Item> itemList = new List<Item>{ //char limit of 99 in description 
        new() { name = "Damage Increase", desc = "Increases damage you deal", rarity = rarity.Common },
        new() { name = "Health Increase", desc = "Gives you more max health", rarity = rarity.Common },
        new() { name = "Speed Increase", desc = "Increases your speed", rarity = rarity.Common },
        new() { name = "Extra Life", desc = "You gain an extra life", rarity = rarity.Rare },
        new() { name = "Bleed", desc = "Your hits bleed enemies", rarity = rarity.Uncommon },
        new() { name = "Lifesteal", desc = "Your hits heal you", rarity = rarity.Uncommon},
        new() { name = "Regen", desc = "Your health slowly regenerates over time", rarity = rarity.Uncommon },
        new() { name = "Shotgun", desc = "You shoot a spread of bullets instead of one", rarity = rarity.Rare },
        new() { name = "Glass Cannon", desc = "Halves your health to double your damage", rarity = rarity.Epic },
        new() { name = "Firerate Increase", desc = "You shoot faster", rarity = rarity.Common },
        new() { name = "Explosive bullets", desc = "Your bullets explode on impact", rarity = rarity.Uncommon },
        new() { name = "Crit Chance", desc = "You have an increased chance to deal critical damage" , rarity = rarity.Uncommon },
    };

    List<InventoryItem> preGenItems = new List<InventoryItem>();

    public void InitializeInventoryUI(int inventorySize) //this is called every time the item ui pops up
    { 
        List<Item> tempItems = new List<Item>(itemList);
        for (int i = preGenItems.Count - 1; i >= 0; i--) //makes sure the items previously generated are cleared to not bunch up on the inventory window
        {
            Destroy(preGenItems[i].gameObject); //removes item
            preGenItems.RemoveAt(i); //removes floating null pointer
        } 

        for (int i = 0; i < inventorySize; i++) //generates an item without duplication an assigns it to the prefab.
        {
            index = UnityEngine.Random.Range(0, tempItems.Count); 
            randomItemName = tempItems[index].name;
            randomItemDesc = tempItems[index].desc;
            randomItemRarity = tempItems[index].rarity;
            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {randomItemName}");
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
