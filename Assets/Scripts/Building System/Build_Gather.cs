using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Gather : MonoBehaviour
{
    public int survivorRequire;
    public bool survivorActive;

    public int resourceType;
    public int resourceNum;

    void Start(){
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Interact(){
        if(!WorldController.controller.resourceBuild.Contains(gameObject))
            WorldController.controller.resourceBuild.Add(gameObject);
        if(survivorActive){
            WorldController.controller.UseSurvivor(survivorRequire);
            survivorActive = false;
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        if(WorldController.controller.survivorNum >= survivorRequire){
            WorldController.controller.UseSurvivor(-survivorRequire);
            survivorActive = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
