using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
//This code is currently for a mouse pointer, if we want updates for a controller or joystick, will need to refactor slightly
    private bool mouseOver = false;
    public int textNumber;
    public GameObject inventory;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    // Start is called before the first frame update
    void Start()
    {
        // InventoryPage itemRender = inventory.GetComponent<InventoryPage>();
        // Debug.Log($"In Item: {itemRender.randomItem}");
        itemName.text = textNumber.ToString();
    }

    // Update is called once per frame
   void Update()
    {
        if (mouseOver)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseOver = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseOver = false;
        Debug.Log("Mouse exit");
    }
}
