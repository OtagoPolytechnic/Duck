using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class Timer : MonoBehaviour
{
    [SerializeField]
    private InventoryPage itemPanel;
    [HideInInspector]
    public int inventorySize = 3;
    [SerializeField]
    private GameObject HUD;
    private Label waveNumberText;
    private Label timerText;

    public float waveLength;
    private float currentTime;
    public int waveNumber;
    public bool running;
    bool geninventory = false;
    void Awake()
    {
        VisualElement document = HUD.GetComponent<UIDocument>().rootVisualElement;

        waveNumberText = document.Q("WaveNumber") as Label;
        timerText = document.Q("Timer") as Label;
    }

    void Start()
    {
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();
    }

    void Update()
    {

        if(running)
        {
            currentTime -= Time.deltaTime;
        }
        else 
        {
            if (itemPanel.itemChosen)
            {
                NextWave();
            }
        }
        
        if(currentTime <= 0)
        {
            EndWave();
        }

        setTimerText();
    }

    private void EndWave()
    {
        running = false;

        CullEnemies();


        if (!geninventory)
        {
            itemPanel.InitializeInventoryUI(inventorySize);
            
            geninventory = true;
        }
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
        if(EnemySpawner.spawnTimer < 0.1f)
        {
            EnemySpawner.spawnTimer = 0.1f;
        }

        running = true;
        geninventory = false;
        itemPanel.itemChosen = false;
    }

    public static void CullEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject enemy in enemies)
        {
            EnemySpawner.currentEnemies.Remove(enemy);
            Destroy(enemy);
        }
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}
