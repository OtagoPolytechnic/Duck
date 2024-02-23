using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Timer timer;
   [SerializeField]
    private InventoryPage inventoryUI;

    public void Update() 
    {
        if (timer.running == false) 
        {
            inventoryUI.Show();
        }
        else 
        {
            inventoryUI.Hide();
        }
    }
}
