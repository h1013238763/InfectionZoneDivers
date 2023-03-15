using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public BuildingDatabase buildDatabase;
    private GameObject currentBuilding;

    public void AssignToMouse(int id){
        currentBuilding = buildDatabase.buildingDict[id];
    }

    void FixedUpdate(){
        
    }
}
