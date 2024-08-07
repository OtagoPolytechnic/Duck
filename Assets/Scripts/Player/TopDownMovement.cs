using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public static TopDownMovement Instance;

    private float moveSpeed = 10f;
    public float MoveSpeed
    {
        get {return moveSpeed;}
        set {moveSpeed = value;}
    }
    public Rigidbody2D hitBox;
    private Vector2 moveInput;

    private MapManager mapManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        mapManager = FindObjectOfType<MapManager>();
    }

    void Start()
    {
        hitBox = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
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
