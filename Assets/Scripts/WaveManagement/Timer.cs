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
    public float currentTime;
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
        if(GameSettings.gameState == GameState.InGame && GameSettings.waveNumber % 5 != 0 || TerminalBehaviour.Instance.stopBoss)
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
        
        if(currentTime <= 0 || (BossHealth.Instance.boss !=null && BossHealth.Instance.boss.Health <=0))
        {
            Debug.Log(BossHealth.Instance.boss);
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
    	
        CullBullets();
        if (waveNumber % 5 == 4)
        {
            CullEnemies();
        }
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

        if (waveNumber % 5 == 0 && !TerminalBehaviour.Instance.stopBoss)
        {
            bossSpawner.SpawnBoss();
            timerText.visible = false;
        }
        else
        { 
            timerText.visible = true;
        }
            
        geninventory = false;
        itemPanel.itemChosen = false;

        //Enemy scaling

        if (waveNumber % 5 == 0)
        {
            EnemySpawner.Instance.EnemyLevel++;
        }

        if (EnemySpawner.Instance.SpawnTimer > 0.1f)
        {
            EnemySpawner.Instance.SpawnTimer -= 0.1f;
        }
        else
        {
            EnemySpawner.Instance.EnemyCap += 1;
        }

        //Scale stats if after wave 25
        if (waveNumber > 25)
        {
            EnemyBase.endlessScalar += 0.1f;
        }
    }

    public static void CullEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemySpawner.Instance.currentEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }
    public static void CullBullets()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}
