using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Infection Zone Divers/Item", order = 0)]
public class Item : ScriptableObject{
    public int itemID;
    public string itemName;
    [TextArea]
    public string itemDescribe;
    public Sprite itemSprite;
    public int itemCap;
    public int[] itemPrice = new int[4];
}


