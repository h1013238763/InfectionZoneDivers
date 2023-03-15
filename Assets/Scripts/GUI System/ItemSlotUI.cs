using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour{

    public ShortItem item;
    [SerializeField]private GameObject imageObj;
    [SerializeField]private GameObject textObj;

    public void Reset(ShortItem item, Sprite sprite){
        this.item = item;
        imageObj.GetComponent<Image>().sprite = sprite;
        imageObj.SetActive(true);
        textObj.GetComponent<Text>().text = item.itemNum.ToString();
        textObj.SetActive(true);
    }

    public void Hide(){
        imageObj.SetActive(false);
        textObj.SetActive(false);
    }

    public void Print(){
        if(item != null)
            Debug.Log(item.itemID + ":" + item.itemNum);
    }
}