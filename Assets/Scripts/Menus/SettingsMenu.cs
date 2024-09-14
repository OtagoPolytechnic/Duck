using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    private VisualElement document;
    public AudioMixer audioMixer;
    private Slider mainVolume;
    private Slider musicVolume;
    private Slider sfxVolume;

    private const string MAIN_VOL_PREF = "MainVolume"; //Keys for the playerprefs
    private const string MUSIC_VOL_PREF = "MusicVolume";
    private const string SFX_VOL_PREF = "SFXVolume";
    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<ClickEvent>(ReturnToMainMenu);

        mainVolume = document.Q<Slider>("MainVolume");
        musicVolume = document.Q<Slider>("MusicVolume");
        sfxVolume = document.Q<Slider>("SFXVolume");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Loading from playerprefs
        mainVolume.value = PlayerPrefs.GetFloat(MAIN_VOL_PREF, 0.75f); // Default values of 0.75 if playerprefs are not set
        musicVolume.value = PlayerPrefs.GetFloat(MUSIC_VOL_PREF, 0.75f);
        sfxVolume.value = PlayerPrefs.GetFloat(SFX_VOL_PREF, 0.75f);

        //Setting the audio mixer to the playerprefs values
        SetMainVolume(mainVolume.value);
        SetMusicVolume(musicVolume.value);
        SetSFXVolume(sfxVolume.value);

        // Attach listeners to sliders
        mainVolume.RegisterValueChangedCallback(evt => SetMainVolume(evt.newValue));
        musicVolume.RegisterValueChangedCallback(evt => SetMusicVolume(evt.newValue));
        sfxVolume.RegisterValueChangedCallback(evt => SetSFXVolume(evt.newValue));

        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        SFXManager.Instance.PlayBackgroundMusic("TitleMusic");
    }
    private void ReturnToMainMenu(ClickEvent click)
    {
        if (document != null)
        {
            document.style.display = DisplayStyle.None;
        }
        PlayerPrefs.Save(); // Save the playerprefs on menu close. They save automatically when application exits as well
    }

    public void SetMainVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", Mathf.Log10(volume) * 20); //Converts the slider value to decibels
        if (volume == 0) //Log10(0) sets it to max volume instead so this is manually setting it to the lowest volume
        {
            audioMixer.SetFloat("MainVolume", -80);
        }
        PlayerPrefs.SetFloat(MAIN_VOL_PREF, volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        if (volume == 0)
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }
        PlayerPrefs.SetFloat(MUSIC_VOL_PREF, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        if (volume == 0)
        {
            audioMixer.SetFloat("SFXVolume", -80);
        }
        PlayerPrefs.SetFloat(SFX_VOL_PREF, volume);
    }
}
