using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GUI inventory controller
/// add to inventory panel to control add, remove, use item. and set slots ot it.
/// </summary>
public class Inventory : MonoBehaviour
{
    public List<ShortItem> invent = new List<ShortItem>();
    [SerializeField]private int inventCapacity;
    [SerializeField]private DatabaseController database;
    [SerializeField]private GameObject inventUI;

    public void OpenInventory(){
        inventUI.GetComponent<InventUI>().OpenInventory(inventCapacity);
        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
        inventUI.GetComponent<InventUI>().PrintInvent();
    }

    public int AddToInvent(int itemIDIn, int itemNumIn){
        int itemCapacity = database.itemDict[itemIDIn].itemCap;
        // if there exist this item
        for(int i = 0; i < invent.Count; i ++){
            if(invent[i].itemID == itemIDIn ){
                if(itemCapacity == invent[i].itemNum){continue;}
                if((itemCapacity-invent[i].itemNum) < itemNumIn ){
                    invent[i].itemNum = itemCapacity;
                    itemNumIn -= (itemCapacity-invent[i].itemNum);
                    if(inventUI.activeInHierarchy)
                        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
                }
                else{
                    invent[i].itemNum -= itemNumIn;
                    if(inventUI.activeInHierarchy)
                        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
                    return 0;
                }
            }   
        }
        if(invent.Count < inventCapacity){
            invent.Add(new ShortItem(itemIDIn, itemNumIn));
            if(inventUI.activeInHierarchy)
                inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
            return 0;
        }
        return itemNumIn;
    }

    public void RemovFromInvent(){

    }

    public void UseItem(){

    }

    public void Print(){
        foreach(ShortItem item in invent){
            Debug.Log(item.itemID.ToString() + " " + item.itemNum.ToString());
        }
    }
}
