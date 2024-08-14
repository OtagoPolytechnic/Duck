using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class Item
{
    public int id;
    public string name;
    public string desc;
    public rarity rarity;
    public int stacks;
}

public enum rarity{
    Common,
    Uncommon,
    Rare,
    Epic
}
    
public class InventoryPage : MonoBehaviour
{   
    [HideInInspector]
    public bool itemChosen;  
    private int index;
    private rarity roll;    
    public ItemController itemController;
    [SerializeField]
    private VisualElement panel;
    private Button item1;
    private Button item2;
    private Button item3;
    private IMGUIContainer container;
    [SerializeField]
    private StyleColor commonColor = new StyleColor(new Color32(135, 150, 146, 255));
    private StyleColor uncommonColor  = new StyleColor(new Color32(79, 122, 52, 255));
    private StyleColor rareColor  = new StyleColor(new Color32(50, 173, 196, 255));
    private StyleColor epicColor  = new StyleColor(new Color32(127, 6, 145, 255));
    private List<Item> selectedItems = new List<Item>();

    public static Instance;

    //make sure not to dupelicate the item ids
    public static List<Item> itemList = new List<Item>{ //char limit of 99 in description 
        new() { id = 0, name = "Sharpened Talons", desc = "Increases damage you deal", rarity = rarity.Common, stacks = 0 },
        new() { id = 01, name = "Oats", desc = "Gives you more max health", rarity = rarity.Common, stacks = 0 },
        new() { id = 02, name = "Boots", desc = "Increases your waddle speed", rarity = rarity.Common, stacks = 0 },
        new() { id = 03, name = "Band-Aid", desc = "Your health slowly regenerates over time", rarity = rarity.Rare, stacks = 0 },
        new() { id = 04, name = "Illegal Trigger", desc = "You shoot faster", rarity = rarity.Common, stacks = 0 },
        new() { id = 05, name = "Chompers", desc = "Your hits bleed enemies", rarity = rarity.Uncommon, stacks = 0 },
        new() { id = 06, name = "Leech", desc = "Your hits on enemies heal you", rarity = rarity.Rare, stacks = 0 },
        new() { id = 07, name = "Explosive Bullets", desc = "Your bullets explode on impact", rarity = rarity.Rare, stacks = 0 },
        new() { id = 08, name = "Egg", desc = "You gain an extra life", rarity = rarity.Epic, stacks = 0 },
        new() { id = 09, name = "Lucky Feather", desc = "You have an increased chance to deal critical damage" , rarity = rarity.Uncommon, stacks = 0 },
        new() { id = 10, name = "Glass Cannon", desc = "Halves your health to double your damage", rarity = rarity.Epic, stacks = 0 },
        new() { id = 11, name = "Shotgun", desc = "You shoot a spread of bullets instead of one", rarity = rarity.Epic, stacks = 0 },
        new() { id = 12, name = "Lucky Dive", desc = "Gain two random basic stats at half strength", rarity = rarity.Uncommon, stacks = 0}
    };
    //in this list, there cannot be less than 3 of each rarity for the case that 3 of one rarity is picked on the item selection. 

    public rarity GetWeightedRarity() {

        // not consts currently incase we want these values to change over time with the waves
        float commonRoll = 0.5f; //50%
        float uncommonRoll = 0.8f; //30%
        float rareRoll = 0.95f; //15%
        float epicRoll = 1f;  //5%  

        // generate a random value (0->1)
        float randomRarity = Random.value;

        // Figure out which threshold this falls under.
        if (randomRarity < commonRoll)
        {
            roll = rarity.Common;
        }
        else if (randomRarity < uncommonRoll)
        {
            roll = rarity.Uncommon;
        }
        else if (randomRarity < rareRoll)
        {
            roll = rarity.Rare;
        }
        else if (randomRarity < epicRoll)
        {
            roll = rarity.Epic;
        }
        return roll;
    }
  
    void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        panel = GetComponent<UIDocument>().rootVisualElement;
        
        container = panel.Q<IMGUIContainer>("ItemPanelContainer");

        item1 = panel.Q<Button>("Item1");
        item1.RegisterCallback<ClickEvent>(RegisterItem1Click);
        item2 = panel.Q<Button>("Item2");
        item2.RegisterCallback<ClickEvent>(RegisterItem2Click);
        item3 = panel.Q<Button>("Item3");
        item3.RegisterCallback<ClickEvent>(RegisterItem3Click);
    }

    

    public void InitializeInventoryUI(int inventorySize) //this is called every time the inventory ui pops up
    { 

        List<Item> generatedRarityList = new List<Item>();

        for (int i = 0; i < inventorySize; i++) 
        {
            
            // Get a random rarity.
            rarity randomItemRarity = GetWeightedRarity();
            // Create a list of all the available items of that rarity.
            foreach (Item j in itemList)
            {
                if (j.rarity == randomItemRarity)
                {
                    generatedRarityList.Add(j);
                }
            }
            //if the item has already been selected, remove it from the possible pool of items
            foreach (Item k in selectedItems)
            {
                if (generatedRarityList.Contains(k))
                {
                    generatedRarityList.Remove(k);
                }
            }
            // then pick a random index from that subset and use that as the item.
            index = Random.Range(0, generatedRarityList.Count); 
            selectedItems.Add(generatedRarityList[index]);

            Label itemName = panel.Q<Label>($"ItemName{i+1}");
            itemName.text = selectedItems[i].name;

            Label itemDesc = panel.Q<Label>($"ItemDesc{i+1}");
            itemDesc.text = selectedItems[i].desc;

            Label itemStacks = panel.Q<Label>($"ItemStacks{i+1}");
            itemStacks.text = $"You have {selectedItems[i].stacks}";

            Button currentButton = panel.Q<Button>($"Item{i+1}");
            rarity itemRarity = selectedItems[i].rarity;
            if (itemRarity == rarity.Uncommon) //sets background color
            {
                currentButton.style.backgroundColor = uncommonColor;
            }
            else if (itemRarity == rarity.Rare)
            {
                currentButton.style.backgroundColor = rareColor;
            }
            else if (itemRarity == rarity.Epic)
            {
                currentButton.style.backgroundColor = epicColor;
            }
            else //assume all other items are common
            {
                currentButton.style.backgroundColor = commonColor;
            }

            generatedRarityList.Clear();
            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {selectedItems[i].name}");
        }
    }

    private void RegisterItem1Click(ClickEvent click)
    {
        itemController.ItemPicked(selectedItems[0].id); //activate the item selected's code
        itemChosen = true; 
        selectedItems.Clear();
    }
    private void RegisterItem2Click(ClickEvent click)
    {
        itemController.ItemPicked(selectedItems[1].id); //activate the item selected's code
        itemChosen = true; 
        selectedItems.Clear();
    }
    private void RegisterItem3Click(ClickEvent click)
    {
        itemController.ItemPicked(selectedItems[2].id); //activate the item selected's code
        itemChosen = true; 
        selectedItems.Clear();

    }
    
                
    public void Show()
    {
        container.visible = true;
    }
    public void Hide()
    {
        container.visible = false;
    }

    //Return the name, rarity, and stacks of the items that the player has currently.
    public List<Item> GetItems()
    {
        List<Item> playerItems = new List<Item>();
        foreach (Item i in selectedItems)
        {
            //Add a new item with the name rarity and stacks of the selected item
            playerItems.Add(new Item { name = i.name, rarity = i.rarity, stacks = i.stacks });
        }
        //Sort by rarity
        playerItems.Sort((x, y) => x.rarity.CompareTo(y.rarity));
        return playerItems;
    }

}
