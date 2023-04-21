using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Bench : MonoBehaviour
{
    Item item;
    int num;

    public void Reset(Item item, int num,Sprite sprite){
        this.item = item;
        this.num = num;
        transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
        transform.GetChild(1).gameObject.GetComponent<Text>().text = item.itemName;
        transform.GetChild(2).gameObject.GetComponent<Text>().text = "x " + num.ToString();
        for(int i = 0; i < 4; i ++){
            transform.GetChild(4).GetChild(i).gameObject.GetComponent<Text>().text = item.itemPrice[i].ToString();
        }
    }

    // hide child objects
    public void Hide(){
        gameObject.SetActive(false);
        item = null;
    }

    // While mouse over, show item details
    public void MouseEnter(){
        if(item != null)
            GUIController.controller.ShowItemDetail(item.itemID);
    }

    // while mouse exit, stop showing item details
    public void MouseExit(){
        GUIController.controller.HideItemDetail();
    }

    public void MouseClick(){
        if(ItemController.controller.ItemGetAble(item.itemID, num, PlayerAction.player.gameObject.GetComponent<Invent>()) && ItemController.controller.ResourceCheck(item.itemPrice)){
            ItemController.controller.ItemGet(item.itemID, num, PlayerAction.player.gameObject.GetComponent<Invent>(), "Player");
            ItemController.controller.ResourceUse(item.itemPrice);
        }
    }
}
