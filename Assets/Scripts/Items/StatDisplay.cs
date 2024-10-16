using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


public class StatDisplay : MonoBehaviour
{
    private VisualElement document;
    private Color green = new Color(0.0f, 0.6f, 0.0f);
    private Color red = new Color(0.6f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
    }

    public void UpdateStats()
    {
        setStats();
        setWeapon();
        setItems();
    }

    public void StatsChanged()
    {
        statChanges();
        UpdateStats();
    }

    private void setStats()
    {
        VisualElement statsPanel = document.Q<VisualElement>("Stats");
        
        //Set each stat
        statsPanel.Q<Label>("Health").text = $"{PlayerStats.Instance.CurrentHealth}/{PlayerStats.Instance.MaxHealth}";
        statsPanel.Q<Label>("Damage").text = $"{WeaponStats.Instance.Damage}";
        statsPanel.Q<Label>("Range").text = $"{WeaponStats.Instance.Range}";
        statsPanel.Q<Label>("CritChance").text = $"{WeaponStats.Instance.CritChance}%";
        statsPanel.Q<Label>("CritDamage").text = $"{WeaponStats.Instance.CritDamage}%";
        statsPanel.Q<Label>("MovementSpeed").text = $"{TopDownMovement.Instance.MoveSpeed}";
        statsPanel.Q<Label>("AttackSpeed").text = $"{WeaponStats.Instance.AttackSpeed:0.00}";
        statsPanel.Q<Label>("Regeneration").text = $"{PlayerStats.Instance.RegenerationPercentage}%";
        statsPanel.Q<Label>("ExplosionSize").text = $"{WeaponStats.Instance.ExplosionSize}";
        statsPanel.Q<Label>("ExplosionDamage").text = $"{WeaponStats.Instance.ExplosionDamage}";
        statsPanel.Q<Label>("BulletSpeed").text = $"{WeaponStats.Instance.BulletSpeed}";
        statsPanel.Q<Label>("BleedDamage").text = $"{WeaponStats.Instance.BleedDamage}%";
        if (WeaponStats.Instance.PierceAmount == -1 || WeaponStats.Instance.CurrentWeapon == WeaponType.Sword)
        {
            statsPanel.Q<Label>("Pierce").text = "∞";
        }
        else
        {
            statsPanel.Q<Label>("Pierce").text = $"{WeaponStats.Instance.PierceAmount}";
        }
        statsPanel.Q<Label>("Lifesteal").text = $"{PlayerStats.Instance.LifestealPercentage}%";
    }
    
    private void setWeapon()
    {
        VisualElement weaponPanel = document.Q<VisualElement>("Weapon");
        weaponPanel.Q<Label>("WeaponName").text = WeaponStats.Instance.WeaponNameFormatted();
    }

    private void setItems()
    {
        ListView itemList = document.Q<ListView>("ItemsListView");
        List<string> items = new List<string>();
        foreach (Item i in ItemPanel.Instance.heldItems)
        {
            if ((int)i.rarity > 2 && i.rarity != rarity.Weapon)
            {
                items.Add($"{i.name} x{i.stacks}");
            }
        }
        itemList.itemsSource = items;
        itemList.Rebuild();
    }

