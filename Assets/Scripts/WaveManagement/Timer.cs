using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class Timer : MonoBehaviour
{
    [SerializeField]
    private ItemPanel itemPanel;

    [SerializeField]
    private GameObject HUD;
    private Label waveNumberText;
    private Label timerText;
    [SerializeField] private BossSpawner bossSpawner;
    public float waveLength;
    private float currentTime;
    public int waveNumber;
    bool geninventory = false;
    public static Timer Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        VisualElement document = HUD.GetComponent<UIDocument>().rootVisualElement;

        waveNumberText = document.Q("WaveNumber") as Label;
        timerText = document.Q("Timer") as Label;
    }

    void Start()
    {
        GameSettings.waveNumber = waveNumber;
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();
        GameSettings.waveNumber = waveNumber;
    }

    void Update()
    {
        if (GameSettings.gameState == GameState.EndGame || GameSettings.gameState == GameState.BossVictory)
        {
            return;
        }
        if(GameSettings.gameState == GameState.InGame && GameSettings.waveNumber % 5 != 0)
        {
            currentTime -= Time.deltaTime;
       
        }
        else if(GameSettings.gameState == GameState.ItemSelect)
        {
            if (itemPanel.itemChosen)
            {
                NextWave();
            }
        }
        
        if(currentTime <= 0 || (BossHealth.Instance.boss !=null && BossHealth.Instance.boss.health <=0))
        {
            if (waveNumber == 25 && GameSettings.gameState == GameState.InGame)
            {
                GameManager.Instance.BossVictory();
            }
            else
            {
                EndWave();
            }
        }

        setTimerText();
    }

    public void EndWave()
    {
        GameSettings.gameState = GameState.ItemSelect;
        CullEnemies();
        if (!geninventory)
        {
            itemPanel.InitializeItemPanel(waveNumber);
            
            geninventory = true;
        }
    }

    private void setTimerText()
    {
        timerText.text = currentTime.ToString("0") + " s";
    }

    private void NextWave()
    {
        GameSettings.gameState = GameState.InGame;
        waveNumber += 1;
        GameSettings.waveNumber = waveNumber;
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();

        if (waveNumber % 5 == 0)
        {
            bossSpawner.SpawnBoss();
            timerText.visible = false;
        }
        else
        { 
            timerText.visible = true;
        }
            

        EnemySpawner.healthMultiplier += 0.5f;
        EnemySpawner.spawnTimer -= 0.1f;
        if(EnemySpawner.spawnTimer < 0.1f)
        {
            EnemySpawner.spawnTimer = 0.1f;
        }
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
