using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public static float moveSpeed = 10f;
    public Rigidbody2D hitBox;
    private Vector2 moveInput;

    private MapManager mapManager;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    void Start()
    {
        hitBox = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame)
        {
            hitBox.velocity = Vector2.zero;
            return;
        }
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        if(mapManager != null)
        {
            float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
            hitBox.velocity = moveInput * (moveSpeed * tileSpeedModifier);
        }
        else
        {
            hitBox.velocity = moveInput * moveSpeed;
        }
    }
}
