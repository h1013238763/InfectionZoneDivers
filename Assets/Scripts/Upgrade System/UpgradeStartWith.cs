using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStartWith : Upgrade
{
    public Weapon weapon;

    public Item ammo;
    public int ammoNum;

    public int[] r = new int[4];

    public override void Effect(){
        PlayerAction.player.weaponSlot[0] = weapon;

        ItemController.controller.DropItemSet(ammo.itemID, ammoNum, new Vector2(100.5f, 96));

        PlayerAction.player.SetWeapon();

        WorldController.controller.ResourceGet(r);
    }
}
