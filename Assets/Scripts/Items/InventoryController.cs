using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject timerManager;
    public Timer timer;
   [SerializeField]
    private InventoryPage inventoryUI;

    public void Start() 
    {
        timer = timerManager.GetComponent<Timer>();
    }

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
