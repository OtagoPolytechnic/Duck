using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //this is the controller for the inventory panel's control.
    public GameObject timerManager;
    public Timer timer;
    [SerializeField]
    private InventoryPage itemPanel;
    [HideInInspector]
    public int inventorySize = 3;

    public void Start() 
    {
        timer = timerManager.GetComponent<Timer>();
    }

    public void Update() 
    {
        if (!timer.running)
        {
            itemPanel.Show();
        }
        else
        {     
            itemPanel.Hide();
        }
    }
}
