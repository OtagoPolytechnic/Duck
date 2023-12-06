using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    public List<ItemList> items = new List<ItemList>();
    // Start is called before the first frame update
    void Start()
    {
        DamageIncrease item = new DamageIncrease();
        items.Add(new ItemList(item, item.Name(), item.Rarity(), 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
