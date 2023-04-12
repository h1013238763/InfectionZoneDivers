using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public static BuildController controller;
    public BuildingDatabase buildDatabase;
    private GameObject currentBuilding;

    public bool overlap = false;
    public bool enough = true;

    void Start(){
        controller = this;
    }

    public void AssignCurrentBuilding(int id){
        currentBuilding = buildDatabase.buildingDict[id];
    }

    public void PlaceBuilding(){
        if(overlap){
            Debug.Log("overlap");
            return;
        }
        Debug.Log("Place");
        // GameObject temp = GUIController.controller.buildTip;
        // Instantiate(currentBuilding, temp.transform.position, temp.transform.rotation, transform);
    }
}
