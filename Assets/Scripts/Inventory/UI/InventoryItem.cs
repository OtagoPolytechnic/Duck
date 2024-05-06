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
    public string textName;
    public string textDesc;
    public int textStacks;
    public GameObject inventory;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI itemStacks;
    public Image bordercolor;
    public GameObject timerManager;
    public Timer timer;
    public ItemController itemController;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerManager.GetComponent<Timer>();
        itemName.text = textName;
        itemDesc.text = textDesc;
        itemStacks.text = textStacks.ToString();
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
        textStacks++;
        itemStacks.text = textStacks.ToString();
        itemController.ItemPicked(textName); //assign the clicked item to the player
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
