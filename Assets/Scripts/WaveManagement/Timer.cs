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
    private float currentTime;
    public int waveNumber;
    public bool running;

    private bool gotItems = true;
    bool geninventory = false;
    [HideInInspector]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [HideInInspector]
    public GameObject[] itemObjects;

    void Start()
    {
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();
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
                    NextWave();
                }
            }

        }
        
        if(currentTime <= 0)
        {
            EndWave();
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

    private void NextWave()
    {
        waveNumber += 1;
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();

        EnemySpawner.healthMultiplier += 0.5f;
        EnemySpawner.spawnTimer -= 0.1f;

        running = true;
        geninventory = false;
        //inventoryItems.Clear();
    }

    private void EndWave()
    {
        running = false;

        //cull enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        gotItems = false;
        if (!geninventory)
        {
            inventoryUI.InitializeInventoryUI(inventorySize);
            
            geninventory = true;
        }
    }
}
