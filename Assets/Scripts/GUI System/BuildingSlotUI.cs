using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlotUI : MonoBehaviour
{
    public int buildID;
    [SerializeField]private GameObject imageObj;

    public void Reset(int build, Sprite sprite){
        this.buildID = build;
        imageObj.GetComponent<Image>().sprite = sprite;
        gameObject.SetActive(true);
    }

    public void Print(){
        if(buildID != 0)
            Debug.Log(buildID);
    }
}
