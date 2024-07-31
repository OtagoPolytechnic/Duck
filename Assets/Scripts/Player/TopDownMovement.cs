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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
