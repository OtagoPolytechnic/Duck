using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanelController : MonoBehaviour
{
    //this is the controller for the inventory panel's control.

    [SerializeField]
    private ItemPanel itemPanel;
    [HideInInspector]
    public int inventorySize = 3;
    
    public void Update() 
    {
        if (GameSettings.gameState == GameState.ItemSelect)
        {
            itemPanel.Show();
        }
        else
        {     
            itemPanel.Hide();
        }
    }
}
