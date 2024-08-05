using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        SFXManager.Instance.PlayBackgroundMusic(SFXManager.Instance.TitleScreen);
    }

    public void Play()
    {
        GameSettings.gameState = GameState.InGame;
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        GameSettings.gameState = GameState.InGame;
        SceneManager.LoadScene("Tutorial");
    }

    public void HighscoreButton()
    {
        SceneManager.LoadScene("Highscores");
    }
}


