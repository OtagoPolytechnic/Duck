using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //this is the controller for the inventory panel's control.
    public GameObject timerManager;
    public Timer timer;
   [SerializeField]
    private InventoryPage inventoryUI;
    public int inventorySize = 3;

    public void Start() 
    {
        timer = timerManager.GetComponent<Timer>();
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    public void Update() 
    {
        if (!timer.running)
        {
            inventoryUI.Show();
        }
        else
        {     
            inventoryUI.Hide();
        }
    }
}
