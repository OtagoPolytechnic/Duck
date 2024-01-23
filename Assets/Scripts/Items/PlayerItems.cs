using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public int health;

    public List<ItemList> items = new List<ItemList>();
    // Start is called before the first frame update
    void Start()
    {
        Regen item = new Regen();
        items.Add(new ItemList(item, item.Name(), item.Rarity(), 1)); //this should be done when the round has ended, here for testing
        StartCoroutine(CallItemOnTick());    
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    IEnumerator CallItemOnTick()
    {
        foreach (ItemList i in items)
        {
            i.item.OnTick(this, i.level);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemOnTick());
    }
}