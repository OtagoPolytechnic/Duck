using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
   private bool playerDead = false;
   public void GameOver()
   {
        if (!playerDead)
        {
            playerDead = true;
            gameOverUI.SetActive(true);
            Debug.Log("Game Over");
        }
        
   }
}
