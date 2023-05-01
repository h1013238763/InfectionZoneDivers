using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public static BuildController controller;
    public BuildingDatabase database;

    public List<GameObject> buildInWorld;

    public List<GameObject> buildStructure;
    public List<GameObject> buildCraft;
    public List<GameObject> buildDefence;
    public List<GameObject> buildSurvivor;
    public List<GameObject> buildOther;

    public List<GameObject> currBuildList;

    public GameObject currentBuilding;

    public int rotate = 0;

    public bool overlap = false;
    public bool enough = true;

    void Start(){
        controller = this;
    }

    public void AssignList(int id){
        switch(id){
            case 0:
                currBuildList = buildStructure;
                break;
            case 1:
                currBuildList = buildCraft;
                break;
            case 2:
                currBuildList = buildDefence;
                break;
            case 3:
                currBuildList = buildSurvivor;
                break;
            case 4:
                currBuildList = buildOther;
                break;
            default:
                break;
        }
        GUIController.controller.SetBlueprintPanel();
    }

    public void AssignCurrentBuilding(int id){
        currentBuilding = currBuildList[id];
        GUIController.controller.SetBuildTipSprite(currentBuilding);
    }

    public void PlaceBuilding(){
        if(currentBuilding == null)
            return;
        
        if(overlap){
            Debug.Log("overlap");
            return;
        }
        if(WorldController.controller.ResourceCheck(currentBuilding.GetComponent<Building>().buildRequire)){
            GameObject temp = GUIController.controller.buildTip;
            Instantiate(currentBuilding, temp.transform.position, temp.transform.rotation, transform);
        }
        
    }
}
