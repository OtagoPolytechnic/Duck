using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Item
{
    public int id;
    public string name;
    public string desc;
    public rarity rarity;
    public int stacks;
    public StyleColor rarityColor 
    {
        get
        {
           return rarityColors[rarity];
        }
    }
    private Dictionary<rarity, StyleColor> rarityColors = new Dictionary<rarity, StyleColor>
    {
        {rarity.Common, new StyleColor(new Color32(135, 150, 146, 255))},
        {rarity.Uncommon, new StyleColor(new Color32(79, 122, 52, 255))},
        {rarity.Rare, new StyleColor(new Color32(50, 173, 196, 255))},
        {rarity.Epic, new StyleColor(new Color32(127, 6, 145, 255))},
        {rarity.Weapon, new StyleColor(new Color32(90, 90, 90, 255))},
        {rarity.Legendary, new StyleColor(new Color32(179, 109, 28, 255))},
        {rarity.Cursed, new StyleColor(new Color32(108, 21, 13, 255))},
    };

}
[Serializable]
public class ItemList
{
    public List<Item> items;

 
}
