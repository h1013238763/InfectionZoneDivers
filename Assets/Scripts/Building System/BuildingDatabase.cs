using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDict", menuName = "Infection Zone Divers/BuildingDict", order = 0)]
public class BuildingDatabase : ScriptableObject
{
    public List<GameObject> buildingDict = new List<GameObject>();
}
