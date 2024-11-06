using System.Collections;
using UnityEngine;

public class Blades : MonoBehaviour
{
    public GameObject player;
    private Vector3 initialPosition;
    private float moveSpeed = 2.5f;
    public GameObject bladesCenter;
    private bool isMoving = false;
    private bool moveDirection = false;
    private float attackCooldown;
    private float moveOutDelay = 5f;   
    private int bladeDamage;
    public EnemyBase originEnemy;

    public int BladeDamage
    {
        get { return bladeDamage; }
        set { bladeDamage = value; }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        bladesCenter = GameObject.FindWithTag("BladesCenter");
        attackCooldown = 0; 
        bladeDamage = 5 + GameSettings.waveNumber;
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame) { return; }

        if (isMoving)
        {
            if (moveDirection)
            {
                MoveTowards(bladesCenter.transform.position);
            }
            else
            {
                MoveTowards(initialPosition);
            }
            if (bladesCenter == null)
            {
                bladesCenter = GameObject.FindWithTag("BladesCenter");
            }
            if ((Vector3.Distance(transform.position, bladesCenter.transform.position) < 10f && moveDirection)|| (Vector3.Distance(transform.position, initialPosition) < 0.1f && !moveDirection))
            {
                moveDirection = !moveDirection;
                isMoving = false;
                attackCooldown = moveOutDelay;
            }
        }
        else
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0)
            {
                isMoving = true;
            }
        }
    }



    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(bladeDamage, originEnemy);
        }
        else if (other.gameObject.CompareTag("Edges") )
        {
            Destroy(gameObject);
        }
    }
}
