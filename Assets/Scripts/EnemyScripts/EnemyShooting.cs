using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyShooting : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform bulletPosition;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            timer = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(enemyBullet, bulletPosition.position, Quaternion.identity);
        
    }
}
