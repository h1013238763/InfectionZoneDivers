using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAddBuilding : Upgrade
{
    public List<GameObject> builds;

    public int listID;

    public override void Effect(){
        for(int i = 0; i < builds.Count; i ++){
            switch(listID){
                case 0:
                    if(!BuildController.controller.buildStructure.Contains(builds[i])){
                        BuildController.controller.buildStructure.Add(builds[i]);
                    }
                    break;    
                case 1:
                    if(!BuildController.controller.buildCraft.Contains(builds[i])){
                        BuildController.controller.buildCraft.Add(builds[i]);
                    }

                    break; 
                case 2:
                    if(!BuildController.controller.buildDefence.Contains(builds[i])){
                        BuildController.controller.buildDefence.Add(builds[i]);
                    }
                    break; 
                case 3:
                    if(!BuildController.controller.buildSurvivor.Contains(builds[i])){
                        BuildController.controller.buildSurvivor.Add(builds[i]);
                    }
                    break; 
                case 4:
                    if(!BuildController.controller.buildOther.Contains(builds[i])){
                        BuildController.controller.buildOther.Add(builds[i]);
                    }
                    break; 
                default:
                    break;
            }
        } 
    }
}
