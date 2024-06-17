using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveMusic : MonoBehaviour
{
    public AudioClip WaveMusic;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WaveSound(float volume = 0.1f)
    {
        PlayBackgroundMusic(WaveMusic, volume);
    }


    public void PlayBackgroundMusic(AudioClip clip, float volume = 0.1f)
    {
        // Check if the music is already playing and if it's the same clip
        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return; // Do nothing if the same music is already playing
        }

        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop background music if the main scene is loaded
        if (scene.name == "MainScene")
        {
            PlayBackgroundMusic(WaveMusic);

        }
    }
}

