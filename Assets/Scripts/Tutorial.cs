using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        GameSettings.gameState = GameState.InGame;
        SceneManager.LoadScene("Titlescreen");
    }
}
