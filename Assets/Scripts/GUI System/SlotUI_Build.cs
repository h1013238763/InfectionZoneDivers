using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Build : MonoBehaviour
{
    public int slotID;

    public void Reset(int id, Sprite sprite){
        slotID = id;

        transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
        transform.GetChild(1).gameObject.GetComponent<Text>().text = BuildController.controller.currBuildList[id].GetComponent<Building>().buildName;
        for(int i = 0; i < 4; i ++){
            transform.GetChild(3).GetChild(i).gameObject.GetComponent<Text>().text = BuildController.controller.currBuildList[id].GetComponent<Building>().buildRequire[i].ToString();
        }

        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
        slotID = -1;
    }

    public void MouseEnter(){
        if(slotID != -1)
            GUIController.controller.ShowBuildDetail(slotID);
    }

    // while mouse exit, stop showing item details
    public void MouseExit(){
        GUIController.controller.HideItemDetail();
    }

    public void Assign(){
        GUIController.controller.rotate = 0;
        BuildController.controller.AssignCurrentBuilding(slotID);
    }
}
