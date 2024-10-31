using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;


public class SettingsMenu : MonoBehaviour
{
    private VisualElement document;
    public AudioMixer audioMixer;
    private Slider mainVolume;
    private Slider musicVolume;
    private Slider sfxVolume;
    private Toggle toggleShoot;
    private RadioButton Keyboard;
    private RadioButton Controller;
    private Button goBack;
    private DropdownField resolutionDropdown;
    private List<string> resolutions = new List<string>();

    private const string MAIN_VOL_PREF = "MainVolume"; //Keys for the playerprefs
    private const string MUSIC_VOL_PREF = "MusicVolume";
    private const string SFX_VOL_PREF = "SFXVolume";
    private const string RESOLUTION_X_PREF = "ResolutionX";
    private const string RESOLUTION_Y_PREF = "ResolutionY";
    private const string SHOOT_TOGGLE_PREF = "ShootToggle";

    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        goBack = document.Q<Button>("Return");

        // Query radio buttons by index within the RadioButtonGroup
        RadioButtonGroup radioButtonGroup = document.Q<RadioButtonGroup>("RadioButtonGroup");
        Keyboard = radioButtonGroup[0] as RadioButton;
        Controller = radioButtonGroup[1] as RadioButton;
        goBack.RegisterCallback<ClickEvent>(Return);
        goBack.RegisterCallback<NavigationSubmitEvent>(Return);

        mainVolume = document.Q<Slider>("MainVolume");
        musicVolume = document.Q<Slider>("MusicVolume");
        sfxVolume = document.Q<Slider>("SFXVolume");
        resolutionDropdown = document.Q<DropdownField>("ResolutionDropdown");
        toggleShoot = document.Q<Toggle>("ShootToggle");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Loading from playerprefs
        mainVolume.value = PlayerPrefs.GetFloat(MAIN_VOL_PREF, 0.75f); // Default values of 0.75 if playerprefs are not set
        musicVolume.value = PlayerPrefs.GetFloat(MUSIC_VOL_PREF, 0.75f);
        sfxVolume.value = PlayerPrefs.GetFloat(SFX_VOL_PREF, 0.75f);

        toggleShoot.value = PlayerPrefs.GetInt(SHOOT_TOGGLE_PREF, 0) == 1;

        //Setting the audio mixer to the playerprefs values
        SetMainVolume(mainVolume.value);
        SetMusicVolume(musicVolume.value);
        SetSFXVolume(sfxVolume.value);
        
        //Setting the shoot toggle to the playerprefs value
        SetShootToggle(toggleShoot.value);

        // Attach listeners to sliders
        mainVolume.RegisterValueChangedCallback(evt => SetMainVolume(evt.newValue));
        musicVolume.RegisterValueChangedCallback(evt => SetMusicVolume(evt.newValue));
        sfxVolume.RegisterValueChangedCallback(evt => SetSFXVolume(evt.newValue));

        // Attach listener to toggle
        toggleShoot.RegisterValueChangedCallback(evt => SetShootToggle(evt.newValue));

        PopulateResolutions();

        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        //Check scene and play appropriate music
        if (SceneManager.GetActiveScene().name == "Titlescreen")
        {
            SFXManager.Instance.PlayBackgroundMusic("TitleMusic");
        }
        navigationSetting();
    }

    private void navigationSetting()
    {
        goBack.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: 
                    Controller.Focus();
                    Controller.style.color = new StyleColor(new Color(0, 1, 0));
                    break;
                case NavigationMoveEvent.Direction.Down: mainVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Left: goBack.Focus(); break;
                case NavigationMoveEvent.Direction.Right: goBack.Focus(); break;
            }
            e.PreventDefault();
        });

        Controller.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up:
                    Keyboard.Focus();
                    Keyboard.style.color = new StyleColor(new Color(0, 1, 0));
                    Controller.style.color = new StyleColor(new Color(1, 1, 1));
                    break;
                case NavigationMoveEvent.Direction.Down:
                    goBack.Focus();
                    Controller.style.color = new StyleColor(new Color(1, 1, 1));
                    break;
                case NavigationMoveEvent.Direction.Left: Controller.Focus(); break;
                case NavigationMoveEvent.Direction.Right: Controller.Focus(); break;
            }
            e.PreventDefault();
        });

        Keyboard.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up:
                    resolutionDropdown.Focus();
                    Keyboard.style.color = new StyleColor(new Color(1, 1, 1));
                    break;
                case NavigationMoveEvent.Direction.Down:
                    Controller.Focus();
                    Controller.style.color = new StyleColor(new Color(0, 1, 0));
                    Keyboard.style.color = new StyleColor(new Color(1, 1, 1));
                    break;
                case NavigationMoveEvent.Direction.Left: Keyboard.Focus(); break;
                case NavigationMoveEvent.Direction.Right: Keyboard.Focus(); break;
            }
            e.PreventDefault();
        });

        resolutionDropdown.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: toggleShoot.Focus(); break;
                case NavigationMoveEvent.Direction.Down:
                    Keyboard.Focus();
                    Keyboard.style.color = new StyleColor(new Color(0, 1, 0));
                    break;
                case NavigationMoveEvent.Direction.Left: resolutionDropdown.Focus(); break;
                case NavigationMoveEvent.Direction.Right: resolutionDropdown.Focus(); break;
            }
            e.PreventDefault();
        });

        toggleShoot.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: sfxVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Down: resolutionDropdown.Focus(); break;
                case NavigationMoveEvent.Direction.Left: toggleShoot.Focus(); break;
                case NavigationMoveEvent.Direction.Right: toggleShoot.Focus(); break;
            }
            e.PreventDefault();
        });

        sfxVolume.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: musicVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Down: toggleShoot.Focus(); break;
                case NavigationMoveEvent.Direction.Left: sfxVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Right: sfxVolume.Focus(); break;
            }
            e.PreventDefault();
        });

        musicVolume.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: mainVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Down: sfxVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Left: musicVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Right: musicVolume.Focus(); break;
            }
            e.PreventDefault();
        });

        mainVolume.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: goBack.Focus(); break;
                case NavigationMoveEvent.Direction.Down: musicVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Left: mainVolume.Focus(); break;
                case NavigationMoveEvent.Direction.Right: mainVolume.Focus(); break;
            }
            e.PreventDefault();
        });
    }

    private void Return(EventBase evt)
    {
        SFXManager.Instance.PlayRandomSFX(new string[] {"Button-Press", "Button-Press2", "Button-Press3", "Button-Press4"});
        PlayerPrefs.Save(); // Save the playerprefs on menu close. They save automatically when application exits as well
        //if MainScene is loaded
        if (!SceneManager.GetSceneByName("Titlescreen").isLoaded)
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
            //Makes sure the return button is focused when the settings scene is opened
            if (evt is NavigationSubmitEvent)
            {
                Scene Titlescreen = SceneManager.GetSceneByName("Titlescreen");
                GameObject[] rootObjects = Titlescreen.GetRootGameObjects();
                UIDocument uiDocument = rootObjects
                    .Select(obj => obj.GetComponent<UIDocument>())
                    .FirstOrDefault(doc => doc != null);
                if (uiDocument != null)
                {
                    VisualElement rootElement = uiDocument.rootVisualElement;
                    Button buttonToFocus = rootElement.Query<Button>(className: "focus-button").First();
                    if (buttonToFocus != null)
                    {
                        buttonToFocus.Focus();
                    }
                }
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

    public void SetShootToggle(bool toggle)
    {
        GameSettings.toggleShoot = toggle;
        PlayerPrefs.SetInt(SHOOT_TOGGLE_PREF, toggle ? 1 : 0);
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
