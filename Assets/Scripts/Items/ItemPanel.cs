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
    public StyleColor rarityColor;
    public StyleColor buttonColor;
}

public enum rarity{
    Common,
    Uncommon,
    Rare,
    Epic,
    Weapon,
    Legendary,
    Cursed
}
    
public class ItemPanel : MonoBehaviour
{   
    [HideInInspector]
    public bool itemChosen;  
    private int index;
    private rarity roll;    
    public ItemEffectTable itemController;
    [SerializeField]
    private VisualElement panel;
    private Button item1;
    private Button item2;
    private Button item3;
    private Button skip;
    private IMGUIContainer container;
    [SerializeField]

    private StyleColor buttonColor  = new StyleColor(new Color32(90, 90, 90, 255));
    private List<Item> selectedItems = new List<Item>();

    //make sure not to dupelicate the item ids
    public static List<Item> itemList = new List<Item>{ //char limit of 99 in description 
        new() { id = 0, name = "Sharpened Talons", desc = "Increases damage you deal", rarity = rarity.Common, stacks = 0, rarityColor = new StyleColor(new Color32(135, 150, 146, 255)) },
        new() { id = 01, name = "Oats", desc = "Gives you more max health", rarity = rarity.Common, stacks = 0, rarityColor = new StyleColor(new Color32(135, 150, 146, 255)) },
        new() { id = 02, name = "Boots", desc = "Increases your waddle speed", rarity = rarity.Common, stacks = 0, rarityColor = new StyleColor(new Color32(135, 150, 146, 255)) },
        new() { id = 03, name = "Band-Aid", desc = "Your health slowly regenerates over time", rarity = rarity.Rare, stacks = 0, rarityColor = new StyleColor(new Color32(50, 173, 196, 255)) },
        new() { id = 04, name = "Illegal Trigger", desc = "You shoot faster", rarity = rarity.Common, stacks = 0, rarityColor = new StyleColor(new Color32(135, 150, 146, 255)) },
        new() { id = 05, name = "Chompers", desc = "Your hits bleed enemies", rarity = rarity.Uncommon, stacks = 0, rarityColor = new StyleColor(new Color32(79, 122, 52, 255)) },
        new() { id = 06, name = "Leech", desc = "Your hits on enemies heal you", rarity = rarity.Rare, stacks = 0, rarityColor = new StyleColor(new Color32(50, 173, 196, 255)) },
        new() { id = 07, name = "Explosive Bullets", desc = "Your bullets explode on impact", rarity = rarity.Rare, stacks = 0, rarityColor = new StyleColor(new Color32(50, 173, 196, 255)) },
        new() { id = 08, name = "Egg", desc = "You gain an extra life", rarity = rarity.Epic, stacks = 0, rarityColor = new StyleColor(new Color32(127, 6, 145, 255)), buttonColor = new StyleColor(new Color32(93, 6, 145, 255)) },
        new() { id = 09, name = "Lucky Feather", desc = "You have an increased chance to deal critical damage" , rarity = rarity.Uncommon, stacks = 0, rarityColor = new StyleColor(new Color32(79, 122, 52, 255)) },
        new() { id = 10, name = "Glass Cannon", desc = "Halves your health to double your damage", rarity = rarity.Epic, stacks = 0, rarityColor = new StyleColor(new Color32(127, 6, 145, 255)), buttonColor = new StyleColor(new Color32(93, 6, 145, 255)) },
        new() { id = 11, name = "Shotgun", desc = "You shoot a spread of bullets instead of one", rarity = rarity.Epic, stacks = 0, rarityColor = new StyleColor(new Color32(127, 6, 145, 255)), buttonColor = new StyleColor(new Color32(93, 6, 145, 255)) },
        new() { id = 12, name = "Lucky Dive", desc = "Gain two random basic stats at half strength", rarity = rarity.Uncommon, stacks = 0, rarityColor = new StyleColor(new Color32(79, 122, 52, 255)) },
        new() { id = 13, name = "Weapon", desc = "Temp", rarity = rarity.Weapon, stacks = 0, rarityColor = new StyleColor(new Color32(90, 90, 90, 255)), buttonColor = new StyleColor(new Color32(22, 22, 22, 255)) },
        new() { id = 14, name = "Weapon", desc = "Temp", rarity = rarity.Weapon, stacks = 0, rarityColor = new StyleColor(new Color32(90, 90, 90, 255)), buttonColor = new StyleColor(new Color32(22, 22, 22, 255)) },
        new() { id = 15, name = "Weapon", desc = "Temp", rarity = rarity.Weapon, stacks = 0, rarityColor = new StyleColor(new Color32(90, 90, 90, 255)), buttonColor = new StyleColor(new Color32(22, 22, 22, 255)) },
        new() { id = 16, name = "Weapon Upgrade", desc = "Temp", rarity = rarity.Legendary, stacks = 0, rarityColor = new StyleColor(new Color32(179, 109, 28, 255)), buttonColor = new StyleColor(new Color32(134, 60, 10, 255)) },
        new() { id = 17, name = "Weapon Upgrade", desc = "Temp", rarity = rarity.Legendary, stacks = 0, rarityColor = new StyleColor(new Color32(179, 109, 28, 255)), buttonColor = new StyleColor(new Color32(134, 60, 10, 255)) },
        new() { id = 18, name = "Weapon Upgrade", desc = "Temp", rarity = rarity.Legendary, stacks = 0, rarityColor = new StyleColor(new Color32(179, 109, 28, 255)), buttonColor = new StyleColor(new Color32(134, 60, 10, 255)) },
        new() { id = 19, name = "Curse", desc = "Temp", rarity = rarity.Cursed, stacks = 0, rarityColor = new StyleColor(new Color32(108, 21, 13, 255)), buttonColor = new StyleColor(new Color32(77, 7, 1, 255)) },
        new() { id = 20, name = "Curse", desc = "Temp", rarity = rarity.Cursed, stacks = 0, rarityColor = new StyleColor(new Color32(108, 21, 13, 255)), buttonColor = new StyleColor(new Color32(77, 7, 1, 255)) },
        new() { id = 21, name = "Curse", desc = "Temp", rarity = rarity.Cursed, stacks = 0, rarityColor = new StyleColor(new Color32(108, 21, 13, 255)), buttonColor = new StyleColor(new Color32(77, 7, 1, 255)) },
    };
    //in this list, there cannot be less than 3 of each rarity for the case that 3 of one rarity is picked on the item selection. 
    void Awake()
    {
        panel = GetComponent<UIDocument>().rootVisualElement;
        
        container = panel.Q<IMGUIContainer>("ItemPanelContainer");

        item1 = panel.Q<Button>("Item1");
        item1.RegisterCallback<ClickEvent>(RegisterItem1Click);
        item2 = panel.Q<Button>("Item2");
        item2.RegisterCallback<ClickEvent>(RegisterItem2Click);
        item3 = panel.Q<Button>("Item3");
        item3.RegisterCallback<ClickEvent>(RegisterItem3Click);
        skip = panel.Q<Button>("Skip");
        skip.RegisterCallback<ClickEvent>(RegisterSkipClick);
    }

