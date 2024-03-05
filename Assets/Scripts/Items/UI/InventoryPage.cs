using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] 
    private InventoryItem itemPrefab;
    [SerializeField]
    private RectTransform contentPanel;
    
    List<InventoryItem> listOfItems = new List<InventoryItem>();
    List<int> listOfIDs = new List<int>();

    public void InitializeInventoryUI(int inventorySize)
    {
        //generateItem(inventorySize = 3);
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
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

//    public void generateItem(int inventorySize)
//    {
//        bool reserved;
//
//        while (listOfIDs.Count > inventorySize)
//        {
//        	reserved = false;
//        	int number = Random.Range(1, 4); //change last number with amount of total items + 1
//        	foreach (int card in listOfIDs)
//        	{
//        		if (number == card)
//        		{
//        			reserved = true;
//        		}
//        	}
//        	if (!reserved)
//        	{
//        		listOfIDs.Add(number);
//        	}
//        }
//    }
}
