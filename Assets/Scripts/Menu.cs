using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{

    private Button playButton;
    private Button highscoreButton;
    private Button tutorialButton;
    private Button quitButton;
    private Label versionNumber;
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        playButton = document.Q("PlayButton") as Button;
        playButton.RegisterCallback<ClickEvent>(Play);

        highscoreButton = document.Q<Button>("HighscoreButton");
        tutorialButton = document.Q<Button>("TutorialButton");

        quitButton = document.Q<Button>("QuitButton");
        quitButton.RegisterCallback<ClickEvent>(Quit);

        versionNumber = document.Q<Label>("VersionNumber");
        versionNumber.text = "Alpha V0.9.0";

    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        SFXManager.Instance.PlayBackgroundMusic(SFXManager.Instance.TitleScreen);
        //Load the highscores and tutorial scenes in the background
        StartCoroutine(LoadBackgroundScene("Highscores", Highscore, highscoreButton));
        StartCoroutine(LoadBackgroundScene("Tutorial", Tutorial, tutorialButton));
    }

    private void OnDisable()
    {
        playButton.UnregisterCallback<ClickEvent>(Play);
        highscoreButton.UnregisterCallback<ClickEvent>(Highscore);
        tutorialButton.UnregisterCallback<ClickEvent>(Tutorial);
        quitButton.UnregisterCallback<ClickEvent>(Quit);
    }

    IEnumerator LoadBackgroundScene(string sceneName, EventCallback<ClickEvent> click, Button button)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            button.RegisterCallback<ClickEvent>(click);
            yield return null;
        }
    }

    public void Play(ClickEvent click)
    {
        GameSettings.gameState = GameState.InGame;
        StartCoroutine(LoadScene("Palin-MainScene"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        //TODO: Add loading screen
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Quit(ClickEvent click)
    {
        Application.Quit();
    }

    public void Tutorial(ClickEvent click)
    {
        //TODO: Set UI document of Tutorial scene to visible
        
    }

    public void Highscore(ClickEvent click)
    {
        //TODO: Set UI document of Highscore scene to visible
    }
}


