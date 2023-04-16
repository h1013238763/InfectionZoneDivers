using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventSlotUI : MonoBehaviour{

    public ShortItem item;
    [SerializeField]private GameObject imageObj;
    [SerializeField]private GameObject textObj;

    // reset child objects to current item
    public void Reset(ShortItem item, Sprite sprite){
        this.item = item;
        imageObj.GetComponent<Image>().sprite = sprite;
        imageObj.SetActive(true);
        textObj.GetComponent<Text>().text = item.itemNum.ToString();
        textObj.SetActive(true);
    }

    // hide child objects
    public void Hide(){
        imageObj.SetActive(false);
        textObj.SetActive(false);
    }

    // While mouse over, show item details
    public void MouseEnter(){
        Debug.Log("Mouse Enter");
        if(item != null)
            GUIController.controller.ShowItemDetail(item);
    }

    // while mouse exit, stop showing item details
    public void MouseExit(){
        Debug.Log("Mouse Exit");
        GUIController.controller.HideItemDetail();
    }

    public void MouseRightClick(){
        
    }
}