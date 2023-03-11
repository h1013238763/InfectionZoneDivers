using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Invent", menuName = "Infection Zone Divers/Invent", order = 3)]
public class Inventory : ScriptableObject
{
    public List<Item> invent = new List<Item>();
    [SerializeField]private int inventCapacity;

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
    public void AddToInvent(){
  
    }

    public void RemovFromInvent(){

    }

    public void UseItem(){

    }
}
