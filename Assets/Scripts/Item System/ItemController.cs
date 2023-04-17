using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{   
    public ItemDatabase database;
    public static ItemController controller;

    public List<GameObject> itemWorldPool;
    public GameObject dropItem;
    public List<ShortItem> itemStat;

    public int metallic;
    public int plastic;
    public int electronic;
    public int chemical;

    // Start is called before the first frame update
    void Start()
    {
        controller = this;
        DropItemPool();
        DropItemSet(4, 300, new Vector2(0,0));
        DropItemSet(1, 1, new Vector2(3, 0));
    }

    public void DropItemPool(){
        for(int i = 0; i < 5; i ++){
            GameObject temp = Instantiate(dropItem, gameObject.transform);
            itemWorldPool.Add(temp);
            temp.SetActive(false);
        }
    }

    public GameObject DropItemGet(){
        for(int i = 0; i < itemWorldPool.Count; i ++){
            if(!itemWorldPool[i].activeSelf)
                return itemWorldPool[i];
        }
        return null;
    }

    public void DropItemSet(int id, int num, Vector2 pos){
        GameObject temp = DropItemGet();
        if(temp == null){
            temp = Instantiate(dropItem);
            itemWorldPool.Add(temp);
        }
        temp.GetComponent<DropItem>().Initial(id, num, database.itemDict[id].itemSprite);
        temp.transform.position = new Vector3(pos.x, pos.y, 0);
        temp.SetActive(true);
    }

    public int ItemGet(int id, int num, Invent invent, string tag){
        List<ShortItem> tempList = invent.inventList;
        // get item capacity for quick use
        int itemCapacity = database.itemDict[id].itemCap;
        // loop through inventory
        for(int i = 0; i < tempList.Count; i ++){
            // if find empty slot
            if(tempList[i] == null){
                tempList[i] = new ShortItem(id, num);
                GUIController.controller.SetGUI(invent, tag);
                return 0;
            }
            // if there exist this item
            else if(tempList[i].itemID == id ){
                // if this slot full
                if(itemCapacity == tempList[i].itemNum){continue;}
                // if not full
                if((itemCapacity-tempList[i].itemNum) < num ){
                    tempList[i].itemNum = itemCapacity;
                    num -= (itemCapacity-tempList[i].itemNum);
                }
                else{
                    tempList[i].itemNum -= num;
                    GUIController.controller.SetGUI(invent, tag);
                    return 0;
                }
            }   
        }
        GUIController.controller.SetGUI(invent, tag);
        return num;
    }
    public int ItemUse(int id, int num, Invent invent, string tag){
        int tempNum = num;
        List<ShortItem> tempList = invent.inventList;
        // loop through invent
        for(int i = tempList.Count-1; i > -1; i--){
            // if find item
            if(tempList[i] == null){
                continue;
            }
            else if(tempList[i].itemID == id){
                // if require num left < item num
                if(tempList[i].itemNum > tempNum){
                    tempList[i].itemNum -= tempNum;
                    GUIController.controller.SetGUI(invent, tag);
                    return num;
                }
                else{
                    tempNum -= tempList[i].itemNum;
                    tempList[i] = null;
                }
            }
        }
        GUIController.controller.SetGUI(invent, tag);
        return num - tempNum;
    }
    public void ItemTransfer(Transform inventPanel, ShortItem item){
        // if transfer request from player inventory
        Invent from;
        Invent to;
        if(inventPanel.name == "PlayerInventory"){
            from = GameObject.Find("Player").GetComponent<Invent>();
            // if two inventory panel op
            if(GUIController.controller.publicInvent.activeSelf){
                to = GUIController.controller.currentPublicInvent;
            }
            else{
                // if this item is a weapon
                if( database.itemDict[item.itemID] is Weapon ){
                    
                }
                // if this item is a consumable
                else if( database.itemDict[item.itemID].consumable ){
                    
                }
                return;
            }
        }
        else{
            from = GUIController.controller.currentPublicInvent;
            to = GameObject.Find("Player").GetComponent<Invent>();
        }

        int itemIn = ItemUse(item.itemID, item.itemNum, from, from.gameObject.tag);
        ItemGet(item.itemID, itemIn, to, to.gameObject.tag);
    }
    public void ItemEquip(ShortItem item, bool isWeapon){
        PlayerAction player = GameObject.Find("Player").GetComponent<PlayerAction>();
        // default equip weapon
        if(isWeapon){
           
        }
        // default equip item
        else{
            
        }
    }

    public Weapon WeaponFind(ShortItem item){
        return (Weapon)database.itemDict[item.itemID];
    }

    public Item ItemFind(ShortItem item){
        return database.itemDict[item.itemID];
    }

    public int ItemNumber(int id, Invent invent){
        int temp = 0;
        List<ShortItem> tempList = invent.inventList;
        for(int i = 0; i < tempList.Count; i ++){
            if(tempList[i] == null){
                continue;
            }
            if(tempList[i].itemID == id){
                temp += tempList[i].itemNum;
            }
        }
        return temp;
    }
}
