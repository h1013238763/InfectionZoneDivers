using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Quick : MonoBehaviour
{
    public int index;
    public int itemID;

    public void Reset(Sprite sprite, int i, int d, int num){
        transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
        index = i;
        itemID = d;
        transform.GetChild(0).gameObject.SetActive(true);
        if(num > 1){
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.GetComponent<Text>().text = num.ToString();
        }
    }

    public void Hide(){
        transform.GetChild(0).gameObject.SetActive(false);
        if(index > 1)
            transform.GetChild(1).gameObject.SetActive(false);
    }

    public void MouseClick(){
        ItemController.controller.ItemUnequip(index);
    }

    // While mouse over, show item details
    public void MouseEnter(){
        GUIController.controller.ShowItemDetail(itemID);
    }

    // while mouse exit, stop showing item details
    public void MouseExit(){
        GUIController.controller.HideItemDetail();
    }
}
