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

    /// <summary>
    /// a pool of in-world dropped items
    /// </summary>
    public void DropItemPool(){
        for(int i = 0; i < 5; i ++){
            GameObject temp = Instantiate(dropItem, gameObject.transform);
            itemWorldPool.Add(temp);
            temp.SetActive(false);
        }
    }

    /// <summary>
    /// get a item object from the item pool
    /// </summary>
    /// <returns> the unassigned drop item </returns>
    public GameObject DropItemGet(){
        for(int i = 0; i < itemWorldPool.Count; i ++){
            if(!itemWorldPool[i].activeSelf)
                return itemWorldPool[i];
        }
        return null;
    }

    /// <summary>
    /// assign the unassigned drop item
    /// </summary>
    /// <param name="id"> the id of item </param>
    /// <param name="num"> the number of item </param>
    /// <param name="pos"> the position of this item </param>
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

    /// <summary>
    /// Put item into inventory
    /// </summary>
    /// <param name="id"> the id of input item</param>
    /// <param name="num"> the number of input item</param>
    /// <param name="invent"> which invent receive this item</param>
    /// <param name="tag"> what game object call this function</param>
    /// <returns> how many items can't put in </returns>
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

    /// <summary>
    ///  get item out of inventory
    /// </summary>
    /// <param name="id"> the id of output item </param>
    /// <param name="num"> how many need to get</param>
    /// <param name="invent"> which invent provide items</param>
    /// <param name="tag"> which game object call this function</param>
    /// <returns> how many items are removed </returns>
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

    /// <summary>
    /// transfer item between inventorys
    /// </summary>
    /// <param name="inventPanel"> Which inventory panel call the function </param>
    /// <param name="item"> What need to be transfered </param>
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
                if( database.itemDict[item.itemID] is Weapon || database.itemDict[item.itemID].consumable){
                    ItemEquip(item, database.itemDict[item.itemID] is Weapon);
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

    /// <summary>
    /// equip weapons and consumables
    /// </summary>
    /// <param name="item"> what item need to be equiped </param>
    /// <param name="isWeapon"> is this item a weapon ? </param>
    public void ItemEquip(ShortItem item, bool isWeapon){
        PlayerAction player = GameObject.Find("Player").GetComponent<PlayerAction>();
        bool set = false;
        // equip weapon
        if(isWeapon){
            if(player.weaponSlot[player.currentWeapon] == null ){
                player.weaponSlot[player.currentWeapon] = (Weapon)database.itemDict[item.itemID];
                set = true;
            }
            else if(player.weaponSlot[(player.currentWeapon+1)%2] == null){
                player.weaponSlot[(player.currentWeapon+1)%2] = (Weapon)database.itemDict[item.itemID];
                set = true;
            }
        }
        // equip item
        else{
            for(int i = 0; i < 4; i ++){
                if(player.quickSlot[i] == null){
                    player.quickSlot[i] = new ShortItem(item.itemID, item.itemNum);
                    set = true;
                    break;
                }
            }
        }
        // remove item from inventory
        if(set)
            ItemUse(item.itemID, item.itemNum, GameObject.Find("Player").GetComponent<Invent>(), "Player");
    }

    /// <summary>
    /// unequip weapons and consumables
    /// </summary>
    /// <param name="slot"> which slot call this function </param>
    public void ItemUnequip(int slot){
        // weapon
        if(slot <= 1){
            Weapon weapon = PlayerAction.player.weaponSlot[(PlayerAction.player.currentWeapon+slot)%2];
            if(weapon == null) 
                return;
            if(ItemGet(weapon.itemID, 1, PlayerAction.player.gameObject.GetComponent<Invent>(), "Player") == 0)
                PlayerAction.player.weaponSlot[(PlayerAction.player.currentWeapon+slot)%2] = null;
        }
        // item
        else{
            ShortItem item = PlayerAction.player.quickSlot[slot-2];
            if(item == null)
                return;
            if(ItemGet(item.itemID, item.itemNum, PlayerAction.player.gameObject.GetComponent<Invent>(), "Player") == 0)
                PlayerAction.player.quickSlot[slot-2] = null;
        }
        GUIController.controller.SetGUI(GameObject.Find("Player").GetComponent<Invent>(), "Player");
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
