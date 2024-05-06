using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseOver = false;
    public bool itemChosen;   
    public string itemName;
    public string itemDesc;
    public rarity itemRarity;
    public int itemStacks;
    public GameObject inventory;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;
    public TextMeshProUGUI textStacks;
    public Image bordercolor;
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
    }

    // Update is called once per frame
   void Update()
    {
        if (mouseOver)
        {
            bordercolor.color = new Color32(43,56,77,150);//temp colors
        }
        else
        {
            bordercolor.color = new Color32(43,56,77,200);//temp colors
        }
    }

    public void Click()
    {
        itemStacks++;
        itemController.ItemPicked(itemName); //assign the clicked item to the player
        itemChosen = true; 
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseOver = false;
    }
}
