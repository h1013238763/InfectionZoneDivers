using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public int itemID;
    public int itemNum;

    public void Initial(int id, int num, Sprite sprite){
        itemID = id;
        itemNum = num;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            itemNum = ItemController.controller.ItemGet(itemID, itemNum, collision.GetComponent<Invent>(), "Player");
        }

        if(itemNum <= 0){
            gameObject.SetActive(false);
        }
    }
}
