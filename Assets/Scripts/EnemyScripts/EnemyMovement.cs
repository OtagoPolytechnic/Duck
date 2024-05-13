using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float distance;
    [SerializeField] private float attackRange;
    
    private MapManager mapManager;
    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        //turns enemy towards player
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        if (distance >= attackRange)
        { 
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (speed * tileSpeedModifier) * Time.deltaTime);
        }
        
    }
}
