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
    private Button replay;
    private Button quit;
    private Button submit;
    private TextField playerName;
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
        replay = gameOverUI.Q<Button>("Replay");
        replay.RegisterCallback<ClickEvent>(Restart);
        replay.RegisterCallback<NavigationSubmitEvent>(Restart);
        quit = gameOverUI.Q<Button>("Quit");
        quit.RegisterCallback<ClickEvent>(MainMenu);
        quit.RegisterCallback<NavigationSubmitEvent>(MainMenu);
        submit = gameOverUI.Q<Button>("SubmitScore");
        playerName = gameOverUI.Q<TextField>("PlayerName");
        navigationSetting();
    }

    
    private void navigationSetting()
    {
        replay.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: submit.Focus(); break;
                case NavigationMoveEvent.Direction.Down: quit.Focus(); break;
                case NavigationMoveEvent.Direction.Left: replay.Focus(); break;
                case NavigationMoveEvent.Direction.Right: replay.Focus(); break;
            }
            e.PreventDefault();
        });

        quit.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: replay.Focus(); break;
                case NavigationMoveEvent.Direction.Down: playerName.Focus(); break;
                case NavigationMoveEvent.Direction.Left: quit.Focus(); break;
                case NavigationMoveEvent.Direction.Right: quit.Focus(); break;
            }
            e.PreventDefault();
        });

        submit.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: playerName.Focus(); break;
                case NavigationMoveEvent.Direction.Down: replay.Focus(); break;
                case NavigationMoveEvent.Direction.Left: submit.Focus(); break;
                case NavigationMoveEvent.Direction.Right: submit.Focus(); break;
            }
            e.PreventDefault();
        });

        playerName.RegisterCallback<NavigationMoveEvent>(e =>
        {
            //I want WASD to not trigger the navigation event so the player can type their name
            if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                return;
            }
            switch(e.direction)
            {
                case NavigationMoveEvent.Direction.Up: quit.Focus(); break;
                case NavigationMoveEvent.Direction.Down: submit.Focus(); break;
                case NavigationMoveEvent.Direction.Left: playerName.Focus(); break;
                case NavigationMoveEvent.Direction.Right: playerName.Focus(); break;
            }
            e.PreventDefault();
        });
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
