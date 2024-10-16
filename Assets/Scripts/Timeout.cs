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
