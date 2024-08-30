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
            string[] commands = input.value.Split(" ");
            switch (commands[0])
            {
                case "help" :
                    Help();
                break;
                case "setweapon":
                    SetWeapon(commands[1]);
                break;
                case "giveitem":
                    GiveItem(commands[1], commands[2]);
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
                default:
                    output.text += "Not a valid command\n\n";
                break;
            }   
            input.value = "";
            input.Focus();
        }
    }

    public void Help()
    {
        output.text += "\nAvailable Commands: \n\n"+
        "setweapon {weaponName}    \t| will set player weapon to weapon given. Accepted values: pistol, dualpistol, shotgun, sniper, machinegun, rocket, sword\n\n" +
        "giveitem {itemID} {amount}\t| will give an item or multiple from the item list to the player (will not add to list of items the player has)\n\n" +
        "setstat {stat} {value}    \t| sets the stat specified to the value given. Accepted stats: damage, crit, maxhealth, firedelay, movespeed, critdamage\n\n" +
        "setwave {waveNo.}         \t| sets the next wave to the specified number\n\n" +
        "skipwave                  \t| will set the timer to 0, skipping the wave you are on\n\n" +
        "godmode {bool}            \t| gives the player god mode. true/false\n\n" +
        "spawn {enemyId} {count}   \t| spawns an enemy with the id given and the amount given\n\n" +
        "cull                      \t| will cull all current eneimes and bullets on screen\n\n" +
        "stopspawn {value}         \t| will toggle eneimes or bosses from spawning. Accepted values: enemy, boss\n\n";
        ;
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
                WeaponStats.Instance.CurrentWeapon = WeaponType.Machine;
            break;
            case "rocket":
                WeaponStats.Instance.CurrentWeapon = WeaponType.Pistol;
            break;
            case "sword":
                WeaponStats.Instance.CurrentWeapon = WeaponType.Pistol;
            break;
            default:
                output.text += "Invalid weapon name\n\n";
            break;
        }
        output.text += $"Weapon set to {WeaponStats.Instance.CurrentWeapon}\n\n";
    }

    private void GiveItem(string itemId, string itemAmount)
    {
        if (!int.TryParse(itemId, out int id))
        {
            output.text += "ItemId given is incorrect\n\n";
            return;
        }
        if (!int.TryParse(itemAmount, out int amount))
        {
            output.text += "Amount given is not a number\n\n";
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            effectTable.ItemPicked(id); //activate the item selected's code
        }
        output.text += $"Added {amount} {ItemPanel.itemList[id].name} to player\n\n";
    }

    private void SetWave(string wave)
    {
        if (!int.TryParse(wave, out int waveNumber))
        {
            output.text = "Wave number given is not a number\n\n";
            return;
        }
        GameSettings.waveNumber = waveNumber - 1;
        Timer.Instance.waveNumber = waveNumber - 1;
        output.text += $"Next wave set to {GameSettings.waveNumber + 1}";
    }

    private void GodMode(string boolean)
    {
        if (!bool.TryParse(boolean, out bool isGod))
        {
            output.text += "Bool given is not a boolean\n\n";
            return;
        }
        if (isGod)
        {
            StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(99999999f));
            output.text += "Godmode is on\n\n";
        }
        else
        {
            StartCoroutine(PlayerStats.Instance.DisableCollisionForDuration(0f));
            output.text += "Godmode is off\n\n";
        }

    }

    private void Cull()
    {
        Timer.CullBullets();
        Timer.CullEnemies();
        output.text += "All eneimes on screen culled\n\n";
    }

    private void StopSpawn(string choice)
    {
        switch (choice)
        {
            case "enemy":
                stopEnemy = !stopEnemy;
                output.text += "Enemies will stop spawning\n\n";
            break;
            case "boss":
                stopBoss = !stopBoss;
                output.text += "Bosses will stop spawning\n\n";
            break;
            default:
                output.text += "Choice not a valid value\n\n";
            break;
        }
    }
    
    private void SkipWave()
    {
        Timer.Instance.currentTime = 0.1f;
        output.text += "Wave Skipped!\n\n";
    }
}
