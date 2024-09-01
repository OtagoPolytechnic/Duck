using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class EnemyWithLevel
{
    public GameObject enemy;
    public int level;
}
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    private int enemyLevel;
    public int EnemyLevel
    {
        get {return enemyLevel;}
        set
        {
            enemyLevel = value;
            foreach(EnemyWithLevel e in allEnemies)
            {
                if (e.level == EnemyLevel)
                {
                    availableEnemies.Add(e.enemy);
                }
            }
        }
    }
    public List<EnemyWithLevel> allEnemies;
    private List<GameObject> availableEnemies = new List<GameObject>();
    private float spawnTimer = 1f; //Time between spawns
    public float SpawnTimer
    {
        get {return spawnTimer;}
        set {spawnTimer = value;}
    }
    private float lastSpawn; //Time since last spawn
    private int enemyCap = 10;
    public int EnemyCap
    {
        get {return enemyCap;}
        set {enemyCap = value;}
    }
    [HideInInspector] public List<GameObject> currentEnemies = new List<GameObject>();

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
        EnemyLevel = 1;
        lastSpawn = spawnTimer;
    }

    void Update()
    {
       
        if(GameSettings.gameState == GameState.InGame && GameSettings.waveNumber%5!=0 && !TerminalBehaviour.Instance.stopEnemy)
        {
            if (lastSpawn > spawnTimer && currentEnemies.Count < enemyCap)
            {
                Spawn(Random.Range(0, availableEnemies.Count));
                lastSpawn = 0;
            }
            else
            {
                lastSpawn += Time.deltaTime;
            }
        }
    }

    public void Spawn(int enemyNum)
    {
        Vector3 location = new Vector3();
        int side = Random.Range(1,5); //Which side of the screen the enemy spawns at
        switch (side)
        {
            case 1:  
                location = Camera.main.ViewportToWorldPoint(new Vector3(1.1f,Random.Range(0f,1f),0));
            break;
            case 2:
                location = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f,Random.Range(0f,1f),0));
            break;
            case 3:
                location = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f,1f),1.1f,0));
            break;
            case 4:
                location = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f,1f),-0.1f,0));
            break;
        }

        GameObject enemy = Instantiate(availableEnemies[enemyNum], location, Quaternion.identity);
        currentEnemies.Add(enemy);
    }
}