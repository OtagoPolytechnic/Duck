using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public Bullet bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemPicked(string textName, int stacks)
    {
        Debug.Log(textName);

        switch(textName) 
        {
            case "Damage Increase": //goes to Bullet.cs currently bugged as of 30/04
                bullet.damageModifier += 10 + stacks;
            break;
            case "Health Increase":
                PlayerHealth.maxHealth *= 1.10f;
                Debug.Log(PlayerHealth.maxHealth);
            break;
            default:
                Debug.LogError("No item was picked, either there is a new item added that hasn't been mirrored here or an item's name is incorrect.");
            break;
        }
    }
}
