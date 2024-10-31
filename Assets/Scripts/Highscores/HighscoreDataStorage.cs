using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighscoreSaveData
{
    public List<EntryData> highscores = new List<EntryData>();
}

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
    public WeaponType weapon; //Weapon used
    public int waveNumber; //Wave number reached
    public bool endBossKilled; //If the end boss was killed
    public List<ItemStorage> items; //Items collected in the run
    public int enemiesKilled;
    public GameMode gameMode;
    public string GameVersion;
    public SkillEnum skill;

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

    public EntryData(string name, int score, WeaponType weapon, int waveNumber, List<Item> itemInput, int enemiesKilled, bool endBossKilled, GameMode gameMode, SkillEnum skill)
    {
        entryName = name;
        entryScore = score;
        this.weapon = weapon;
        this.waveNumber = waveNumber;
        this.endBossKilled = endBossKilled;
        this.gameMode = gameMode;
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
        GameVersion = Application.version;
        this.skill = skill;
    }

    //Return formatted list of all info from the record
    public List<string> GetEntryInfo()
    {
        List<string> info = new List<string>();
        info.Add("Name: " + entryName + "\t"); //Tab ensures that the score takes up the whole space. Hacky way of doing it but I can't figure out how to edit the row templates.
        info.Add("Score: " + entryScore);
        if (weapon == WeaponType.RocketLauncher)
        {
            info.Add("Weapon: " + "Rocket Launcher");
        }
        else if (weapon == WeaponType.MachineGun)
        {
            info.Add("Weapon: " + "Machine Gun");
        }
        else if (weapon == WeaponType.DualPistol)
        {
            info.Add("Weapon: " + "Dual Pistol");
        }
        else
        {
            info.Add("Weapon: " + weapon);
        }
        info.Add("Skill: " + char.ToUpper(skill.ToString()[0]) + skill.ToString().Substring(1)); //Capitalize the first letter
        if (endBossKilled && gameMode == GameMode.Boss)
        {
            info.Add("Final Boss Killed");
        }
        else
        {
            info.Add("Wave died: " + waveNumber);
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
                info.Add(" "+ item.stacks + "x" + item.name + "\n");
            }
        }
        info.Add("Killed: " + enemiesKilled+ "\t");
        info.Add("Date: " + DateFormatted);
        info.Add("Time: " + TimeFormatted);
        info.Add("Version: " + GameVersion);
        return info;
    }
}
