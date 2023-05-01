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
        
    }

    public void Interact(){
        if(!WorldController.controller.resourceBuild.Contains(gameObject))
            WorldController.controller.resourceBuild.Add(gameObject);
        if(survivorActive){
            WorldController.controller.survivorNum += survivorRequire;
            survivorActive = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 1f );
            return;
        }
        if(WorldController.controller.survivorNum >= survivorRequire){
            WorldController.controller.survivorNum -= survivorRequire;
            survivorActive = true;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 1f );
        }
    }
}
