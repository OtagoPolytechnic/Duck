using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemList 
{
    public Item item;
    public string name;
    public string rarity;
    public int level;

    public ItemList(Item item, string name, string rarity, int level)
    {
        this.item = item;
        this.name = name;
        this.rarity = rarity;
        this.level = level;
    }
}
