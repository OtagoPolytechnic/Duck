using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

    public void HighscoreButton()
    {
        SceneManager.LoadScene("Highscores");
    }


}


