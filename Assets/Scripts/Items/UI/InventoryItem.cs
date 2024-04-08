using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
//This code is currently for a mouse pointer, if we want updates for a controller or joystick, will need to refactor slightly
    private bool mouseOver = false;
    public string textName;
    public string textDesc;
    public GameObject inventory;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public Image bordercolor;
    // Start is called before the first frame update
    void Start()
    {
        // InventoryPage itemRender = inventory.GetComponent<InventoryPage>();
        // Debug.Log($"In Item: {itemRender.randomItem}");
        itemName.text = textName;
        itemDesc.text = textDesc;
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

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Click");
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseOver = true;
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseOver = false;
        Debug.Log("Exit");
    }
}
