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
}
