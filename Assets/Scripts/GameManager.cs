using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
   private bool playerDead = false;
   public void GameOver()
   {
        if (!playerDead)
        {
            playerDead = true;
            //disables shooting, movement and timer on game over
            FindObjectOfType<Shooting>().enabled = false;
            FindObjectOfType<Timer>().enabled = false;
            FindObjectOfType<TopDownMovement>().enabled = false;
            FindObjectOfType<EnemySpawner>().enabled = false;
            //call kill all active enemies
            //call game over UI
            gameOverUI.SetActive(true);
            //Debug.Log("Game Over");
        }
   }

     public void Restart() 
   {
        // Load current scene
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
        PlayerHealth.regenAmount = 0;
        PlayerHealth.regenTrue = false;
        PlayerHealth.lifestealAmount = 0;
        PlayerHealth.damage = 20;
        PlayerHealth.explosionSize = 0;
        PlayerHealth.explosiveBullets = false;
        PlayerHealth.critChance = 0.01f;
        PlayerHealth.hasShotgun = false;
        PlayerHealth.bulletAmount = 0;

        TopDownMovement.moveSpeed = 10f;

        EnemyHealth.bleedTrue = false;
        EnemyHealth.bleedAmount = 0;

        //Enemy variables
        EnemySpawner.healthMultiplier = 1f;
        EnemySpawner.spawnTimer = 5f;
    }
}
