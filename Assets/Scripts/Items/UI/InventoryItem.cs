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
    public bool itemChosen;    //this needs to be in the inventory page because this doesn't exist on game start unless we put this prefab in /Resources https://docs.unity.cn/520/Documentation/Manual/LoadingResourcesatRuntime.html#:~:text=Resource%20Folders%20are%20collections%20of,name%20the%20folder%20“Resources”.
    public string textName;
    public string textDesc;
    public GameObject inventory;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public Image bordercolor;
    public GameObject timerManager;
    public Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerManager.GetComponent<Timer>();
        //this code may be ok to delete unsure at 11:51pm, 9/04/2024
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
        //assign the clicked item to the player ---> either in the player script or its own that it attached to the player prefab
        itemChosen = true; 
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
