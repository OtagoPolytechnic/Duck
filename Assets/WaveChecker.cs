using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    // Reference to the Timer script, which can be assigned via the Inspector


    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log("WaveChecker Update called"); // Debug statement
        int currentWaveNumber = GameSettings.waveNumber;
        Debug.Log($"Current Wave Number: {currentWaveNumber}");
        
    }
}