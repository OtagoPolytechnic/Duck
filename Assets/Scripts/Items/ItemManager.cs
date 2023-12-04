using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string Name();
    public virtual void Update(int level) //requires player connection to be able to be called and used i.e (Player player)
    {

    }
    public virtual void OnPickup(int level) //requires player connection to be able to be called and used i.e (Player player)
    {

    }
}

public class Healonhit : Item
{
    public override string Name()
    {
        return "Heal on hit";
    }
    public override void Update(int level) //also needs the above player connection
    {
        //player.health += 5 * level;
    }
}
public class DamageIncrease : Item
{
    public override string Name()
    {
        return "Damage Increase";
    }
    public override void OnPickup(int level) //also needs the above player connection
    {
        //player.damage += player.damage * level;
    }
}