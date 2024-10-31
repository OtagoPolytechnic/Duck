using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float damageInterval;
    private float damageTick;
    // Start is called before the first frame update
    void Start()
    {
        damageTick = damageInterval;
        //SFXManager.Instance.PlaySFX("Radiation");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSettings.gameState != GameState.InGame) {return;}

        damageTick -= Time.deltaTime;
        if (damageTick <= 0)
        {
            DealDamage();
            damageTick = damageInterval;
        }

        duration -= Time.deltaTime;
        if (duration <= 0) {Destroy(gameObject);}
    }

    private void DealDamage()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2);
        foreach (Collider2D c in overlaps)
        {
            if (c.CompareTag("Enemy"))
            {
                c.GetComponent<EnemyBase>().ReceiveDamage(WeaponStats.Instance.RadiationDamage, false);
            }
        }
    }
}
