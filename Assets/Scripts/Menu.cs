using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    private void Start()
    {
        // Play the title screen music if we are on the title screen
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayBackgroundMusic(SFXManager.Instance.TitleScreen);
        }
        else
        {
            Debug.LogError("SFXManager instance is null in Menu.Start().");
        }
    }


    public void Play()
    {
        // Stop music when the main scene is loaded
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.StopBackgroundMusic();
        }
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void HighscoreButton()
    {
        SceneManager.LoadScene("Highscores");
    }


}


