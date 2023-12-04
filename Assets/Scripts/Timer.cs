using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float waveLength;
    public float currentTime;
    public int waveNumber;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = waveLength;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        
        if(currentTime <= 0){
            nextWave();
        }

        setTimerText();
    }

    private void setTimerText()
    {
        timerText.text = currentTime.ToString("0");
    }

    private void nextWave()
    {
        waveNumber += 1;
        currentTime = waveLength;
    }
}
