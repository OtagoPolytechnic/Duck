using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string Name();
    public abstract string Rarity();
    public virtual void OnTick(PlayerItems playeritems, int level) //requires player connection to be able to be called and used i.e (Player player)
    {

    }
    public virtual void OnPickup(PlayerItems playeritems, int level) //also needs the above player connection
    {

    }
    public virtual void OnHit(int level) //requires enemy connection and the player connection to be able to be called and used i.e (Player player, Enemy enemy)
    {

    }
}

public class Regen: Item
{
    public override string Name()
    {
        return "Regen";
    }
    public override string Rarity()
    {
        return "Common";
    }
    public override void OnTick(PlayerItems playeritems, int level) //also needs the above player connection
    {
        playeritems.health += 1 + ( 4 * level - 1 );
    }
}
public class DamageIncrease : Item
{
    public override string Name()
    {
        return "Damage Increase";
    }
    public override string Rarity()
    {
        return "Common";
    }
    public override void OnPickup(PlayerItems playeritems, int level) //also needs the above player connection
    {
        //player.damage += player.damage * level;
    }
}
public class Speed : Item
{
    public override string Name()
    {
        return "Speed";
    }
    public override string Rarity()
    {
        return "Common";
    }
    public override void OnPickup(PlayerItems playeritems, int level) //also needs the above player connection
    {
        //player.speed += player.speed * level;
    }
}
public class Bleed : Item //needs to be changed to on hit but also update, will revise
{
    public override string Name()
    {
        return "Bleed on hit";
    }
    public override string Rarity()
    {
        return "Common";
    }
    public override void OnHit(int level) //needs player and enemy connection
    {
        //enemy.health -= 5 * level;
    }
}
