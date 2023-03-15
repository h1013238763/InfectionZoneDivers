using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintUI : MonoBehaviour
{
    [SerializeField]private BuildingDatabase buildingDatabase;

    public List<int> buildListCurrent = new List<int>();
    public List<int> buildListStruct = new List<int>();
    public List<int> buildListStorage = new List<int>();
    public List<int> buildListManufactory = new List<int>();
    public List<int> buildListDefence = new List<int>();
    public List<int> buildListPower = new List<int>();
    public List<int> buildListSpecial = new List<int>();

    public List<GameObject> buildSlots = new List<GameObject>();
    [SerializeField]private GameObject slotPrefab;

    void ChangeBuildList(int list){
        switch (list)
        {
            case 0:
                buildListCurrent = buildListStruct;
                break;
            case 1:
                buildListCurrent = buildListStorage;
                break;
            case 2:
                buildListCurrent = buildListManufactory;
                break;
            case 3:
                buildListCurrent = buildListDefence;
                break;
            case 4:
                buildListCurrent = buildListPower;
                break;
            case 5:
                buildListCurrent = buildListSpecial;
                break;
            default:
                break;
        }
        RefreshUI();
    }

    // set blueprint ui panel and slot to building list current.count
    // set each slot with building sprite by building id
    void RefreshUI(){

        for(int i = 0; i < buildListCurrent.Count; i ++){
            buildSlots[i].GetComponent<BuildingSlotUI>().Reset(buildListCurrent[i], buildingDatabase.buildingDict[buildListCurrent[i]].GetComponent<Building>().buildingSprite); 
        }
        for(int i = buildListCurrent.Count; i < buildSlots.Count; i ++){
            buildSlots[i].SetActive(false);
        }
    }
}
