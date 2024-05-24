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
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;
    public TextMeshProUGUI textStacks;
    public Image borderColor;
    public Color32 commonColor;
    public Color32 uncommonColor;
    public Color32 rareColor;
    public Color32 epicColor;
    public GameObject timerManager;
    public Timer timer;
    public ItemController itemController;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerManager.GetComponent<Timer>();
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
        itemController.ItemPicked(itemID); //assign the clicked item to the player
        itemChosen = true; 
    }
}
