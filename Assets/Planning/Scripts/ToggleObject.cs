using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour {

    private bool active;
    public Button buttonComponent;
    public Text nameLabel;
    public Image iconImage;
    public Text priceText;
    public GameObject placingHolder;


    private Item item;
    private CartItem cartItem;
    private ShopScrollList scrollList;
    private CartList cartList;
    private StopPlacing stopPlacing ;

    // Use this for initialization
    void Start() {
        if(buttonComponent != null)
        buttonComponent.onClick.AddListener(HandleClick);
        
    }

    // Update is called once per frame
    void Update() {
    }
    public void Toggle()
    {
        active = !active;

        gameObject.SetActive(true);
    }

    public void Setup(Item currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        nameLabel.text = item.itemName;
        iconImage.sprite = item.icon;
        priceText.text = item.price.ToString();
        scrollList = currentScrollList;

    }

    public void SetupCart(CartItem currentItem, CartList currentCartList)
    {
        cartItem = currentItem;
        nameLabel.text = cartItem.itemName;
        iconImage.sprite = cartItem.icon;
        priceText.text = cartItem.price.ToString();
        cartList = currentCartList;
    }




    public void HandleClick()
    {
        if (placingHolder == null)
            placingHolder = GameObject.Find("Camera");
            stopPlacing = placingHolder.GetComponent<StopPlacing>();
        stopPlacing.OnEnd("");
        //scrollList.TryTransferItemToOtherShop(item);
        stopPlacing.OnEnd(iconImage.sprite.name);
        int chk =Convert.ToInt32(buttonComponent.name.Substring(5));
        stopPlacing.SetCursor(chk,item);
    }

}
