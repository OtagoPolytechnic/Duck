using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [HideInInspector]
    public bool itemChosen;  
    [HideInInspector]
    public int itemID; 
    [HideInInspector]
    public string itemName;
    [HideInInspector]
    public string itemDesc;
    [HideInInspector]
    public rarity itemRarity;
    [HideInInspector]
    public int itemStacks;
    public GameObject inventory;
    public TextMeshProUGUI textName, textDesc, textStacks;
    public Image borderColor;
    public Color32 commonColor, uncommonColor, rareColor, epicColor;
    public GameObject timerManager;
    public ItemController itemController;

    void Start()
    {
        textName.text = itemName;
        textDesc.text = itemDesc;
        textStacks.text = itemStacks.ToString();

        if (itemRarity == rarity.Uncommon) //sets background color
        {
            borderColor.color = uncommonColor;
        }
        else if (itemRarity == rarity.Rare)
        {
            borderColor.color = rareColor;
        }
        else if (itemRarity == rarity.Epic)
        {
            borderColor.color = epicColor;
        }
        else //assume all other items are common
        {
            borderColor.color = commonColor;
        }
    }
    public void Click()
    {
        itemController.ItemPicked(itemID); //activate the item selected's code
        itemChosen = true; 
    }
}
