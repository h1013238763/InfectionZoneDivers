using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Infection Zone Divers/Armor", order = 2)]
public class Consumable : Item{
    [TextArea]public string consumableDescribe;
    public string consumableType;
    public float[] consumableData = new float[2];
}
