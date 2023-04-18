using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public int index;

    public void Reset(Sprite sprite, int i, int num){
        transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
        index = i;
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
}