    private void statChanges()
    {
        //Check each stat and if its changed from the current value make the text green if its increased and red if its decreased
        VisualElement statsPanel = document.Q<VisualElement>("Stats");

        int oldHealth = int.Parse(statsPanel.Q<Label>("Health").text.Split('/')[1]);
        if (PlayerStats.Instance.MaxHealth > oldHealth)
        {
            statsPanel.Q<Label>("Health").style.color = red;
        }
        else if (PlayerStats.Instance.MaxHealth < oldHealth)
        {
            statsPanel.Q<Label>("Health").style.color = green;
        }

        int oldDamage = int.Parse(statsPanel.Q<Label>("Damage").text);
        if (WeaponStats.Instance.Damage > oldDamage)
        {
            statsPanel.Q<Label>("Damage").style.color = green;
        }
        else if (WeaponStats.Instance.Damage < oldDamage)
        {
            statsPanel.Q<Label>("Damage").style.color = red;
        }

        int oldRange = int.Parse(statsPanel.Q<Label>("Range").text);
        if (WeaponStats.Instance.Range > oldRange)
        {
            statsPanel.Q<Label>("Range").style.color = green;
        }
        else if (WeaponStats.Instance.Range < oldRange)
        {
            statsPanel.Q<Label>("Range").style.color = red;
        }

        int oldCritChance = int.Parse(statsPanel.Q<Label>("CritChance").text.Split('%')[0]);
        if (WeaponStats.Instance.CritChance > oldCritChance)
        {
            statsPanel.Q<Label>("CritChance").style.color = green;
        }
        else if (WeaponStats.Instance.CritChance < oldCritChance)
        {
            statsPanel.Q<Label>("CritChance").style.color = red;
        }

        int oldCritDamage = int.Parse(statsPanel.Q<Label>("CritDamage").text.Split('%')[0]);
        if (WeaponStats.Instance.CritDamage > oldCritDamage)
        {
            statsPanel.Q<Label>("CritDamage").style.color = green;
        }
        else if (WeaponStats.Instance.CritDamage < oldCritDamage)
        {
            statsPanel.Q<Label>("CritDamage").style.color = red;
        }

        float oldMoveSpeed = float.Parse(statsPanel.Q<Label>("MovementSpeed").text);
        //Comparing floats needs to be rounded or its unreliable
        if ((float)Math.Round(TopDownMovement.Instance.MoveSpeed, 2) > oldMoveSpeed)
        {
            statsPanel.Q<Label>("MovementSpeed").style.color = green;
        }
        else if ((float)Math.Round(TopDownMovement.Instance.MoveSpeed, 2) < oldMoveSpeed)
        {
            statsPanel.Q<Label>("MovementSpeed").style.color = red;
        }

        float oldAttackSpeed = float.Parse(statsPanel.Q<Label>("AttackSpeed").text);
        if ((float)Math.Round(WeaponStats.Instance.AttackSpeed, 2) > oldAttackSpeed)
        {
            statsPanel.Q<Label>("AttackSpeed").style.color = green;
        }
        else if ((float)Math.Round(WeaponStats.Instance.AttackSpeed, 2) < oldAttackSpeed)
        {
            statsPanel.Q<Label>("AttackSpeed").style.color = red;
        }

        int oldRegen = int.Parse(statsPanel.Q<Label>("Regeneration").text.Split('%')[0]);
        if (PlayerStats.Instance.RegenerationPercentage > oldRegen)
        {
            statsPanel.Q<Label>("Regeneration").style.color = green;
        }
        else if (PlayerStats.Instance.RegenerationPercentage < oldRegen)
        {
            statsPanel.Q<Label>("Regeneration").style.color = red;
        }

        int oldExplosionSize = int.Parse(statsPanel.Q<Label>("ExplosionSize").text);
        if (WeaponStats.Instance.ExplosionSize > oldExplosionSize)
        {
            statsPanel.Q<Label>("ExplosionSize").style.color = green;
        }
        else if (WeaponStats.Instance.ExplosionSize < oldExplosionSize)
        {
            statsPanel.Q<Label>("ExplosionSize").style.color = red;
        }

        int oldExplosionDamage = int.Parse(statsPanel.Q<Label>("ExplosionDamage").text);
        if (WeaponStats.Instance.ExplosionDamage > oldExplosionDamage)
        {
            statsPanel.Q<Label>("ExplosionDamage").style.color = green;
        }
        else if (WeaponStats.Instance.ExplosionDamage < oldExplosionDamage)
        {
            statsPanel.Q<Label>("ExplosionDamage").style.color = red;
        }

        int oldBulletSpeed = int.Parse(statsPanel.Q<Label>("BulletSpeed").text);
        if (WeaponStats.Instance.BulletSpeed > oldBulletSpeed)
        {
            statsPanel.Q<Label>("BulletSpeed").style.color = green;
        }
        else if (WeaponStats.Instance.BulletSpeed < oldBulletSpeed)
        {
            statsPanel.Q<Label>("BulletSpeed").style.color = red;
        }

        int oldBleed = int.Parse(statsPanel.Q<Label>("BleedDamage").text.Split('%')[0]);
        if (WeaponStats.Instance.BleedDamage > oldBleed)
        {
            statsPanel.Q<Label>("BleedDamage").style.color = green;
        }
        else if (WeaponStats.Instance.BleedDamage < oldBleed)
        {
            statsPanel.Q<Label>("BleedDamage").style.color = red;
        }
        if (statsPanel.Q<Label>("Pierce").text != "∞")
        {
            int oldPierce = int.Parse(statsPanel.Q<Label>("Pierce").text);
            if (WeaponStats.Instance.PierceAmount == -1 || WeaponStats.Instance.CurrentWeapon == WeaponType.Sword || WeaponStats.Instance.PierceAmount > oldPierce)
            {
                statsPanel.Q<Label>("Pierce").style.color = green;
            }
            else if(WeaponStats.Instance.PierceAmount < oldPierce)
            {
                statsPanel.Q<Label>("Pierce").style.color = red;
            }
        }

        int oldLifesteal = int.Parse(statsPanel.Q<Label>("Lifesteal").text.Split('%')[0]);
        if (PlayerStats.Instance.LifestealPercentage > oldLifesteal)
        {
            statsPanel.Q<Label>("Lifesteal").style.color = green;
        }
        else if (PlayerStats.Instance.LifestealPercentage < oldLifesteal)
        {
            statsPanel.Q<Label>("Lifesteal").style.color = red;
        }
    }
}
