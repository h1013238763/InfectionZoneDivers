using System.Collections;
using System.Collections.Generic;

public class ShortItem{
    public int itemID;
    public int itemNum;

    public ShortItem(int itemID, int itemNum){
        this.itemID = itemID;
        this.itemNum = itemNum;
    }

    public ShortItem(Item item){
        this.itemID = item.itemID;
        this.itemNum = 0;
    }

    public override string ToString(){
        return "ID: " + itemID + ", num: " + itemNum;
    }
}

