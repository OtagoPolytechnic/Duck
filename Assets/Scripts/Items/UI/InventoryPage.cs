using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    public int index;
    public int randomItemName;
    public int randomItemDesc;

    List<InventoryItem> listOfItems = new List<InventoryItem>();
    

    List<int> itemList = new List<int>{1, 2, 3, 4}; //replace with actual items

    public void InitializeInventoryItems() 
    {
        
    }

    public void InitializeInventoryUI(int inventorySize) //this needs to be called every time the item ui pops up
    { 
        for (int i = 0; i < inventorySize; i++)
        {
            index = Random.Range(0, itemList.Count); 
            randomItemName = itemList[index];
            randomItemDesc = itemList[index];
            Debug.Log($"In InventoryPage.cs: index chosen is {index} and value is {randomItemName}");
            itemList.RemoveAt(index);
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            item.GetComponent<InventoryItem>().textName = randomItemName;
            item.GetComponent<InventoryItem>().textDesc = randomItemDesc;
            item.transform.SetParent(contentPanel);
            item.transform.localScale = new Vector3(1, 1, 1); //this is to fix the parent scale issue. See https://github.com/BIT-Studio-4/Duck-Game/issues/65 for context
            listOfItems.Add(item);
        }
    }
                
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
