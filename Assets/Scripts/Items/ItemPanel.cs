using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.InputSystem;


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
    public InputActionAsset inputActions;
    [HideInInspector]
    public bool itemChosen;
    public ItemEffectTable itemController;
    [SerializeField]
    private VisualElement document;
    private VisualElement container;
    private VisualElement selectionContainer;
    private VisualElement confirmPanel;
    private VisualElement continuePanel;

    [SerializeField] private StatDisplay statDisplay;

    private const float COMMON = 0.5f; //50%
    private const float UNCOMMON = 0.8f; //30%


    [SerializeField]
    private List<Item> selectedItems = new List<Item>();
    private List<Button> itemButtons = new List<Button>();
    private VisualElement rerollOuter;
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
        rerollOuter = rerollAndSkip.Q<VisualElement>("RerollOuter");
        VisualElement skipOuter = rerollAndSkip.Q<VisualElement>("SkipOuter");
        skip = skipOuter.Q<Button>("Skip");
        reroll = rerollOuter.Q<Button>("Reroll");

        confirmPanel = container.Q<VisualElement>("PopupBackground");
        VisualElement innerConfirmPanel = confirmPanel.Q<VisualElement>("Popup");
        confirmButton = innerConfirmPanel.Q<Button>("Confirm");
        cancelButton = innerConfirmPanel.Q<Button>("Cancel");
        selectedItemLabel = innerConfirmPanel.Q<Label>("SelectedItem");

        continuePanel = container.Q<VisualElement>("ContinueBox");
        continueButton = continuePanel.Q<Button>("Continue");

        confirmButton.RegisterCallback<ClickEvent>(ConfirmSelection);
        confirmButton.RegisterCallback<KeyDownEvent>(ConfirmSelection);
        cancelButton.RegisterCallback<ClickEvent>(CancelSelection);
        cancelButton.RegisterCallback<KeyDownEvent>(CancelSelection);
        continueButton.RegisterCallback<ClickEvent>(Continue);
        continueButton.RegisterCallback<KeyDownEvent>(Continue);

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
        statDisplay.UpdateStats();
        GetItems(3, waveNumber);
        VisualElement rerollCount = rerollOuter.Q<VisualElement>("RerollCount");
        rerollCount.Q<Label>("RerollCountText").text = $"{rerollCharges}";
        activateButtons();
    }

    private void activateButtons()
    {
        itemButtons[0].RegisterCallback<ClickEvent>(RegisterItem1Click);
        itemButtons[0].RegisterCallback<KeyDownEvent>(RegisterItem1Click);
        itemButtons[1].RegisterCallback<ClickEvent>(RegisterItem2Click);
        itemButtons[1].RegisterCallback<KeyDownEvent>(RegisterItem2Click);
        itemButtons[2].RegisterCallback<ClickEvent>(RegisterItem3Click);
        itemButtons[2].RegisterCallback<KeyDownEvent>(RegisterItem3Click);
        skip.RegisterCallback<ClickEvent>(RegisterSkipClick);
        skip.RegisterCallback<KeyDownEvent>(RegisterSkipClick);
        reroll.RegisterCallback<ClickEvent>(RegisterRerollClick);
        reroll.RegisterCallback<KeyDownEvent>(RegisterRerollClick);
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
            name.text = selectedItems[i].name.ToUpper();
            rarity.text = selectedItems[i].rarity.ToString().ToUpper();
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

    //Removes the weapon from the list of items
    public List<Item> HighscoreItems()
    {
        //New list to store the items
        List<Item> highscoreItems = new List<Item>();
        //For each item in the held items list, add it to the highscore items list
        foreach (Item i in heldItems)
        {
            //If not the weapon
            if (i.rarity != rarity.Weapon)
            {
                highscoreItems.Add(i);
            }
        }
        //Sort the list by rarity
        highscoreItems.Sort((x, y) => x.rarity.CompareTo(y.rarity));
        return highscoreItems;
    }

    private void RegisterItem1Click(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            selectedIndex = 0;
            selectedItemLabel.text = selectedItems[0].name.ToUpper();
            confirmPanel.style.display = DisplayStyle.Flex;
        }
    }
    private void RegisterItem2Click(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            selectedIndex = 1;
            selectedItemLabel.text = selectedItems[1].name.ToUpper();
            confirmPanel.style.display = DisplayStyle.Flex;
        }
    }

    private void RegisterItem3Click(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {            
            selectedIndex = 2;
            selectedItemLabel.text = selectedItems[2].name.ToUpper();
            confirmPanel.style.display = DisplayStyle.Flex;
        }
    }
    private void RegisterSkipClick(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            selectedIndex = -1;
            selectedItemLabel.text = "SKIP";
            confirmPanel.style.display = DisplayStyle.Flex;
        }
    }

    private void ConfirmSelection(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            if (selectedIndex == -1)
            {
                PlayerStats.Instance.CurrentHealth += PlayerStats.Instance.MaxHealth / 2;
                itemController.ItemPicked(-1);
            }
            else
            {
                itemController.ItemPicked(selectedItems[selectedIndex].id); //activate the item selected's code
                addItemToList(selectedItems[selectedIndex]);
            }
            selectedItems.Clear();
            confirmPanel.style.display = DisplayStyle.None;
            selectionContainer.style.display = DisplayStyle.None;
            continuePanel.style.display = DisplayStyle.Flex;
            statDisplay.StatsChanged();
            VisualElement rerollCount = rerollOuter.Q<VisualElement>("RerollCount");
            rerollCount.Q<Label>("RerollCountText").text = $"{rerollCharges}";
        }
    }

    private void Continue(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            continuePanel.style.display = DisplayStyle.None;
            selectionContainer.style.display = DisplayStyle.Flex;
            resetColour();
            itemChosen = true;
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

    private void CancelSelection(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            confirmPanel.style.display = DisplayStyle.None;
        }
    }

    private void RegisterRerollClick(EventBase evt)
    {
        if (SubmitCheck.Submit(evt, inputActions))
        {
            if (rerollCharges > 0)
            {
                rerollCharges--;
                //Change the reroll count
                VisualElement rerollCount = rerollOuter.Q<VisualElement>("RerollCount");
                rerollCount.Q<Label>("RerollCountText").text = $"{rerollCharges}";
                //Deregister the buttons
                itemButtons[0].UnregisterCallback<ClickEvent>(RegisterItem1Click);
                itemButtons[0].UnregisterCallback<KeyDownEvent>(RegisterItem2Click);
                itemButtons[1].UnregisterCallback<ClickEvent>(RegisterItem2Click);
                itemButtons[1].UnregisterCallback<KeyDownEvent>(RegisterItem2Click);
                itemButtons[2].UnregisterCallback<ClickEvent>(RegisterItem3Click);
                itemButtons[2].UnregisterCallback<KeyDownEvent>(RegisterItem3Click);

                //Clear the selected items
                selectedItems.Clear();

                //Get new items
                InitializeItemPanel(GameSettings.waveNumber);
            }
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
