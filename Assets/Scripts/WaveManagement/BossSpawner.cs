using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private Timer timer; // Reference to the Timer script
    private HashSet<int> specialWaves = new HashSet<int> { 5, 10, 15, 20, 25 };
    private HashSet<int> wavesLogged = new HashSet<int>();

    void Start()
    {
        // Find the Timer component in the scene
        timer = FindObjectOfType<Timer>();

        if (timer == null)
        {
            Debug.LogError("Timer script not found in the scene. Ensure there is a Timer component in the scene.");
        }
    }

    void Update()
    {
        if (timer != null)
        {
            int currentWaveNumber = timer.waveNumber;

            // Check if the current wave number is one of the special waves
            if (specialWaves.Contains(currentWaveNumber) && !wavesLogged.Contains(currentWaveNumber))
            {
                Debug.Log($"Special Wave: Wave {currentWaveNumber}");
                wavesLogged.Add(currentWaveNumber);
            }
        }
    }
}
