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

    void Start(){

        controller = this;

        dayNum = 1;
    }

    public void Hide(){
        tempScale = 0;
        GUIController.controller.ExitPanels();
    }

    public void AddDay(){
        dayNum ++;
        GUIController.controller.SetTimePanel(dayNum);
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
        ItemController.controller.ResourceGet(resourceList);
        
        tempScale = 0;
        AddDay();
        GUIController.controller.ExitPanels();
    }

    public void GameEnd(int endState){
        // 0 = retreat
        // 1 = died or fail or core be destroyed
    }

}
