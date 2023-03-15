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
    [SerializeField]private ItemDatabase database;
    [SerializeField]private GameObject inventUI;

    public void OpenInventory(){
        inventUI.GetComponent<InventUI>().OpenInventory(inventCapacity);
        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
    }

    public int AddToInvent(int id, int num){
        int itemCapacity = database.itemDict[id].itemCap;
        // if there exist this item
        for(int i = 0; i < invent.Count; i ++){
            if(invent[i].itemID == id ){
                if(itemCapacity == invent[i].itemNum){continue;}
                if((itemCapacity-invent[i].itemNum) < num ){
                    invent[i].itemNum = itemCapacity;
                    num -= (itemCapacity-invent[i].itemNum);
                    if(inventUI.activeInHierarchy)
                        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
                }
                else{
                    invent[i].itemNum -= num;
                    if(inventUI.activeInHierarchy)
                        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
                    return 0;
                }
            }   
        }
        if(invent.Count < inventCapacity){
            invent.Add(new ShortItem(id, num));
            if(inventUI.activeInHierarchy)
                inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
            return 0;
        }
        return num;
    }

    public bool FindItem(int id){
        foreach(var item in invent){
            if(item.itemID == id)
                return true;
        }
        return false;
    }

    public int UseItem(int id, int num){
        int temp = num;
        // loop through invent
        for(int i = 0; i < invent.Count; i++){
            // if find item
            if(invent[i].itemID == id){
                // if require num left < item num
                if(invent[i].itemNum > temp){
                    invent[i].itemNum -= temp;
                    if(inventUI.activeInHierarchy)
                        inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
                    return num;
                }
                else{
                    temp -= invent[i].itemNum;
                    invent.RemoveAt(i);
                }
            }
        }
        if(inventUI.activeInHierarchy)
            inventUI.GetComponent<InventUI>().ResetInvent(invent, inventCapacity);
        return num - temp;
    }
}
