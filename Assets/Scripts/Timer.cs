using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float waveLength;
    public float currentTime;
    public bool running;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = waveLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(running){
            currentTime -= Time.deltaTime;
        }
        if(currentTime <= 0){
            running = false;
            currentTime = 0;
        }

        setTimerText();
    }

    private void setTimerText()
    {
        timerText.text = currentTime.ToString("0");
    }
}
