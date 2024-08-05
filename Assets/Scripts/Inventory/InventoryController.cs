using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //this is the controller for the inventory panel's control.
    public GameObject timerManager;
    [SerializeField]
    private InventoryPage inventoryUI;
    [HideInInspector]
    public int inventorySize = 3;
    
    public void Update() 
    {
        if (GameSettings.gameState == GameState.ItemSelect)
        {
            inventoryUI.Show();
        }
        else
        {     
            inventoryUI.Hide();
        }
    }
}
