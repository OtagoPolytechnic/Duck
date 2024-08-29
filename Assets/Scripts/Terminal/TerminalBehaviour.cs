using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEditor;

public class TerminalBehaviour : MonoBehaviour
{

    private Label output;
    private TextField input;
    private VisualElement document;
    private VisualElement terminalWindow;

    
    void Awake()
    {
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
                default:
                    output.text += "ERROR: Not a command\n";
                break;
            }   
            input.value = "";
            input.Focus();
        }
    }

    public void Help()
    {
        output.text += "\nAvailable Commands: \n\nsetweapon {weaponName} | will set player weapon to weapon given. Accepted values: pistol, dualpistol, shotgun, sniper, machinegun, rocket, sword\n\n" +
        "giveitem {itemID} | will give an item from the item list to the player (will not add to list of items the player has)\n\n" +
        "setwave {waveNo.} | sets the wave to specified number\n\n" +
        "setstat {stat} {value} | sets the stat specified to the value given. Accepted stats: damage, crit, maxhealth, firedelay, movespeed, critdamage\n\n" +
        "spawn {enemyId} {count} | spawns an enemy with the id given and the amount given\n\n" +
        "god | sets and unsets the player from god mode\n\n";
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
        }
        output.text += $"Weapon set to {WeaponStats.Instance.CurrentWeapon}\n";
    }
}
