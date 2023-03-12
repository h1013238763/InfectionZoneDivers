using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour{
    public ShortItem item;

    void Start(){
        AssignItem(2, 19);
    }

    public void AssignItem(int itemID, int itemNum){
        item = new ShortItem(itemID, itemNum);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Survivor"){
            int inNum = collision.gameObject.GetComponent<Inventory>().AddToInvent(item.itemID, item.itemNum);
            if(inNum == 0)
                gameObject.SetActive(false);
            else
                item.itemNum = inNum;
        }
    }
}