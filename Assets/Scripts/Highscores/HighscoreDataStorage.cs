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
[Serializable]
public enum WeaponType
{
    Pistol,
    DualPistol,
    Shotgun,
    Sniper,
    Machine
};

[Serializable]
public class ItemStorage
{
    public string name;
    public rarity rarity;
    public int stacks;
}

[Serializable]
public class EntryData
{
    public string entryName;
    public int entryScore;
    public WeaponType weapon;
    public int waveNumber; //This is only used in the endless mode. Wave number user died on.
    public List<ItemStorage> items; //Items collected in the run
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


    public EntryData(string name, int score, WeaponType weapon, int waveNumber, List<Item> itemInput, int enemiesKilled) //Constructor for endless mode
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        this.waveNumber = waveNumber;
        items = new List<ItemStorage>();
        foreach (Item item in itemInput)
        {
            ItemStorage itemStorage = new ItemStorage();
            itemStorage.name = item.name;
            itemStorage.rarity = item.rarity;
            itemStorage.stacks = item.stacks;
            items.Add(itemStorage);
        }
        this.enemiesKilled = enemiesKilled;
        dateTime = DateTime.Now;
    }

    public EntryData(string name, int score, WeaponType weapon, List<Item> itemInput, int enemiesKilled) //Constructor for boss mode with no level
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        items = new List<ItemStorage>();
        foreach (Item item in itemInput)
        {
            ItemStorage itemStorage = new ItemStorage();
            itemStorage.name = item.name;
            itemStorage.rarity = item.rarity;
            itemStorage.stacks = item.stacks;
            items.Add(itemStorage);
        }
        this.enemiesKilled = enemiesKilled;
        dateTime = DateTime.Now;
    }

    //Return formatted list of all info from the record
    public List<string> GetEntryInfo()
    {
        List<string> info = new List<string>();
        info.Add("Name: " + entryName + "\t"); //Tab ensures that the score takes up the whole space. Hacky way of doing it but I can't figure out how to edit the row templates.
        info.Add("Score: " + entryScore);
        info.Add("Weapon: " + weapon);
        if (waveNumber != 0)
        {
            info.Add("Wave Number: " + waveNumber);
        }
        info.Add("Items:");
        if (items == null || items.Count == 0)
        {
            info.Add(" None");
        }
        else
        {
            foreach (ItemStorage item in items)
            {
                info.Add(" "+ item.stacks + "x" + item.name);
            }
        }
        info.Add("Killed: " + enemiesKilled);
        info.Add("Date: " + DateFormatted);
        info.Add("Time: " + TimeFormatted);
        return info;
    }
}
