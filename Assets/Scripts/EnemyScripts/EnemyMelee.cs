using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    public GameObject player;
    private bool stopCheck;
    private float distance;
    private bool attacking = false;
    [SerializeField] private float attackRange;
    private GameObject attack;

    void Awake()
    {
        ScaleStats();
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        attack = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        Health = BaseHealth;
    }

    void Update()
    {
        if (GameSettings.gameState != GameState.InGame || Dying) {return;}
        Bleed();

        if (SkillEffects.Instance.decoyActive && !stopCheck)
        {
            player = GameObject.FindWithTag("Decoy");
            stopCheck = true;
        }
        else if (!SkillEffects.Instance.decoyActive && stopCheck)
        {
            player = GameObject.FindWithTag("Player");
            stopCheck = false;
        }
        if (!attacking) //This enemy type stops moving to attack
        {
            Move();
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= attackRange)
            {
                if (SkillEffects.Instance.vanishActive) { return; }
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        attack.SetActive(true); //show the attack
        attack.GetComponent<BoxCollider2D>().enabled = true; //enable the collider
                                                             // Play the enemy bite sound
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX("Bite");
        }
        else
        {
            Debug.LogError("SFXManager instance is null in EnemyMelee.Attack().");
        }

        yield return new WaitForSeconds(1f); //Attack duration

        attack.SetActive(false);
        attacking = false;
        StopCoroutine(Attack());
    }

    public override void Move()
    {
        if (SkillEffects.Instance.vanishActive) { return; }

        Vector2 direction = player.transform.position - transform.position;

        //turns enemy towards player
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float tileSpeedModifier = mapManager.GetTileWalkingSpeed(transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, Speed * tileSpeedModifier * Time.deltaTime);
        transform.GetChild(0).rotation = Quaternion.Euler(Vector3.forward * angle);
    }
    // public override void Die()
    // {
    //     SFXManager.Instance.PlaySFX("EnemyDie");
    //     ScoreManager.Instance.IncreasePoints(Points);
    //     EnemySpawner.Instance.currentEnemies.Remove(gameObject);
    //     Destroy(gameObject);
    // }
}