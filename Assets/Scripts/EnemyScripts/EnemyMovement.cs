using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;

    private MapManager mapManager;
    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == patrolPoints[targetPoint].position)
        {
            increaseTargetInt();
        }

        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, (speed*tileSpeedModifier) * Time.deltaTime);
    }

    void increaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}
