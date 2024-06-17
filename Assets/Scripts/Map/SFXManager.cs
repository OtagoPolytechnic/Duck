using System.Collections;
using System.Collections.Generic;
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

    public void DuckShootSound(float volume = 1.0f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(DuckShooting);
    }
    public void EnemyShootSound(float volume = 5.0f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(EnemyShoot);
    }
    public void EnemyBiteSound(float volume = 0.4f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(Bite);
    }
    public void DuckHitSound(float volume = 1.0f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(DuckHit);
    }
    public void EnemyDieSound(float volume = 3.0f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(EnemyDie);
    }

    public void GameOverSound(float volume = 1.0f)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(GameOver);
    }
  public void TitleScreenSound(float volume = 1.0f)
{
    audioSource.volume = volume;
    audioSource.PlayOneShot(TitleScreen);
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
        // Stop background music if the main scene is loaded
        if (scene.name == "MainScene" && audioSource.isPlaying)
        {
            PlayBackgroundMusic(WaveMusic);

        }
        else if ((scene.name == "TitleScreen" || scene.name == "Tutorial" || scene.name == "Highscores"))
        {
            PlayBackgroundMusic(TitleScreen);
        }
    }
}