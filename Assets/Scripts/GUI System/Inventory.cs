using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GUI inventory controller
/// add to inventory panel to control add, remove, use item. and set slots ot it.
/// </summary>
public class Inventory : MonoBehaviour
{
    public List<SlotItem> invent = new List<SlotItem>();
    [SerializeField]private int inventCapacity;
    [SerializeField]private DatabaseController itemDict;

    /*
    loop through list until item added
        if there exist this item
            if exist-item capacity left < add-item num
                add exist-item capacity to full
            if exist-item capacity left >= add-item num
                add add-item num to exist item
            add-item num -= add num
        else
            create new item
            add to new item
    */ 
    public void AddToInvent(SlotItem item){
        
    }

    public void RemovFromInvent(SlotItem item){

    }

    public void UseItem(){

    }

    public void SetSlotItem(){

    }
}
