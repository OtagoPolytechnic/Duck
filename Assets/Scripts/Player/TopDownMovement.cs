using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed; // Moved these variables outside of any method to make them accessible throughout the class
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize your variables here if needed
        rb2d = GetComponent<Rigidbody2D>(); // Assuming the Rigidbody2D is attached to the same GameObject
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;
    }
}
