using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

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

    public AudioMixer audioMixer;
    public AudioMixerGroup playerShootGroup;
    public AudioMixerGroup enemyBiteGroup;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
    
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

        // Assign a default AudioMixerGroup if needed
        audioSource.outputAudioMixerGroup = playerShootGroup;
    }

    public void DuckShootSound()
    {
        audioSource.outputAudioMixerGroup = playerShootGroup;
        audioSource.PlayOneShot(DuckShooting);
    }

    public void EnemyShootSound()
    {
        audioSource.outputAudioMixerGroup = playerShootGroup;
        audioSource.PlayOneShot(EnemyShoot);
    }

    public void EnemyBiteSound()
    {
        audioSource.outputAudioMixerGroup = enemyBiteGroup;
        audioSource.PlayOneShot(Bite);
    }

    public void DuckHitSound()
    {
        audioSource.outputAudioMixerGroup = playerShootGroup;
        audioSource.PlayOneShot(DuckHit);
    }

    public void EnemyDieSound()
    {
        audioSource.outputAudioMixerGroup = playerShootGroup;
        audioSource.PlayOneShot(EnemyDie);
    }

    public void GameOverSound()
    {
        audioSource.outputAudioMixerGroup = playerShootGroup;
        audioSource.PlayOneShot(GameOver);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;
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
