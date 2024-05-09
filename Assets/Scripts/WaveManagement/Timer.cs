using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    [SerializeField]
    private InventoryPage inventoryUI;
    [HideInInspector]
    public int inventorySize = 3;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI timerText;
    public float waveLength;
    public float currentTime;
    public int waveNumber;
    public bool running;
    private bool gotItems = true;
    bool geninventory = false;
    private GameObject[] spawnPoints;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public GameObject[] itemObjects;

    void Start()
    {
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
    }

    void Update()
    {
        if (!gotItems) //function is needed because unity cannot parse tags on same frame as instantiation 
        {     
            GetItems();
        }
        if(running)
        {
            currentTime -= Time.deltaTime;
        }
        else 
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].itemChosen)
                {
                    Debug.Log("Item Picked!");
                    nextWave();
                }
            }

        }
        
        if(currentTime <= 0)
        {
            running = false;
            gotItems = false;
            if (!geninventory)
            {
                inventoryUI.InitializeInventoryUI(inventorySize);
                
                geninventory = true;
            }
            
        }

        setTimerText();
    }

    private void GetItems()
    {
        itemObjects = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemObjects.Length; i++)
        {
            inventoryItems[i] = itemObjects[i].GetComponent<InventoryItem>();
        }
        gotItems = true;
    }
    private void setTimerText()
    {
        timerText.text = currentTime.ToString("0") + " s";
    }

    private void nextWave()
    {
        waveNumber += 1;
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();

        //Update every spawn point when the next wave starts
        for (int i=0; i<spawnPoints.Length; i++){
            spawnPoints[i].GetComponent<EnemySpawner>().enemyHealth += 10;
            spawnPoints[i].GetComponent<EnemySpawner>().spawnTimer -= 0.1f;
        }

        running = true;
        geninventory = false;
        //inventoryItems.Clear();
    }
}
