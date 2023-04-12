using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDict", menuName = "Infection Zone Divers/ItemDict", order = 0)]
public class ItemDatabase : ScriptableObject{ 
    public List<Item> itemDict = new List<Item>();
}
