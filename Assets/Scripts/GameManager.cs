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
            scoreManager.FinalScore();
            gameOverUI.SetActive(true);
            
            //Debug.Log("Game Over");
        }
   }

     public void Restart() 
   {
      // Load current scene
      SceneManager.LoadScene("HighscoreSystem");
   }

   public void MainMenu() 
   {
      SceneManager.LoadScene("Titlescreen");
   }
}
