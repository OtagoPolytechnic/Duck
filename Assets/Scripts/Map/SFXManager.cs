using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public Transform Camera;
    public AudioClip DuckShooting;
    public AudioClip EnemyShoot;
    public AudioClip Bite;
    public AudioClip DuckHit;
    public AudioClip EnemyDie;
    public AudioClip GameOver;
    public AudioClip TitleScreen;
    public AudioClip WaveMusic;
    private AudioSource audioSource;
 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SFXManager instance set and will not be destroyed on load.");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void DuckShootSound()
    {
        audioSource.PlayOneShot(DuckShooting);
    }

    public void EnemyShootSound()
    {
        audioSource.PlayOneShot(EnemyShoot);
    }

    public void EnemyBiteSound()
    {
        audioSource.PlayOneShot(Bite);
    }

    public void DuckHitSound()
    {
        audioSource.PlayOneShot(DuckHit);
    }

    public void EnemyDieSound()
    {
        audioSource.PlayOneShot(EnemyDie);
    }
    
    public void GameOverSound()
    {
        audioSource.PlayOneShot(GameOver);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        // Check if the music is already playing and if it's the same clip
        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return; // Do nothing if the same music is already playing
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScreen")
        {
            PlayBackgroundMusic(TitleScreen);
        }
        else if (scene.name == "MainScene")
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }
}