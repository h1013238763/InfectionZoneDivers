using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeZombies : Upgrade
{
    
    public int kills;

    public GameObject zombie;

    public List<int> killList;
    public List<int> healthList;

    public override void Effect(){
        for(int i = 0; i < 4; i ++){
            if(kills >= killList[i]){
                zombie.GetComponent<Health>().health = healthList[i];
            }
        }
    }
}
