using System.Collections;
using UnityEngine;

public class Blades : MonoBehaviour
{
    public GameObject player;
    private Vector3 initialPosition;
    private float moveSpeed = 2.5f; 
    private GameObject bladesCenter; 
    private bool isMovingIn = false;
    private bool isMovingOut = false;
    private bool isCharging = false;
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
        StartCoroutine(MoveBlades());
        bladeDamage = 5 + (GameSettings.waveNumber / 5) * 5;
    }

    void Update()
    {
        if (isMovingIn && bladesCenter != null)
        {
          
            transform.position = Vector3.MoveTowards(transform.position, bladesCenter.transform.position, moveSpeed * Time.deltaTime);

         
            if (Vector3.Distance(transform.position, bladesCenter.transform.position) < 0.1f)
            {
                isMovingIn = false;
                StartCoroutine(WaitAndMoveOut());
            }
        }
        else if (isMovingOut)
        {
          
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

          
            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                isMovingOut = false; 
            }
        }
    }

    private IEnumerator MoveBlades()
    {
        while (true)
        {        
            yield return new WaitForSeconds(5f);
            isMovingIn = true; 
            yield return new WaitForSeconds(7f); 
            isMovingIn = false; 
            yield return new WaitForSeconds(5f);
            isMovingOut = true; 
        }
    }

    private IEnumerator WaitAndMoveOut()
    {
        yield return new WaitForSeconds(5f);
        isMovingOut = true; 
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().ReceiveDamage(bladeDamage, originEnemy);
           
        }
        else if (other.gameObject.CompareTag("Edges") || other.gameObject.CompareTag("Decoy"))
        {
            Destroy(gameObject);
        }
    }
}
