using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;



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
    public ItemEffectTable itemController;
    [SerializeField]
    private VisualElement panel;
    private Button item1;
    private Button item2;
    private Button item3;
    private Button skip;
    private VisualElement container;
    private IMGUIContainer buttonContainer;

    private const float COMMON = 0.5f; //50%
    private const float UNCOMMON = 0.8f; //30%


    [SerializeField]
    private StyleColor buttonColor  = new StyleColor(new Color32(70, 70, 70, 255));
    private List<Item> selectedItems = new List<Item>();
    public static List<Item> itemList = new List<Item>();

    public List<Item> heldItems = new List<Item>();
    public static ItemPanel Instance;

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
        panel = GetComponent<UIDocument>().rootVisualElement;
        
        container = panel.Q<VisualElement>("Background");
        buttonContainer = panel.Q<IMGUIContainer>("ButtonContainer");

        item1 = panel.Q<Button>("Item1");
        item1.RegisterCallback<ClickEvent>(RegisterItem1Click);
        item2 = panel.Q<Button>("Item2");
        item2.RegisterCallback<ClickEvent>(RegisterItem2Click);
        item3 = panel.Q<Button>("Item3");
        item3.RegisterCallback<ClickEvent>(RegisterItem3Click);
        skip = panel.Q<Button>("Skip");
        skip.RegisterCallback<ClickEvent>(RegisterSkipClick);
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
            float randomRarity = Random.value;
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
        if (waveNumber % 5 == 0 || waveNumber == 1) //if the wave is a bound wave
        {
            GetBoundItems(3, waveNumber);
        }
        else
        {
            GetUnboundItems(3, waveNumber);
        }
    }

    private void GetBoundItems(int repetitions, int waveNumber)
    {
        List<Item> generatedRarityList = new List<Item>();

        List<rarity> rarities = GetRarities(waveNumber);
        // Create a list of all the available items of that rarity.
        foreach (Item j in itemList)
        {
            if (rarities.Contains(j.rarity)
            && (j.weapons.Contains(WeaponStats.Instance.CurrentWeapon.ToString()) || j.weapons.Count == 0)) //If it has the right weapons or no weapons at all
            {
                generatedRarityList.Add(j);
            }
        }
        buttonContainer.style.backgroundColor = generatedRarityList[0].rarityColor;

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
            itemRarity.text = selectedItems[i].rarity.ToString();

            itemRarity.style.color = selectedItems[i].rarityColor;
            currentButton.style.backgroundColor = buttonColor;

            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {selectedItems[i].name}");
        }
        generatedRarityList.Clear();
    }
    private void GetUnboundItems(int repetitions, int waveNumber)
    {
        buttonContainer.style.backgroundColor = new StyleColor(new Color32(166,166,166,255));
        List<Item> generatedRarityList = new List<Item>();

        for (int i = 0; i < repetitions; i++) 
        {
            
            // Get a random rarity.
            List<rarity> rarities = GetRarities(waveNumber);
            // Create a list of all the available items of that rarity.
            foreach (Item j in itemList)
            {
                if (j.rarity == rarities[0])
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

            itemRarity.text = selectedItems[i].rarity.ToString();
            currentButton.style.backgroundColor = selectedItems[i].rarityColor;

            generatedRarityList.Clear();
            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {selectedItems[i].name}");
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
        itemController.ItemPicked(selectedItems[0].id); //activate the item selected's code
        itemChosen = true; 
        addItemToList(selectedItems[0]);
        selectedItems.Clear();
    }
    private void RegisterItem2Click(ClickEvent click)
    {
        itemController.ItemPicked(selectedItems[1].id); //activate the item selected's code
        itemChosen = true; 
        addItemToList(selectedItems[1]);
        selectedItems.Clear();
    }
    private void RegisterItem3Click(ClickEvent click)
    {
        itemController.ItemPicked(selectedItems[2].id); //activate the item selected's code
        itemChosen = true; 
        addItemToList(selectedItems[2]);
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
