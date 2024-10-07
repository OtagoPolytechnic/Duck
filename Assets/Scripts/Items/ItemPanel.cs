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
    private int index;
    public ItemEffectTable itemController;
    [SerializeField]
    private VisualElement document;
    private VisualElement container;
    private VisualElement selectionContainer;
    private VisualElement confirmPanel;

    private const float COMMON = 0.5f; //50%
    private const float UNCOMMON = 0.8f; //30%


    [SerializeField]
    private List<Item> selectedItems = new List<Item>();
    private List<Button> itemButtons = new List<Button>();
    private Button skip;
    private Button reroll;
    private Button confirmButton;
    private Button cancelButton;
    private Label selectedItemLabel;
    public static List<Item> itemList = new List<Item>();

    public List<Item> heldItems = new List<Item>();
    public static ItemPanel Instance;

    private int rerollCharges;
    private int selectedIndex;
    private System.Random rand;
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

        confirmButton.RegisterCallback<ClickEvent>(ConfirmSelection);
        cancelButton.RegisterCallback<ClickEvent>(CancelSelection);

        confirmPanel.style.display = DisplayStyle.None;

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
        VisualElement weaponPanel = document.Q<VisualElement>("Weapon");
        ListView itemList = document.Q<ListView>("ItemsListView");
        
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

        //Set the weapon
        weaponPanel.Q<Label>("WeaponName").text = WeaponStats.Instance.WeaponNameFormatted();

        //List of strings to display the items
        List<string> items = new List<string>();
        //Set the items
        foreach (Item i in heldItems)
        {
            //If the rarity is Epic, Legendary, or Cursed, add the name and quantity to the list
            if (i.rarity == rarity.Epic || i.rarity == rarity.Legendary || i.rarity == rarity.Cursed)
            {
                items.Add($"{i.name} x{i.stacks}");
            }
        }
        //If there are no items, add a label saying "None"
        if (items.Count == 0)
        {
            items.Add("No items");
        }
        //Set the list of items
        itemList.itemsSource = items;
        itemList.Rebuild();
        VisualElement rerollCount = reroll.Q<VisualElement>("RerollCount");
        rerollCount.Q<Label>("RerollCountText").text = $"{rerollCharges}";
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
        itemChosen = true;
        selectedItems.Clear();
        confirmPanel.style.display = DisplayStyle.None;
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
