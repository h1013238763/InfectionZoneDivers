using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_1 : Upgrade
{
    public override void Effect(){
        GameObject pObject = PlayerAction.player.gameObject;
        PlayerAction.player.weaponSlot[0] = (Weapon)ItemController.controller.database.itemDict[1];
        ItemController.controller.ItemGet(4, 100, pObject.transform.GetComponent<Invent>(), "Player");

        PlayerAction.player.SetWeapon();
    }


}
