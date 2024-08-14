using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighscoreSaveData 
{
    public List<EntryData> highscores = new List<EntryData>();
}

//I will remove this when merged with the weapon update code. Will use the enum from that.
public enum WeaponType
{ 
    Pistol,
    DualPistol,
    Shotgun,
    Sniper,
    Machine
};

[Serializable]
public class EntryData 
{
    public string entryName;
    public int entryScore;
    public WeaponType weapon;
    public int waveNumber; //This is only used in the endless mode. Wave number user died on.
    public List<Item> items; //Items collected in the run
    public int enemiesKilled;

    public EntryData(string name, int score, WeaponType weapon, int waveNumber, List<Item> items, int enemiesKilled) //Constructor for endless mode
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        this.waveNumber = waveNumber;
        this.items = items;
        this.enemiesKilled = enemiesKilled;
    }

    public EntryData(string name, int score, WeaponType weapon, List<Item> items, int enemiesKilled) //Constructor for boss mode with no level
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        this.items = items;
        this.enemiesKilled = enemiesKilled;
    }
}
