using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public Bullet bullet;
    public PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemPicked(string textName)
    {
        Debug.Log(textName);

        switch(textName) 
        {
            case "Damage Increase": //goes to Bullet.cs currently bugged as of 30/04
                bullet.damageModifier += 10;
            break;
            case "Health Increase":
                playerHealth.maxHealth += playerHealth.maxHealth * 1.10f;
            break;
        }
    }
}
