using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    public static WorldController controller;
    public int dayNum;
    [Space(10)]

    public float progressPoint;
    public int survivorNum;
    public int survivorCount;
    public int resourceScale = 400;
    public int difficultyScale;
    public int tempDifficulty;
    [Space(10)]
    
    public float plasticPercent;
    public float electricPercent;
    public float chemicalPercent;
    [Space(10)]

    public List<GameObject> resourceBuild;
    public int[] resource = new int[4];
    [Space(10)]

    public TilemapRenderer map;
    int worldScale;

    public int[] zombieKills = new int[3];
    public List<UpgradeZombies> zombieUpgrade;

    public int stage;           // 0 = peace stage; 1 = battle stage;

    void Start(){
        controller = this;

        dayNum = 1;
    }

    public bool ResourceCheck(int[] num){
        for(int i = 0; i < 4; i ++){
            if(num[i] > resource[i]){
                if(!GUIController.controller.textTip.activeSelf){
                    GUIController.controller.SetTextTip("Not enough resources");     
                }
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
    
    public void ResourceClear(){
        resource = new int[]{0, 0, 0, 0};
    }

    public void HideSurvivorPanel(){
        GUIController.controller.ExitPanels();
    }

    public void IntoPeaceStage(){
        GUIController.controller.SetTextTip("We're safe... for now");

        stage = 0;

        BuildController.controller.RepairBuildings();

        dayNum ++;
        GUIController.controller.SetTimePanel(dayNum);
        
        ReceiveSurvivor();

        foreach( GameObject build in resourceBuild ){
            if(build.GetComponent<Build_Gather>().survivorActive){
                resource[build.GetComponent<Build_Gather>().resourceType] += build.GetComponent<Build_Gather>().resourceNum;
                GUIController.controller.SetResourcePanel();
            }
        }
    }

    public void IntoBattleStage(){
        if(tempDifficulty == 0)
            return;
        difficultyScale = tempDifficulty;

        stage = 1;
        EnemyController.controller.CombatStart(difficultyScale);
        GUIController.controller.ExitPanels();
    }
    
    public void SetDifficultyScale(int scale){
        tempDifficulty = scale;
    }

    public void ReceiveSurvivor(){

        survivorCount += difficultyScale;
        UseSurvivor(difficultyScale);

        progressPoint = 1 + 0.1f * dayNum + worldScale;
        // resource total get from survivors everyday
        // = progress point [get from kill zombie or building] + ( 1 + 0.25f*(difficultyScale-1) )[from 1 to 1.5] + survivorNum * resourceScale;
        int point = (int)(progressPoint * (1 + 0.25f*(difficultyScale-1)) + survivorCount * resourceScale);
        int[] resourceList = new int[4];

        resourceList[2] = (int)(point * Random.Range(electricPercent - 0.05f, electricPercent + 0.05f));
        resourceList[1] = (int)(point * Random.Range(plasticPercent  - 0.05f, plasticPercent  + 0.05f));
        resourceList[3] = (int)(point * Random.Range(chemicalPercent - 0.05f, chemicalPercent + 0.05f));
        resourceList[0] = point - resourceList[1] - resourceList[2] - resourceList[3];
        WorldController.controller.ResourceGet(resourceList);
        
        difficultyScale = 0;
    }

    public void UseSurvivor(int num){
        survivorNum += num;
        GUIController.controller.resourcePanel.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = survivorNum.ToString();
    }

    public void GameEnd(int endState){
        // 0 = retreat
        if(endState == 0){
            for(int i = 0; i < 4; i ++){
                ShelterController.controller.resource[i] += resource[i];
            }
            ShelterController.controller.survivor += survivorCount;
        }
        // 1 = died or fail or core be destroyed
        else{
            for(int i = 0; i < 4; i ++){
                ShelterController.controller.resource[i] += (int)(resource[i] / 2f);
            }
            ShelterController.controller.survivor += (int)(survivorCount / 7f);
        }

        ShelterController.controller.ActiveShelterGUI();
        for(int i = 0; i < 3; i ++){
            zombieUpgrade[i].kills += zombieKills[i];
        }
        PlayerAction.player.SetActionStage(3);

        ShelterController.controller.ActiveShelterGUI();
    }

    public void Reset(){
        ResourceClear();
        zombieKills = new int[3];
        survivorNum = 0;
        survivorCount = 0;
        difficultyScale = 0;
        tempDifficulty = 0;
        resourceBuild.Clear();
        stage = 0;
        dayNum = 1;
    }

    public void ChangeWorld(){
        map.enabled = false;
        worldScale = 1;
    }
}
