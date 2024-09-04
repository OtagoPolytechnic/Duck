using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement : MonoBehaviour
{
    public static TopDownMovement Instance;

    private const float BASE_MOVE_SPEED = 10f;
    private float flatBonusSpeed = 0;
    public float FlatBonusSpeed
    {
        get {return flatBonusSpeed;}
        set {flatBonusSpeed = value;}
    }
    private float percentBonusSpeed = 100f;
    public float PercentBonusSpeed
    {
        get {return percentBonusSpeed;}
        set {percentBonusSpeed = value;}
    }
    public float MoveSpeed
    {
        get {return (BASE_MOVE_SPEED + FlatBonusSpeed) * (PercentBonusSpeed / 100);}
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
        if (GameSettings.gameState != GameState.InGame)
        {
            hitBox.velocity = Vector2.zero;
            return;
        }
        
        if(mapManager != null)
        {
            float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
            hitBox.velocity = moveInput * (MoveSpeed * tileSpeedModifier);
        }
        else
        {
            hitBox.velocity = moveInput * MoveSpeed;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
