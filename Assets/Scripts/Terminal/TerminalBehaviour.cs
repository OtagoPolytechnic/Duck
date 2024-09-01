using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;

public class TerminalBehaviour : MonoBehaviour
{
    private Label output;
    private TextField input;
    private VisualElement document;
    private VisualElement terminalWindow;
    [SerializeField]
    private ItemEffectTable effectTable;

    public static TerminalBehaviour Instance;
    public bool stopBoss;
    public bool stopEnemy;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        document = GetComponent<UIDocument>().rootVisualElement;
        terminalWindow = document.Q<VisualElement>("TerminalWindow");
        terminalWindow.visible = false;
        input = document.Q<TextField>("Input");
        output = document.Q<Label>("Output");
    }


    public void ActivateWindow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameSettings.gameState == GameState.Paused)
            {
                GameSettings.gameState = GameState.InGame;
                terminalWindow.visible = !terminalWindow.visible; //terminal should be hidden on game start
            }
            else 
            {
                GameSettings.gameState = GameState.Paused;
                terminalWindow.visible = !terminalWindow.visible; //terminal should be hidden on game start
            }
        }
    }

    public void Enter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            string[] commands = input.value.ToLower().Split(" ");
            switch (commands[0])
            {
                case "help":
                    if (commands.Length == 1)
                    {
                        Help(commands[0]);
                    }
                    else
                    {
                        Help(commands[1]);
                    }

                break;
                case "setweapon":
                    SetWeapon(commands[1]);
                break;
                case "giveitem":
                    GiveItem(commands[1], commands[2]);
                break;
                case "setstat":
                    SetStat(commands[1], commands[2]);
                break;
                case "setwave":
                    SetWave(commands[1]);
                break;
                case "godmode":
                    GodMode(commands[1]);
                break;
                case "cull":
                    Cull();
                break;
                case "stopspawn":
                    StopSpawn(commands[1]);
                break;
                case "skipwave":
                    SkipWave();
                break;
                case "spawn":
                    Spawn(commands[1], commands[2]);
                break;
                default:
                    output.text += "\nNot a valid command, type \"help\" to see all available commands\n\n";
                break;
            }   
            input.value = "";
            input.Focus();
            output.text += new string('-', 108);
        }
    }

    public void Help(string command)
    {
        if (command == "help")
        {
            output.text += "\nAvailable Commands: \n\n"+
            "help {command}            \t| will show detailed info about a command\n\n" +
            "setweapon {weaponName}    \t| will set player's weapon to weapon given\n\n" +
            "giveitem {itemID} {amount}\t| will give an item or multiple from the item list to the player\n\n" +
            "setstat {stat} {value}    \t| sets the stat specified to the value given.\n\n" +
            "setwave {waveNo.}         \t| sets the next wave to the specified number\n\n" +
            "skipwave                  \t| will set the timer to 0, skipping the wave you are on\n\n" +
            "godmode {bool}            \t| gives the player god mode.\n\n" +
            "spawn {enemyId} {count}   \t| spawns an enemy with the id given and the amount given\n\n" +
            "cull                      \t| will cull all current enemies and bullets on screen\n\n" +
            "stopspawn {value}         \t| will toggle enemies or bosses from spawning.\n\n";
        }
        else
        {
            switch (command)
            {
                case "setweapon":
                    output.text += "\nsetweapon {weaponName:string}\n\nWill change the players weapon to the name of the weapon provided. Will fail if weapon given does not match a weapon in the enum.\n\nAccepted values:\npistol, \ndualpistol, \nshotgun, \nsniper, \nmachinegun, \nrocket, \nsword\n\n";
                break;
                case "giveitem":
                    output.text += "\ngiveitem {itemID:int} {amount:int}\n\nWill give the player the specified item related to that ID with the amount given(will not add to list of items the player has). Will fail if the ID does not match an item in the list.\n\nAccepted values: see item list\n\n";
                break;
                case "setstat":
                    output.text += "\nsetstat {stat:string {value:int}\n\nWill give the player the increase, specified in value, to the flat bonus modifier of the given stat. Will fail if the stat does not match.\n\nAccepted values:\ndamage, \ncrit, \nmaxhealth, \nfiredelay, \nmovespeed, \ncritdamage\n\n";
                break;
                case "setwave":
                    output.text += "\nsetstat {waveNo.:int}\n\nWill set the next wave to the number given.\n\n";
                break;
                case "godmode":
                    output.text += "\ngodmode {bool:bool}\n\nWill toggle between turning on and off the players ability to be damaged, will not heal the player.\n\n";
                break;
                case "cull":
                    output.text += "\ncull\n\nCulls all active enemies and bullets on stage.\n\n";
                break;
                case "stopspawn":
                    output.text += "\nstopspawn {value:string}\n\nWill stop either enemies or bosses, depeding on value given, from spawning further. Does not cull current eneimes, ues \"cull\" to do that.\n\nAccepted values: \nenemy, \nboss \n\n";
                break;
                case "skipwave":
                    output.text += "\nskipwave\n\nSkips the current wave by setting the timer to 0.1.\n\n";
                break;
                case "spawn":
                    output.text += "\nspawn {enemyID:int} {count:int}\n\nSpawns the amount of eneimes specified in count, and the type of enemy in enemyID.\n\n";
                break;
                default:
                    output.text += "\nNot a valid command, type \"help\" to see all available commands\n\n";
                break;
            }
        }

    }

    private void SetWeapon(string weapon)
    {
        switch (weapon)
        {
            case "pistol":
                WeaponStats.Instance.CurrentWeapon = WeaponType.Pistol;
            break;
            case "dualpistol":
                WeaponStats.Instance.CurrentWeapon = WeaponType.DualPistol;
            break;
            case "shotgun":
                WeaponStats.Instance.CurrentWeapon = WeaponType.Shotgun;
            break;
            case "sniper":
                WeaponStats.Instance.CurrentWeapon = WeaponType.Sniper;
            break;
            case "machinegun":
                WeaponStats.Instance.CurrentWeapon = WeaponType.MachineGun;
            break;
            case "rocket":
                WeaponStats.Instance.CurrentWeapon = WeaponType.RocketLauncher;
            break;
            case "sword":
                WeaponStats.Instance.CurrentWeapon = WeaponType.Sword;
            break;
            default:
                output.text += "\nWeaponName is incorrect\n\n";
            break;
        }
        output.text += $"Weapon set to {WeaponStats.Instance.CurrentWeapon}\n\n";
    }

    private void GiveItem(string itemId, string itemAmount)
    {
        if (!int.TryParse(itemId, out int id))
        {
            output.text += "\nItemId given is incorrect\n\n";
            return;
        }
        if (!int.TryParse(itemAmount, out int amount))
        {
            output.text += "\nAmount given is not a number\n\n";
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            effectTable.ItemPicked(id); //activate the item selected's code
        }
        output.text += $"\nAdded {amount} {ItemPanel.itemList[id].name} to player\n\n";
    }

    private void SetStat(string stat, string valueString)
    {
        if (!int.TryParse(valueString, out int value))
        {
            output.text += "\nValue given is not a number\n\n";
            return;
        }
        switch (stat)
        {
            case "damage":
                WeaponStats.Instance.FlatDamage = value;
                output.text += $"\nAdded {value} amount of damage to the player\n\n";                
            break;
            case "maxhealth":
                PlayerStats.Instance.FlatBonusHealth = value;
                output.text += $"\nAdded {value} amount of max health to the player\n\n";
            break;
            case "crit":
                WeaponStats.Instance.FlatCritChance = value;
                output.text += $"\nAdded {value} amount of crit chance to the player\n\n";
            break;
            case "firedelay":
                WeaponStats.Instance.FlatFireDelay = value; 
                output.text += $"\nAdded {value} amount of fire speed to the player\n\n";               
            break;
            case "movespeed":
                TopDownMovement.Instance.FlatBonusSpeed = value; 
                output.text += $"\nAdded {value} amount of move speed to the player\n\n";               
            break;
            case "critdamage":
                WeaponStats.Instance.FlatCritDamage = value; 
                output.text += $"\nAdded {value} amount of fire speed to the player\n\n";               
            break;
            default:
                output.text += "\nStat given is not a valid enterable stat\n\n";
            break;
        }

    }
    private void Spawn(string enemy, string count)
    {
        if (!int.TryParse(enemy, out int enemyId))
        {
            output.text += "\nEnemyId given is incorrect\n\n";
            return;
        }
        if (!int.TryParse(count, out int amount))
        {
            output.text += "\nCount given is not a number\n\n";
            return;
        }
        
    }
    private void SetWave(string wave)
    {
        if (!int.TryParse(wave, out int waveNumber))
        {
            output.text = "\nWave number given is not a number\n\n";
            return;
        }
        GameSettings.waveNumber = waveNumber - 1;
        Timer.Instance.waveNumber = waveNumber - 1;
        output.text += $"\nNext wave set to {GameSettings.waveNumber + 1}";
    }

    private void GodMode(string boolean)
    {
        if (!bool.TryParse(boolean, out bool isGod))
        {
            output.text += "\nBool given is not a boolean\n\n";
            return;
        }
        if (isGod)
        {
            StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(99999999f));
            output.text += "\nGodmode is on\n\n";
        }
        else
        {
            StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(0f));
            output.text += "\nGodmode is off\n\n";
        }

    }

    private void Cull()
    {
        Timer.CullBullets();
        Timer.CullEnemies();
        output.text += "\nAll enemies on screen culled\n\n";
    }

    private void StopSpawn(string choice)
    {
        switch (choice)
        {
            case "enemy":
                stopEnemy = !stopEnemy;
                output.text += "\nEnemies will stop spawning\n\n";
            break;
            case "boss":
                stopBoss = !stopBoss;
                output.text += "\nBosses will stop spawning\n\n";
            break;
            default:
                output.text += "\nChoice not a valid value\n\n";
            break;
        }
    }
    
    private void SkipWave()
    {
        Timer.Instance.currentTime = 0.1f;
        output.text += "\nWave Skipped!\n\n";
    }
}
