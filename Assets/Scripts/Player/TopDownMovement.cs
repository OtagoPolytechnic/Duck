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

    // Start is called before the first frame update
    void Start()
    {
        // Initialize your variables here if needed
        hitBox = GetComponent<Rigidbody2D>(); // Assuming the Rigidbody2D is attached to the same GameObject
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);

        hitBox.velocity = moveInput * (moveSpeed * tileSpeedModifier);
    }
}
