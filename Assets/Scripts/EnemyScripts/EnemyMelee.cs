using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float distance;
    private float attackRange;
    private MapManager mapManager;
    private bool attacking = false;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
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

        if (!attacking) //This enemy type stops moving to attack
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (speed * tileSpeedModifier) * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        
        if (distance <= attackRange)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        bool attackFinished = false;
        //do the attack
        if (attackFinished)
        {
            StopCoroutine(Attack());
        }
        yield return null;
    }
}
