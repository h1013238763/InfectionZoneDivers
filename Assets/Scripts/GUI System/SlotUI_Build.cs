using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Build : MonoBehaviour
{
    public int buildID;
    [SerializeField]private GameObject imageObj;

    public void Reset(int build, Sprite sprite){
        this.buildID = build;
        imageObj.GetComponent<Image>().sprite = sprite;
        gameObject.SetActive(true);
    }

    public void Assign(){
        GameObject.Find("GUIController").GetComponent<GUIController>().SetBuildTipSprite(imageObj.GetComponent<Image>().sprite);
        GameObject.Find("BuildingController").GetComponent<BuildController>().AssignCurrentBuilding(buildID);
    }
}
