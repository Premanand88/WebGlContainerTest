using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public string price = "$1";
    public GameObject topCamera;
    public int count;
}

public class ShopScrollList : MonoBehaviour
{

    public List<Item> itemList;
    public Transform contentPanel;
    public ShopScrollList otherShop;
    public Text myGoldDisplay;
    public SimpleObjectPool buttonObjectPool;
    public GameObject ListMain;
    public GameObject gameManager;

    private int wallCount, doorCount, furnCount, WindowCount, floorCount;

    public float gold = 10000f;
    private bool disableButtons = false;


    // Use this for initialization
    void Start()
    {
        if (gameObject.tag != "Cart")
        {
            disableButtons = true;
            AddButtons();
        }
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        myGoldDisplay.text = "Gold: " + gold.ToString();
        if (gameObject.tag == "Cart")
        {
            disableButtons = false;
            //RemoveButtons();        
            //AddButtonShop();
        }
        else
        {
            disableButtons = true;
        }
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons(string Filter = "")
    {
        wallCount = 0;
        doorCount = 0;
        WindowCount = 0;
        furnCount = 0;
        floorCount = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];
            if (item.icon.name.Contains("Wall") && (Filter.Contains("Wall") || Filter == string.Empty))
            {
                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "WaBtn" + wallCount.ToString();
                wallCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
            if (item.icon.name.Contains("Door") && (Filter.Contains("Door") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "DoBtn" + doorCount.ToString();
                doorCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
            if (item.icon.name.Contains("Window") && (Filter.Contains("Window") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "WiBtn" + WindowCount.ToString();
                WindowCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
            if (item.icon.name.Contains("Furniture") && (Filter.Contains("Furniture") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "FuBtn" + furnCount.ToString();
                furnCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);

            }
            if (item.icon.name.Contains("Floor") && (Filter.Contains("Floor") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "FlBtn" + floorCount.ToString();
                floorCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
        }

    }
    private void AddButtonShop(string Filter = "")
    {
        wallCount = 0;
        doorCount = 0;
        WindowCount = 0;
        furnCount = 0;
        floorCount = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];

            var objobjIncidentMapList = (from IM in itemList
                                         group IM by new { IM.itemName } into g
                                         where g.Count() > 1
                                         select new
                                         {
                                             itemName = g.Key.itemName,
                                             NameCount = g.Count()
                                         }).ToList();
            //var WallcountNew = objobjIncidentMapList.Where(s => s.itemName.Contains("Wall")).Select(x => x.NameCount).FirstOrDefault();
            if (GameObject.FindGameObjectsWithTag("WallPlaced").Count() >0)
            {
                var WallcountNew = GameObject.FindGameObjectsWithTag("WallPlaced").Count();// objobjIncidentMapList.Where(s => s.itemName.Contains("Wall")).Select(x => x.NameCount).FirstOrDefault();
                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "WaBtn";
                wallCount += 1;
                GameObject wallbtn = GameObject.Find("WaBtn");
                if (wallbtn == null)
                {
                    ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                    sampleButton.enabled = disableButtons;
                    item.itemName = string.Empty;
                    item.itemName = "Side Wall";
                    sampleButton.Setup(item, this);
                }
                //else
                //{
                //    wallbtn.GetComponentInChildren<Text>().
                //}
            }
            if (item.icon.name.Contains("Door") && (Filter.Contains("Door") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "DoBtn" + doorCount.ToString();
                doorCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
            if (item.icon.name.Contains("Window") && (Filter.Contains("Window") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "WiBtn" + WindowCount.ToString();
                WindowCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
            if (item.icon.name.Contains("Furniture") && (Filter.Contains("Furniture") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "FuBtn" + furnCount.ToString();
                furnCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);

            }
            if (item.icon.name.Contains("Floor") && (Filter.Contains("Floor") || Filter == string.Empty))
            {

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel);
                newButton.name = "FlBtn" + floorCount.ToString();
                floorCount += 1;

                ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                sampleButton.enabled = disableButtons;
                sampleButton.Setup(item, this);
            }
        }

    }

    public void FilterButtons(string Filter)
    {
        RemoveButtons();
        AddButtons(Filter);
    }

    public void TryTransferItemToOtherShop(Item item)
    {
        //Debug.Log("enough gold");
        if (otherShop!= null)
        {
            //gold += item.price;
            //otherShop.gold -= item.price;

            AddItem(item, otherShop);
            RemoveItem(item, this);

            RefreshDisplay();
            otherShop.RefreshDisplay();
            //Debug.Log("enough gold");

        }
        //Debug.Log("attempted");
    }

    void AddItem(Item itemToAdd, ShopScrollList shopList)
    {
        if (shopList.itemList.Count > 0)
        {
            bool isExist = false;
            foreach( Item i in shopList.itemList)
            {
                if(i.icon.name == itemToAdd.icon.name&&i.icon.name.Contains("Wall"))
                {
                    isExist = true;
                    i.count = i.count+1;
                    i.itemName = "Side Wall * " + i.count;
                    i.count += 1;
                }
            }
            if (!isExist)
            {
                itemToAdd.count = 1;
                shopList.itemList.Add(itemToAdd);
            }

        }
        else
        {
            itemToAdd.count = 1;
            shopList.itemList.Add(itemToAdd);
        }

    }

    private void RemoveItem(Item itemToRemove, ShopScrollList shopList)
    {
        if (gameObject.tag == "Cart")
        {
            for (int i = shopList.itemList.Count - 1; i >= 0; i--)
            {
                if (shopList.itemList[i] == itemToRemove)
                {
                    shopList.itemList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void RemoveLastItem()
    {       
        itemList.RemoveAt(itemList.Count - 1);
        RefreshDisplay();
    }
}