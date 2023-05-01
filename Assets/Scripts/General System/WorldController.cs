using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController controller;
    public int dayNum;
    [Space(10)]

    public int progressPoint;
    public int survivorNum;
    public int resourceScale;
    public int difficultyScale;
    public int tempScale;
    [Space(10)]
    
    public float chemicalPercent;
    public float plasticPercent;
    public float electricPercent;
    [Space(10)]

    public List<GameObject> resourceBuild;
    public int[] resource = new int[4];
    [Space(10)]

    public int zombieKills;

    public int stage;           // 0 = peace stage; 1 = battle stage;

    void Start(){

        controller = this;

        dayNum = 1;
    }

    public bool ResourceCheck(int[] num){
        for(int i = 0; i < 4; i ++){
            if(num[i] > resource[i]){
                Debug.Log("Insufficient Resource");
                return false;
            }     
        }
        return true;
    }

    public void ResourceGet(int[] num){
        for(int i = 0; i < 4; i ++)
            resource[i] += num[i];
        GUIController.controller.SetResourcePanel();
    }

    /// <summary>
    /// Use resources
    /// </summary>
    /// <param name="num"> the num of change </param>
    /// <returns> if there are enough resource to change </returns>
    public void ResourceUse(int[] num){
        for(int i = 0; i < 4; i ++)
            resource[i] -= num[i];
        GUIController.controller.SetResourcePanel();
    }
    

    public void HideSurvivorPanel(){
        tempScale = 0;
        GUIController.controller.ExitPanels();
    }

    public void AddDay(){
        dayNum ++;
        GUIController.controller.SetTimePanel(dayNum);
        foreach( GameObject build in resourceBuild ){
            if(build.GetComponent<Building>().buildComplete && build.GetComponent<Build_Gather>().survivorActive){
                resource[build.GetComponent<Build_Gather>().resourceType] += build.GetComponent<Build_Gather>().resourceNum;
                GUIController.controller.SetResourcePanel();
            }
        }
    }
    
    public void SetDifficultyScale(int scale){
        tempScale = scale;
        difficultyScale = scale;
    }

    public void ReceiveSurvivor(){
        if(tempScale == 0)
            return;

        survivorNum += difficultyScale;
        // resource total get from survivors everyday
        // = progress point [get from kill zombie or building] + ( 1 + 0.25f*(difficultyScale-1) )[from 1 to 1.5] + survivorNum * resourceScale;
        int point = (int)(progressPoint * (1 + 0.25f*(difficultyScale-1)) + survivorNum * resourceScale);
        int[] resourceList = new int[4];

        resourceList[2] = (int)(point * Random.Range(electricPercent - 0.05f, electricPercent + 0.05f));
        resourceList[1] = (int)(point * Random.Range(plasticPercent  - 0.05f, plasticPercent  + 0.05f));
        resourceList[3] = (int)(point * Random.Range(chemicalPercent - 0.05f, chemicalPercent + 0.05f));
        resourceList[0] = point - resourceList[1] - resourceList[2] - resourceList[3];
        WorldController.controller.ResourceGet(resourceList);
        
        tempScale = 0;
        AddDay();
        GUIController.controller.ExitPanels();
    }

    public void GameEnd(int endState){
        // 0 = retreat
        if(endState == 0){
            for(int i = 0; i < 4; i ++)
                ShelterController.controller.resource[i] += resource[i];

            ShelterController.controller.survivor += survivorNum;
        }
        // 1 = died or fail or core be destroyed
    }
}
