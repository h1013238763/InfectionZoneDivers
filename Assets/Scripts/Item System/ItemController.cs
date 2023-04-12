using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{   
    [SerializeField]private ItemDatabase database;
    public static ItemController controller;

    public List<ShortItem> itemStat;

    public int money;
    public int metallic;
    public int plastic;
    public int electronic;
    public int chemical;

    // Start is called before the first frame update
    void Start()
    {
        controller = this;
    }

    public int ItemGet(int id, int num, List<ShortItem> invent, int inventCap){
        // get item capacity for quick use
        int itemCapacity = database.itemDict[id].itemCap;
        // loop through inventory
        for(int i = 0; i < invent.Count; i ++){
            // if there exist this item
            if(invent[i].itemID == id ){
                // if this slot full
                if(itemCapacity == invent[i].itemNum){continue;}
                // if not full
                if((itemCapacity-invent[i].itemNum) < num ){
                    invent[i].itemNum = itemCapacity;
                    num -= (itemCapacity-invent[i].itemNum);
                }
                else{
                    invent[i].itemNum -= num;
                    return 0;
                }
            }   
        }
        if(invent.Count < inventCap){
            invent.Add(new ShortItem(id, num));
            return 0;
        }
        return num;
    }

    public int ItemUse(int id, int num, List<ShortItem> invent){
        int temp = num;
        // loop through invent
        for(int i = 0; i < invent.Count; i++){
            // if find item
            if(invent[i].itemID == id){
                // if require num left < item num
                if(invent[i].itemNum > temp){
                    invent[i].itemNum -= temp;
                    return num;
                }
                else{
                    temp -= invent[i].itemNum;
                    invent.RemoveAt(i);
                }
            }
        }
        return num - temp;
    }

    public Weapon WeaponFind(ShortItem item){
        return (Weapon)database.itemDict[item.itemID];
    }

    public Item ItemFind(ShortItem item){
        return database.itemDict[item.itemID];
    }

    public int ItemNumber(int id, List<ShortItem> invent){
        int temp = 0;
        for(int i = 0; i < invent.Count; i ++){
            if(invent[i].itemID == id){
                temp += invent[i].itemNum;
            }
        }
        return temp;
    }
}
