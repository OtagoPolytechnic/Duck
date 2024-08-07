using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{

    private Button playButton;
    private Button highscoreButton;
    private Button tutorialButton;
    private Button quitButton;
    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        playButton = document.Q<Button>("PlayButton");
        playButton.RegisterCallback<ClickEvent>(Play);

        highscoreButton = document.Q("HighscoreButton") as Button;
        highscoreButton.RegisterCallback<ClickEvent>(Highscore);

        tutorialButton = document.Q("TutorialButton") as Button;
        tutorialButton.RegisterCallback<ClickEvent>(Tutorial);

        quitButton = document.Q("QuitButton") as Button;
        quitButton.RegisterCallback<ClickEvent>(Quit);

    }
    private void OnDisable()
    {
        playButton.UnregisterCallback<ClickEvent>(Play);
        highscoreButton.UnregisterCallback<ClickEvent>(Highscore);
        tutorialButton.UnregisterCallback<ClickEvent>(Tutorial);
        quitButton.UnregisterCallback<ClickEvent>(Quit);
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        SFXManager.Instance.PlayBackgroundMusic(SFXManager.Instance.TitleScreen);
    }

    public void Play(ClickEvent click)
    {
        GameSettings.gameState = GameState.InGame;
        SceneManager.LoadScene("MainScene");
    }

    public void Quit(ClickEvent click)
    {
        Application.Quit();
    }

    public void Tutorial(ClickEvent click)
    {
        GameSettings.gameState = GameState.InGame;
        SceneManager.LoadScene("Tutorial");
    }

    public void Highscore(ClickEvent click)
    {
        SceneManager.LoadScene("Highscores");
    }
}


