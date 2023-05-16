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

    public bool deleteMode = false;
    public Sprite deleteIcon;
    public GameObject deleteBuild;

    public GameObject currentBuilding;

    public int rotate = 0;

    public List<GameObject> overlapList;

    public bool overlap = false;
    public bool inUI = false;

    void Start(){
        controller = this;

        currentBuilding = null;
        deleteMode = false;
    }

    public void AssignList(int id){
        deleteMode = false;
        GUIController.controller.buildTip.SetActive(false);
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

    public void AddOverlapList(GameObject build){
        if(!overlapList.Contains(build)){
            overlapList.Add(build);
        }
        GUIController.controller.SetBuildTipColor(overlapList.Count > 0);
        overlap = (overlapList.Count > 0);
    }

    public void RemoveOverlapList(GameObject build){
        if(overlapList.Contains(build)){
            overlapList.Remove(build);
        }
        GUIController.controller.SetBuildTipColor(overlapList.Count > 0);
        overlap = (overlapList.Count > 0);
    }

    public void AssignCurrentBuilding(int id){
        currentBuilding = currBuildList[id];
        GUIController.controller.SetBuildTipSprite(currentBuilding);
    }

    public void DeleteMode(bool to){
        currentBuilding = null;

        GameObject tip = GUIController.controller.buildTip;

        deleteMode = to;

        GUIController.controller.buildSlotGrid.SetActive(false);

        tip.SetActive(true);
        tip.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 0.7f);
        tip.GetComponent<SpriteRenderer>().sprite = deleteIcon;
        tip.GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f);
        tip.GetComponent<BoxCollider2D>().offset = new Vector2(0.5f, 0.5f);
    }

    public void CheckInUI(bool temp){
        inUI = temp;
    }

    public void PlaceBuilding(){
        if(deleteBuild != null && deleteBuild.GetComponent<Building>().buildType != "Core" ){
            deleteBuild.GetComponent<Building>().OnDestroy();
            buildInWorld.Remove(deleteBuild);
            deleteBuild = null;
        }

        if(currentBuilding == null || inUI)
            return;
        
        if(overlap){
        
            if(!GUIController.controller.textTip.activeSelf){
                GUIController.controller.SetTextTip("Position not available");     
            }
            return;
        }
        if(WorldController.controller.ResourceCheck(currentBuilding.GetComponent<Building>().buildRequire)){

            WorldController.controller.ResourceUse(currentBuilding.GetComponent<Building>().buildRequire);

            GameObject temp = GUIController.controller.buildTip;
            GameObject build = Instantiate(currentBuilding, temp.transform.position, temp.transform.rotation, transform);
            build.SetActive(true);
            buildInWorld.Add(build);
        }
        
    }

    public void RepairBuildings(){
        foreach(GameObject build in buildInWorld){
            build.SetActive(true);
            build.GetComponent<Health>().currentHealth = build.GetComponent<Health>().health;
        }
    }

    public void Reset(){
        foreach(GameObject build in buildInWorld){
            build.GetComponent<Building>().ForceDestroy();
        }

        overlap = false;
    }
}
