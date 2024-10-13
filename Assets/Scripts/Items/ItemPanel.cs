using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Linq;
using System;


public enum rarity{
    Common,
    Uncommon,
    Rare,
    Epic,
    Weapon,
    Legendary,
    Cursed,
    Unimplemented //For items that are either temp or not working
}
    
public class ItemPanel : MonoBehaviour
{   
    [HideInInspector]
    public bool itemChosen;
    public ItemEffectTable itemController;
    [SerializeField]
    private VisualElement document;
    private VisualElement container;
    private VisualElement selectionContainer;
    private VisualElement confirmPanel;
    private VisualElement continuePanel;

    private const float COMMON = 0.5f; //50%
    private const float UNCOMMON = 0.8f; //30%


    [SerializeField]
    private List<Item> selectedItems = new List<Item>();
    private List<Button> itemButtons = new List<Button>();
    private Button skip;
    private Button reroll;
    private Button confirmButton;
    private Button cancelButton;
    private Button continueButton;
    private Label selectedItemLabel;
    public static List<Item> itemList = new List<Item>();

    public List<Item> heldItems = new List<Item>();
    public static ItemPanel Instance;

    private int rerollCharges;
    private int selectedIndex;
    private System.Random rand;
    private Color green = new Color(0.0f, 0.6f, 0.0f);
    private Color red = new Color(0.6f, 0.0f, 0.0f);
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        document = GetComponent<UIDocument>().rootVisualElement;
    }
    void Start()
    {
        container = document.Q<VisualElement>("Background");
        selectionContainer = container.Q<VisualElement>("Selection");
        VisualElement rerollAndSkip = container.Q<VisualElement>("RerollAndSkip");
        skip = rerollAndSkip.Q<Button>("Skip");
        reroll = rerollAndSkip.Q<Button>("Reroll");

        confirmPanel = container.Q<VisualElement>("PopupBackground");
        VisualElement innerConfirmPanel = confirmPanel.Q<VisualElement>("Popup");
        confirmButton = innerConfirmPanel.Q<Button>("Confirm");
        cancelButton = innerConfirmPanel.Q<Button>("Cancel");
        selectedItemLabel = innerConfirmPanel.Q<Label>("SelectedItem");

        continuePanel = container.Q<VisualElement>("ContinueBox");
        continueButton = continuePanel.Q<Button>("Continue");

        confirmButton.RegisterCallback<ClickEvent>(ConfirmSelection);
        cancelButton.RegisterCallback<ClickEvent>(CancelSelection);
        continueButton.RegisterCallback<ClickEvent>(Continue);

        confirmPanel.style.display = DisplayStyle.None;
        continuePanel.style.display = DisplayStyle.None;

        rerollCharges = GameSettings.MaxRerollCharges;
        rand = new System.Random();
        LoadItems();
        
    }

    private void LoadItems()
    {
        string json = Resources.Load<TextAsset>("items").text;
        ItemList itemListJson = JsonUtility.FromJson<ItemList>(json);
        itemList = itemListJson.items;
    }

    public List<rarity> GetRarities(int waveNumber)
    {
        List<rarity> rarities = new List<rarity>();

        if (waveNumber == 1)//Level 1 is always a weapon
        {
            rarities.Add(rarity.Weapon);
        }
        else if (waveNumber == 5)
        {
            rarities.Add(rarity.Epic);
        }
        else if (waveNumber == 10)
        {
            rarities.Add(rarity.Legendary);
        }
        else if (waveNumber == 15)
        {
            rarities.Add(rarity.Cursed);
        }
        else if (waveNumber % 5 == 0)
        {
            rarities.Add(rarity.Cursed);
            rarities.Add(rarity.Legendary);
            rarities.Add(rarity.Epic);
        }
        else //Wave isn't a special wave
        {
            float randomRarity = UnityEngine.Random.value;
            if (randomRarity < COMMON)
            {
                rarities.Add(rarity.Common);
            }
            else if (randomRarity < UNCOMMON)
            {
                rarities.Add(rarity.Uncommon);
            }
            else
            {
                rarities.Add(rarity.Rare);
            }
        }
        return rarities;
    }

    public void InitializeItemPanel(int waveNumber) //this is called every time the inventory ui pops up
    {
        setStats();
        setWeapon();
        setItems();
        GetItems(3, waveNumber);
        activateButtons();
    }

    private void activateButtons()
    {
        itemButtons[0].RegisterCallback<ClickEvent>(RegisterItem1Click);
        itemButtons[1].RegisterCallback<ClickEvent>(RegisterItem2Click);
        itemButtons[2].RegisterCallback<ClickEvent>(RegisterItem3Click);
        skip.RegisterCallback<ClickEvent>(RegisterSkipClick);
        reroll.RegisterCallback<ClickEvent>(RegisterRerollClick);
    }

    public void setStats()
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

        VisualElement rerollCount = reroll.Q<VisualElement>("RerollCount");
        rerollCount.Q<Label>("RerollCountText").text = $"{rerollCharges}";
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
        foreach (Item i in heldItems)
        {
            if ((int)i.rarity > 2 && i.rarity != rarity.Weapon)
            {
                items.Add($"{i.name} x{i.stacks}");
            }
        }
        itemList.itemsSource = items;
        itemList.Rebuild();
    }

    private void GetItems(int repetitions, int waveNumber)
    {
        for (int i = 0; i < repetitions; i++)
        {
            List<Item> generatedRarityList = new List<Item>();
            //Get a random rarity.
            List<rarity> rarities = GetRarities(waveNumber);
            VisualElement currentItem = selectionContainer.Q<VisualElement>($"Item{i+1}");
            //Create a list of all the available items of that rarity.
            foreach (Item j in itemList)
            {
                if (rarities.Contains(j.rarity)
                && (j.weapons.Contains(WeaponStats.Instance.CurrentWeapon.ToString()) || j.weapons.Count == 0) //If it has the right weapons or no weapons at all
                && !selectedItems.Contains(j)) //If it hasn't already been selected
                {
                    generatedRarityList.Add(j);
                }
            }
            int index = rand.Next(0, generatedRarityList.Count);
            selectedItems.Add(generatedRarityList[index]);
            Label name = currentItem.Q<Label>("Name");
            Label rarity = currentItem.Q<Label>("Rarity");
            Label desc = currentItem.Q<Label>("Description");
            Button button = currentItem.Q<Button>("Button");

            currentItem.style.backgroundColor = selectedItems[i].rarityColor;
            name.text = selectedItems[i].name;
            rarity.text = selectedItems[i].rarity.ToString();
            desc.text = selectedItems[i].desc;
            itemButtons.Add(button);
        }
    }

    private void addItemToList(Item item)
    {
        if (heldItems.Contains(item) == false)
        {
            heldItems.Add(item);
        }
        item.stacks++;
    }

    private void RegisterItem1Click(ClickEvent click)
    {
        selectedIndex = 0;
        selectedItemLabel.text = selectedItems[0].name;
        confirmPanel.style.display = DisplayStyle.Flex;
    }
    private void RegisterItem2Click(ClickEvent click)
    {
        selectedIndex = 1;
        selectedItemLabel.text = selectedItems[1].name;
        confirmPanel.style.display = DisplayStyle.Flex;
    }
    private void RegisterItem3Click(ClickEvent click)
    {
        selectedIndex = 2;
        selectedItemLabel.text = selectedItems[2].name;
        confirmPanel.style.display = DisplayStyle.Flex;
    }
    private void RegisterSkipClick(ClickEvent click)
    {
        selectedIndex = -1;
        selectedItemLabel.text = "Skip";
        confirmPanel.style.display = DisplayStyle.Flex;
    }

    private void ConfirmSelection(ClickEvent click)
    {
        if (selectedIndex == -1)
        {
            PlayerStats.Instance.CurrentHealth += 5;
            itemController.ItemPicked(-1);
        }
        else
        {
            itemController.ItemPicked(selectedItems[selectedIndex].id); //activate the item selected's code
            addItemToList(selectedItems[selectedIndex]);
        }
        //itemChosen = true;
        selectedItems.Clear();
        confirmPanel.style.display = DisplayStyle.None;
        selectionContainer.style.display = DisplayStyle.None;
        continuePanel.style.display = DisplayStyle.Flex;
        updateStats();
        setStats();
        setItems();
        setWeapon();
    }

    private void Continue(ClickEvent click)
    {
        continuePanel.style.display = DisplayStyle.None;
        selectionContainer.style.display = DisplayStyle.Flex;
        resetColour();
        itemChosen = true;
    }

    private void updateStats()
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

    private void resetColour()
    {
        VisualElement statsPanel = document.Q<VisualElement>("Stats");
        //change all fields back to 1B1B1B
        Color defaultColour = new Color(0.11f, 0.11f, 0.11f);
        statsPanel.Q<Label>("Health").style.color = defaultColour;
        statsPanel.Q<Label>("Damage").style.color = defaultColour;
        statsPanel.Q<Label>("Range").style.color = defaultColour;
        statsPanel.Q<Label>("CritChance").style.color = defaultColour;
        statsPanel.Q<Label>("CritDamage").style.color = defaultColour;
        statsPanel.Q<Label>("MovementSpeed").style.color = defaultColour;
        statsPanel.Q<Label>("AttackSpeed").style.color = defaultColour;
        statsPanel.Q<Label>("Regeneration").style.color = defaultColour;
        statsPanel.Q<Label>("ExplosionSize").style.color = defaultColour;
        statsPanel.Q<Label>("ExplosionDamage").style.color = defaultColour;
        statsPanel.Q<Label>("BulletSpeed").style.color = defaultColour;
        statsPanel.Q<Label>("BleedDamage").style.color = defaultColour;
        statsPanel.Q<Label>("Pierce").style.color = defaultColour;
        statsPanel.Q<Label>("Lifesteal").style.color = defaultColour;
    }

    private void CancelSelection(ClickEvent click)
    {
        confirmPanel.style.display = DisplayStyle.None;
    }

    private void RegisterRerollClick(ClickEvent click)
    {
        if (rerollCharges > 0)
        {
            rerollCharges--;
            //Change the reroll count
            VisualElement rerollCount = reroll.Q<VisualElement>("RerollCount");
            rerollCount.Q<Label>("RerollCountText").text = $"{rerollCharges}";
            //Deregister the buttons
            itemButtons[0].UnregisterCallback<ClickEvent>(RegisterItem1Click);
            itemButtons[1].UnregisterCallback<ClickEvent>(RegisterItem2Click);
            itemButtons[2].UnregisterCallback<ClickEvent>(RegisterItem3Click);

            //Clear the selected items
            selectedItems.Clear();

            //Get new items
            InitializeItemPanel(GameSettings.waveNumber);
        }
    }
    
    public void Show()
    {
        container.visible = true;
    }
    public void Hide()
    {
        container.visible = false;
    }

}
