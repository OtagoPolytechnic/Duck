using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
            }
            else 
            {
                inventoryUI.Hide();
            }

        }
    }
}