    public rarity GetWeightedRarity() 
    {
        // not consts currently incase we want these values to change over time with the waves
        float commonRoll = 0.5f; //50%
        float uncommonRoll = 0.8f; //30%
        float rareRoll = 1f; //20% 

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
        return roll;
    }
    public rarity GetBoundRarity(int waveNumber) 
    {
        if (waveNumber == 5)
        {
            roll = rarity.Weapon;
        }
        else if (waveNumber % 25 == 5)
        {
            roll = rarity.Legendary;
        }
        else if (waveNumber % 25 == 10)
        {
            roll = rarity.Epic;
        }
        else if (waveNumber % 25 == 15)
        {
            roll = rarity.Legendary;
        }
        else if (waveNumber % 25 == 20)
        {
            roll = rarity.Cursed;
        }
        return roll;
    }

    public void InitializeItemPanel(int waveNumber) //this is called every time the inventory ui pops up
    { 
        if (waveNumber % 5 == 0 && waveNumber % 25 != 0)
        {
            GetBoundItems(3, waveNumber);
        }
        else
        {
            GetUnboundItems(3);
        }
    }
    private void GetBoundItems(int repetitions, int waveNumber)
    {
        List<Item> generatedRarityList = new List<Item>();

        rarity boundItemRarity = GetBoundRarity(waveNumber);
        // Create a list of all the available items of that rarity.
        foreach (Item j in itemList)
        {
            if (j.rarity == boundItemRarity)
            {
                generatedRarityList.Add(j);
            }
        }
        container.style.backgroundColor = generatedRarityList[0].buttonColor;

        for (int i = 0; i < repetitions; i++) 
        {
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

            Label itemRarity = panel.Q<Label>($"ItemRarity{i+1}");
            itemRarity.text = boundItemRarity.ToString();

            itemRarity.style.color = selectedItems[i].rarityColor;
            currentButton.style.backgroundColor = selectedItems[i].buttonColor;

            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {selectedItems[i].name}");
        }
        generatedRarityList.Clear();
    }
    private void GetUnboundItems(int repetitions)
    {
        container.style.backgroundColor = new StyleColor(new Color32(72,72,72,255));
        List<Item> generatedRarityList = new List<Item>();

        for (int i = 0; i < repetitions; i++) 
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
            Label itemRarity = panel.Q<Label>($"ItemRarity{i+1}");
            itemRarity.style.color = new StyleColor(new Color32(255,255,255,255));



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

            rarity unbounditemRarity = selectedItems[i].rarity;
            itemRarity.text = unbounditemRarity.ToString();
            currentButton.style.backgroundColor = selectedItems[i].rarityColor;

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
    private void RegisterSkipClick(ClickEvent click)
    {
        itemController.ItemPicked(-1);
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

}
