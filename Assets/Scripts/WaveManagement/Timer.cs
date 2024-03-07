using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI timerText;
    public float waveLength;
    public float currentTime;
    public int waveNumber;
    public bool running;
    private GameObject[] spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = waveLength;
        waveNumberText.text = "Wave: " + waveNumber.ToString();
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
        Debug.Log(spawnPoints);
    }

    // Update is called once per frame
    void Update()
    {
        if(running)
        {
            currentTime -= Time.deltaTime;
        }
        else if(Input.GetKeyDown("space"))
        {
            nextWave();
        }
        
        if(currentTime <= 0)
        {
            running = false;
        }

        setTimerText();
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
        running = true;

        for (int i=0; i<spawnPoints.Length; i++){
            spawnPoints[i].GetComponent<EnemySpawner>().enemyHealth += 10;
            spawnPoints[i].GetComponent<EnemySpawner>().spawnTimer -= 0.1f;
        }
    }
}
