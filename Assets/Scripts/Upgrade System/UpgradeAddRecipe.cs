using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAddRecipe : Upgrade
{
    public Build_Bench bench;

    public List<Item> itemAdd;
    public List<int> itemNum;

    public override void Effect(){
        for(int i = 0; i < itemAdd.Count; i ++){
            bench.AddFormula(itemAdd[i], itemNum[i]);
        }
    }
}
