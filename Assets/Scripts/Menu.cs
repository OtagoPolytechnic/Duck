using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{

    void Awake()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        Button playButton = document.Q("PlayButton") as Button;
        playButton.RegisterCallback<ClickEvent>(Play);
        Button highscoreButton = document.Q("HighscoreButton") as Button;
        highscoreButton.RegisterCallback<ClickEvent>(Highscore);
        Button tutorialButton = document.Q("TutorialButton") as Button;
        tutorialButton.RegisterCallback<ClickEvent>(Tutorial);
        Button quitButton = document.Q("QuitButton") as Button;
        quitButton.RegisterCallback<ClickEvent>(Quit);

    }
    private void Start()
    {
        SFXManager.Instance.PlayBackgroundMusic(SFXManager.Instance.TitleScreen);
    }

    public void Play(ClickEvent click)
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit(ClickEvent click)
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

    public void Tutorial(ClickEvent click)
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Highscore(ClickEvent click)
    {
        SceneManager.LoadScene("Highscores");
    }
}


