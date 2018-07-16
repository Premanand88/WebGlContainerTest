using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class CartItem
{
    public string itemName;
    public Sprite icon;
    public string price = "$1";
    public GameObject topCamera;
    public int count;
}

public class ListCount
{
    int count;
    string name;
}

public class CartList : MonoBehaviour {

    public GameObject gameManager;
    private List<GameObject> addedInScene;
    private List<ListCount> listCount;

    public GameObject menuScrollList;
    public List<CartItem> itemCartList;
    private List<Item> availableItems;

    public bool wallExists;
    public CartItem inCart;
    public static int objCount = 0;


    public GameObject buttonObjectPool;
    public Transform contentPanel;


    // Use this for initialization
    void Start () {
        addedInScene = new List<GameObject>();
        itemCartList = new List<CartItem>();
        listCount = new List<ListCount>();
        availableItems = menuScrollList.GetComponent<ShopScrollList>().itemList;
    }

    // Update is called once per frame
    void Update()
    {

        Transform[] ts = gameManager.GetComponentsInChildren<Transform>();
        if (ts.Length > 0 && ts.Length != objCount)
        {
            objCount = ts.Length;
            foreach (Transform item in ts)
            {
                if (!addedInScene.Contains(item.gameObject) && item.gameObject.tag.Contains("Placed"))
                {
                    addedInScene.Add(item.gameObject);
                }
            }

            if (addedInScene.Count > 0)
            {
                bool wallsExist, Door, Window, Furniture;

                foreach (GameObject item in addedInScene)
                {
                    if (item != null && item.name.Contains("Wall"))
                    {
                        if (!wallExists)
                            foreach (Item x in availableItems)
                            {
                                if (x.itemName.Contains("Side Wall"))
                                {
                                    CartItem pushData = new CartItem();
                                    pushData.icon = x.icon;
                                    pushData.itemName = "Side Wall * " + 1;
                                    pushData.price = x.price;
                                    pushData.count = 1;
                                    if (!itemCartList.Contains(pushData))
                                    {
                                        itemCartList.Add(pushData);
                                        if(buttonObjectPool == null)
                                        {
                                            buttonObjectPool = GameObject.FindGameObjectWithTag("buttonPool");
                                        }
                                        GameObject newButton = buttonObjectPool.GetComponent<SimpleObjectPool>().GetObject();
                                        newButton.transform.SetParent(contentPanel);
                                        newButton.name = "WallBtnCart";
                                        wallExists = true;

                                        ToggleObject sampleButton = newButton.GetComponent<ToggleObject>();
                                        sampleButton.enabled = false;
                                        sampleButton.SetupCart(pushData, this);
                                    }
                                }
                            }
                        else
                        {
                            foreach (CartItem i in itemCartList)
                            {
                                int count = 0;
                                if (i.itemName.Contains("Wall"))
                                {
                                    count = count + 1;
                                    i.count = i.count + 1;
                                    i.itemName = "Side Wall * " + i.count;
                                    Debug.Log(i.itemName);
                                    GameObject wallbt = GameObject.Find("WallBtnCart");
                                    wallbt.GetComponentInChildren<Text>().text = i.itemName;
                                }
                            }
                        }
                    }

                    if (item.name.Contains("Door"))
                    {

                    }
                    if (item.name.Contains("Window"))
                    {

                    }
                    if (item.name.Contains("Furniture"))
                    {


                    }
                    if (item.name.Contains("Floor"))
                    {

                    }

                    
                }
            }
        }
    }
}
