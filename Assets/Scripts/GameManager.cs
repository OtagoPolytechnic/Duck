using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private VisualElement gameOverUI;
    private VisualElement container;
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameOverUI = GetComponent<UIDocument>().rootVisualElement;
        container = gameOverUI.Q<VisualElement>("Container");
        Button replay = gameOverUI.Q<Button>("Replay");
        replay.RegisterCallback<ClickEvent>(Restart);
        replay.RegisterCallback<NavigationSubmitEvent>(Restart);
        Button quit = gameOverUI.Q<Button>("Quit");
        quit.RegisterCallback<ClickEvent>(MainMenu);
        quit.RegisterCallback<NavigationSubmitEvent>(MainMenu);
    }

    public void GameOver()
    {
        if (GameSettings.gameState != GameState.EndGame)
        {
            GameSettings.gameState = GameState.EndGame;
            Timer.CullEnemies();
            SFXManager.Instance.PlaySFX("GameOver");
            StartCoroutine(ScoreManager.Instance.FinalScore());
            container.visible = true;
        }
    }

    public void BossVictory()
    {
        if (GameSettings.gameState != GameState.BossVictory)
        {
            GameSettings.gameState = GameState.BossVictory;
            Timer.CullEnemies();
            StartCoroutine(ScoreManager.Instance.FinalScore());
            container.visible = true;
        }
    }

    public void Restart(EventBase evt)
    {
        ResetVariables();
        GameSettings.gameState = GameState.InGame;
        SceneManager.LoadScene("MainScene");
    }

    private void MainMenu(EventBase evt)
    {
        ResetVariables();
        SceneManager.LoadScene("Titlescreen");
    }

    private void ResetVariables() //Any static variables that need to be reset on game start should be added to this method
    {
        //Item stacks
        foreach (Item i in ItemPanel.itemList)
        {
            i.stacks = 0;
        }
    }
}
