using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

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
    private bool isGod;

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
                case "god":
                    GodMode();
                break;
                case "cull":
                    Cull();
                break;
                case "togglespawn":
                    StopSpawn(commands[1]);
                break;
                case "skipwave":
                    SkipWave();
                break;
                case "spawn":
                    Spawn(commands[1], commands[2]);
                break;
                case "setskill":
                    SetSkill(commands[1]);
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
            "god                       \t| gives the player god mode.\n\n" +
            "spawn {enemyId} {count}   \t| spawns an enemy with the id given and the amount given\n\n" +
            "cull                      \t| will cull all current enemies and bullets on screen\n\n" +
            "togglespawn {value}       \t| will toggle enemies or bosses from spawning.\n\n" +
            "setskill {skill}          \t| will set the players active skill to the skill given\n\n";
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
                    output.text += "\nsetstat {stat:string} {value:int}\n\nWill give the player the increase, specified in value, to the flat bonus modifier of the given stat. Will fail if the stat does not match.\n\nAccepted values:\ndamage, \ncrit, \nmaxhealth, \nfiredelay, \nmovespeed, \ncritdamage\n\n";
                break;
                case "setwave":
                    output.text += "\nsetstat {waveNo.:int}\n\nWill set the next wave to the number given.\n\n";
                break;
                case "god":
                    output.text += "\ngod\n\nWill toggle between turning on and off the players ability to be damaged, will not heal the player.\n\n";
                break;
                case "cull":
                    output.text += "\ncull\n\nCulls all active enemies and bullets on stage.\n\n";
                break;
                case "togglespawn":
                    output.text += "\ntogglespawn {value:string}\n\nWill toggle spawning either for enemies or bosses, depending on value given, from spawning further. Does not cull current enemies, use \"cull\" to do that.\n\nAccepted values: \nenemy, \nboss \n\n";
                break;
                case "skipwave":
                    output.text += "\nskipwave\n\nSkips the current wave by setting the timer to 0.1.\n\n";
                break;
                case "spawn":
                    output.text += "\nspawn {enemyID:int} {count:int}\n\nSpawns the amount of enemies specified in count, and the type of enemy in enemyID. Accepted values: 1 - " + EnemySpawner.Instance.allEnemies.Count +"\n\n";
                break;
                case "setskill":
                    output.text += "\nsetskill {skill:string}\n\nSets the players active skill to the skill given. Will fail if the skill given does not match a skill in the list.\n\nAccepted values:\ndash, \nvanish, \ndecoy\n\n";
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

    private void GiveItem(string itemId, string itemAmount = "1")
    {
        if (!int.TryParse(itemId, out int id) || id < 0)
        {
            output.text += "\nItemId given is not a valid number\n\n";
            return;
        }
        if (!int.TryParse(itemAmount, out int amount) || amount < 1)
        {
            output.text += "\nAmount given is not a valid number\n\n";
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            effectTable.ItemPicked(id); //activate the item selected's code
        }
        try
        {
            output.text += $"\nAdded {amount} {ItemPanel.itemList[id].name} to player\n\n";
        }
        catch (System.Exception)
        {
            output.text += $"\nNot a valid ItemID\n\n";
        }
       
    }

    private void SetStat(string stat, string valueString)
    {
        if (!int.TryParse(valueString, out int value))
        {
            output.text += "\nValue given is not a valid number\n\n";
            return;
        }
        switch (stat)
        {
            case "damage":
                WeaponStats.Instance.FlatDamage = value;
            break;
            case "maxhealth":
                PlayerStats.Instance.FlatBonusHealth = value;
            break;
            case "crit":
                WeaponStats.Instance.FlatCritChance = value;
            break;
            case "firedelay":
                WeaponStats.Instance.FlatFireDelay = value; 
            break;
            case "movespeed":
                TopDownMovement.Instance.FlatBonusSpeed = value; 
            break;
            case "critdamage":
                WeaponStats.Instance.FlatCritDamage = value; 
            break;
            default:
                output.text += "\nStat given is not a valid enterable stat\n\n";
            break;
        }
        output.text += $"\nSet {stat} to {value}\n\n";             

    }
    private void Spawn(string enemy, string count = "1")
    {
        if (!int.TryParse(enemy, out int enemyId) || enemyId < 0 || enemyId > EnemySpawner.Instance.allEnemies.Count)
        {
            output.text += "\nEnemyId given is not a valid number\n\n";
            return;
        }
        if (!int.TryParse(count, out int amount) || amount < 1)
        {
            output.text += "\nCount given is not a valid number\n\n";
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            Vector3 location = new Vector3();
            int side = Random.Range(1,5); //Which side of the screen the enemy spawns at
            switch (side)
            {
                case 1:  
                    location = Camera.main.ViewportToWorldPoint(new Vector3(1.1f,Random.Range(0f,1f),0));
                break;
                case 2:
                    location = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f,Random.Range(0f,1f),0));
                break;
                case 3:
                    location = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f,1f),1.1f,0));
                break;
                case 4:
                    location = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f,1f),-0.1f,0));
                break;
            }

            GameObject enemyInstance = Instantiate(EnemySpawner.Instance.allEnemies[enemyId - 1].enemy, location, Quaternion.identity);
            EnemySpawner.Instance.currentEnemies.Add(enemyInstance);
        }
        output.text += $"\nSpawned {amount} of level {enemyId} enemies\n\n";
        
    }
    private void SetWave(string wave)
    {
        if (!int.TryParse(wave, out int waveNumber) || waveNumber < 1)
        {
            output.text = "\nWave number given is not a valid number\n\n";
            return;
        }
        GameSettings.waveNumber = waveNumber - 1;
        Timer.Instance.waveNumber = waveNumber - 1;
        output.text += $"\nNext wave set to {GameSettings.waveNumber + 1}\n\n";
    }

    private void GodMode()
    {
        isGod = !isGod;
        Physics2D.IgnoreLayerCollision(7, 9, isGod);
        output.text += $"\nGodmode is {isGod}\n\n";
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
            {
                stopEnemy = !stopEnemy;
                string stopCondition = stopEnemy ? "stopped" : "resumed";
                output.text += $"\nEnemy spawning is now {stopCondition}\n\n";
            }
            break;
            case "boss":
            {
                stopBoss = !stopBoss;
                string stopCondition = stopEnemy ? "stopped" : "resumed";
                output.text += $"\nBoss spawning is now {stopCondition}\n\n";
            }
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

    private void SetSkill(string skill)
    {
        switch (skill)
        {
            case "dash":
                GameSettings.activeSkill = SkillEnum.dash;
            break;
            case "vanish":
                GameSettings.activeSkill = SkillEnum.vanish;
            break;
            case "decoy":
                GameSettings.activeSkill = SkillEnum.decoy;
            break;
            default:
                output.text += "\nSkill given is not a skill\n\n";
            break;
        }
        output.text += $"Skill set to {GameSettings.activeSkill}\n\n";
    }
}
