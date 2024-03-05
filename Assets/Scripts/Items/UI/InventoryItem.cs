using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
//This code is currently for a mouse pointer, if we want updates for a controller or joystick, will need to refactor slightly
    private bool mouse_over = false;
    public int randomItem;
    
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    // Start is called before the first frame update
    void Start()
    {
        randomItem = UnityEngine.Random.Range(1,4);
        
        itemName.text = randomItem.ToString();
        Debug.Log(randomItem);
    }

    // Update is called once per frame
   void Update()
    {
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit");
    }
}
