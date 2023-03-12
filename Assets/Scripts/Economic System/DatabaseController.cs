using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDict", menuName = "Infection Zone Divers/ItemDict", order = 0)]
public class DatabaseController : ScriptableObject{ 
    public List<Item> ItemDict = new List<Item>();

}
