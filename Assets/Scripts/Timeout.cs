using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeout : MonoBehaviour
{
    public static Timeout Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public IEnumerator TimeoutEnemy(GameObject enemy, float duration)
    {
        Debug.Log("This is being called correctly");
        SpriteRenderer sprite = enemy.GetComponentInChildren<SpriteRenderer>();
        
        if (sprite != null)
        {
            sprite.color = new Color32(255, 0, 0, 128);
        }
        else
        {
            Debug.LogError("Did not get sprite, make sure enemy hierarchy has a sprite render");
        }

        yield return new WaitForSeconds(duration);
        
        Debug.Log("This is culling the eneimes correctly");
        EnemySpawner.Instance.currentEnemies.Remove(enemy);
        Destroy(enemy);

    }

    public IEnumerator TimeoutPlayer(GameObject player, float duration)
    {

        SpriteRenderer sprite = player.GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = new Color32(255, 0, 0, 128);
        }
        else
        {
            Debug.LogError("Did not get sprite, make sure player hierarchy has a sprite render");
        }


        StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(duration));

        yield return new WaitForSeconds(duration);
        GameSettings.gameState = GameState.InGame;

        if (PlayerStats.Instance.lifeEggs.Count > 0)
        {
            PlayerStats.Instance.Respawn();
        }
        else
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }
}
