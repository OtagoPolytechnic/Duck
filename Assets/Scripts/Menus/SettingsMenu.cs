using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class SettingsMenu : MonoBehaviour
{
    private VisualElement document;
    public AudioMixer audioMixer;
    private Slider mainVolume;
    private Slider musicVolume;
    private Slider sfxVolume;
    private DropdownField resolutionDropdown;
    private List<string> resolutions = new List<string>();

    private const string MAIN_VOL_PREF = "MainVolume"; //Keys for the playerprefs
    private const string MUSIC_VOL_PREF = "MusicVolume";
    private const string SFX_VOL_PREF = "SFXVolume";
    private const string RESOLUTION_X_PREF = "ResolutionX";
    private const string RESOLUTION_Y_PREF = "ResolutionY";

    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        Button goBack = document.Q<Button>("Return");
        goBack.RegisterCallback<ClickEvent>(Return);

        mainVolume = document.Q<Slider>("MainVolume");
        musicVolume = document.Q<Slider>("MusicVolume");
        sfxVolume = document.Q<Slider>("SFXVolume");
        resolutionDropdown = document.Q<DropdownField>("ResolutionDropdown");
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

        PopulateResolutions();

        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        //Check scene and play appropriate music
        if (SceneManager.GetActiveScene().name == "Titlescreen")
        {
            SFXManager.Instance.PlayBackgroundMusic("TitleMusic");
        }
    }
    private void Return(ClickEvent click)
    {
        SFXManager.Instance.PlaySFX("ButtonPress");
        PlayerPrefs.Save(); // Save the playerprefs on menu close. They save automatically when application exits as well
        //if MainScene is loaded
        if (SceneManager.GetSceneByName("MainScene").isLoaded)
        {
            //If in game, unload the settings scene
            SceneManager.UnloadSceneAsync("Settings");
        }
        else //This means that the user is in the main menu
        {
            if (document != null)
            {
                document.style.display = DisplayStyle.None;
            }
        }
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

    public void PopulateResolutions()
    {
        Resolution currentRes = new Resolution();
        currentRes.width = PlayerPrefs.GetInt(RESOLUTION_X_PREF, Screen.currentResolution.width);
        currentRes.height = PlayerPrefs.GetInt(RESOLUTION_Y_PREF, Screen.currentResolution.height);
        resolutions = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution resolution = Screen.resolutions[i];
            string option = resolution.width + " x " + resolution.height;
            if (!resolutions.Contains(option)) //If this resolution is not already in the list, add it
            {
                resolutions.Add(option);
            }
        }
        resolutionDropdown.choices = resolutions;
        resolutionDropdown.value = currentRes.width + " x " + currentRes.height;
        SetResolution(currentRes.width + " x " + currentRes.height);
        resolutionDropdown.RegisterValueChangedCallback(evt => SetResolution(evt.newValue));

    }

    public void SetResolution(string resolution) 
    {
        string[] res = resolution.Split('x');
        Resolution newRes = new Resolution();
        newRes.width = int.Parse(res[0]);
        newRes.height = int.Parse(res[1]);
        Screen.SetResolution(newRes.width, newRes.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_X_PREF, newRes.width);
        PlayerPrefs.SetInt(RESOLUTION_Y_PREF, newRes.height);
    }
}
