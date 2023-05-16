using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUnlockMap : Upgrade
{
    public override void Effect(){
        EnemyController.controller.worldScale = 1;
        WorldController.controller.ChangeWorld();
    }
}
