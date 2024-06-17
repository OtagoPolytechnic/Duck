using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public ScoreManager scoreManager;
   private bool playerDead = false;
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
            scoreManager.FinalScore();
            gameOverUI.SetActive(true);
        }
        //call kill all active enemies
        Timer.CullEnemies();
   }

    public void Restart() 
    {
        ResetVariables();
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("Titlescreen");
    }

    public void ResetVariables() //Any static variables that need to be reset on game start should be added to this method
    {
        //Player variables
        PlayerHealth.maxHealth = 100;
        PlayerHealth.regenAmount = 0;
        PlayerHealth.regenTrue = false;
        PlayerHealth.lifestealAmount = 0;
        PlayerHealth.damage = 20;
        PlayerHealth.explosionSize = 0;
        PlayerHealth.explosiveBullets = false;
        PlayerHealth.critChance = 0.01f;
        PlayerHealth.hasShotgun = false;
        PlayerHealth.bulletAmount = 0;

        Shooting.firerate = 0.5f;

        TopDownMovement.moveSpeed = 10f;

        PlayerHealth.bleedTrue = false;
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
