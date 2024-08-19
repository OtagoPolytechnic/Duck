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

    //Apparently DateTime can't be saved to the json file so this is the workaround
    [SerializeField]
    private string dateTimeString;
    public DateTime dateTime
    {
        get => DateTime.Parse(dateTimeString);
        set => dateTimeString = value.ToString("o");
    }

    public string DateFormatted => dateTime.ToString("d");
    public string TimeFormatted => dateTime.ToString("t");


    public EntryData(string name, int score, WeaponType weapon, int waveNumber, List<Item> items, int enemiesKilled) //Constructor for endless mode
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        this.waveNumber = waveNumber;
        this.items = items;
        this.enemiesKilled = enemiesKilled;
        dateTime = DateTime.Now;
    }

    public EntryData(string name, int score, WeaponType weapon, List<Item> items, int enemiesKilled) //Constructor for boss mode with no level
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        this.items = items;
        this.enemiesKilled = enemiesKilled;
        dateTime = DateTime.Now;
    }

    //Return formatted list of all info from the record
    public List<string> GetEntryInfo()
    {
        List<string> info = new List<string>();
        info.Add("Name: " + entryName);
        info.Add("Score: " + entryScore);
        info.Add("Weapon: " + weapon);
        if (waveNumber != 0)
        {
            info.Add("Wave Number: " + waveNumber);
        }
        info.Add("Items:");
        if (items == null || items.Count == 0)
        {
            info.Add("\tNone");
        }
        else
        {
            foreach (Item item in items)
            {
                info.Add("\t" + item);
            }
        }
        info.Add("Killed: " + enemiesKilled);
        info.Add("Date: " + DateFormatted);
        info.Add("Time: " + TimeFormatted);
        return info;
    }
}
