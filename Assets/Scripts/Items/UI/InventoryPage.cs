using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
[System.Serializable]
public class Item
{
    public string name;
    public string desc;
    public string rarity;
}
public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    private int index;
    [HideInInspector]
    public string randomItemName;
    [HideInInspector]
    public string randomItemDesc;
    public List<Item> itemList = new List<Item>();

    List<InventoryItem> preGenItems = new List<InventoryItem>();

    public void InitializeInventoryUI(int inventorySize) //this is called every time the item ui pops up
    { 
        List<Item> tempItems = new List<Item>(itemList);
        for (int i = preGenItems.Count - 1; i >= 0; i--) //makes sure the items previously generated are cleared to not bunch up on the inventory window
        {
            Destroy(preGenItems[i].gameObject); //removes item
            preGenItems.RemoveAt(i); //removes floating null pointer
        } 

        for (int i = 0; i < inventorySize; i++) //generates an item without duplication an assigns it to the prefab.
        {
            index = Random.Range(0, tempItems.Count); 
            randomItemName = tempItems[index].name;
            randomItemDesc = tempItems[index].desc;
            Debug.Log($"In InventoryPage.cs: index chosen is {index} and item is {randomItemName}");
            tempItems.RemoveAt(index);
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            preGenItems.Add(item);
            item.GetComponent<InventoryItem>().textName = randomItemName;
            item.GetComponent<InventoryItem>().textDesc = randomItemDesc;
            item.transform.SetParent(contentPanel);
            item.transform.localScale = new Vector3(1, 1, 1); //this is to fix the parent scale issue. See https://github.com/BIT-Studio-4/Duck-Game/issues/65 for context
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
