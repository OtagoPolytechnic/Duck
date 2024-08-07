using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private VisualElement gameOverUI;
    private VisualElement container;
    private bool playerDead = false;
    void Awake()
    {   
        gameOverUI = GetComponent<UIDocument>().rootVisualElement;
        container = gameOverUI.Q<VisualElement>("Container");
        Button replay = gameOverUI.Q<Button>("Replay");
        replay.RegisterCallback<ClickEvent>(Restart);
        Button quit = gameOverUI.Q<Button>("Quit");
        quit.RegisterCallback<ClickEvent>(MainMenu);
    }
    public void GameOver()
    {
        //This should be handled under a game state end or dead
        if (!playerDead)
        {
            playerDead = true;
            //disables shooting, movement and timer on game over
            FindObjectOfType<Shooting>().enabled = false;
            FindObjectOfType<Timer>().enabled = false;
            FindObjectOfType<TopDownMovement>().enabled = false;
            FindObjectOfType<EnemySpawner>().enabled = false;
            //call game over UI
            //SFXManager.Instance.StopBackgroundMusic();
            SFXManager.Instance.GameOverSound();
            ScoreManager.Instance.FinalScore();
            container.visible = true;
        }
        //call kill all active enemies
        Timer.CullEnemies();
    }

    private void Restart(ClickEvent click)
    {
        ResetVariables();
        SceneManager.LoadScene("UIRemaster");
    }

    private void MainMenu(ClickEvent click)
    {
        SceneManager.LoadScene("Titlescreen");
    }

    private void ResetVariables() //Any static variables that need to be reset on game start should be added to this method
    {
        //Player variables
        EnemyHealth.bleedAmount = 0;

        //Enemy variables
        EnemySpawner.healthMultiplier = 1f;
        EnemySpawner.spawnTimer = 5f;

        //Item stacks
        foreach (Item i in InventoryPage.itemList)
        {
            i.stacks = 0;
        }
    }
}
