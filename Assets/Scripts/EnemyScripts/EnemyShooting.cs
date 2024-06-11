using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//This scripts functionality has been moved to EnemyRanged.cs, this script is now deprecated
//This script will be deleted

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
