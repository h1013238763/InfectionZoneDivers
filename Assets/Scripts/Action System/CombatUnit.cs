using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public Weapon weapon;
    public int ammo;
    public float fireColddown;
    public float reloadTime;
    private bool fireAble;

    public GameObject muzzle;

    void FixedUpdate(){

        /// fire colddown control
        if(fireColddown > -0.1){
            fireColddown -= Time.deltaTime;
            if(fireColddown <= 0 && reloadTime <= 0){
                fireColddown = -0.1f;
                fireAble = true;
            }
        }
        /// reload time control
        if(reloadTime > -0.1){
            reloadTime -= Time.deltaTime;
            if(fireColddown <= 0 && reloadTime <= 0){
                reloadTime = -0.1f;
                int tempAmmo = ItemController.controller.ItemUse( weapon.weaponAmmoIndex, weapon.weaponAmmoCapa-ammo, gameObject.GetComponent<Invent>(), "Enemy");
                ammo += tempAmmo;
                fireAble = true;
                if(gameObject.tag == "Player"){
                    PlayerAction.player.ammoSlot[0] += tempAmmo;
                    GUIController.controller.SetAmmoText();
                    GUIController.controller.SetAmmoInventText();
                }
            }
        }
    }

    public void SetWeapon(Weapon weapon, int ammo){
        this.weapon = weapon;
        this.ammo = ammo;
    }

    public void Fire(double radius, bool isPlayer){
        if(ammo <= 0 && fireAble){
            Reload();
        }

        if(fireAble && ammo > 0){
            
            muzzle.transform.GetComponent<MuzzleFire>().Fire();
            CombatController.controller.Attack(muzzle.transform.position, weapon, radius);
            
            ammo --;
            fireColddown = 1 / weapon.weaponSpeed;
            fireAble = false;
            // player fire colddown animation
            if(isPlayer){
                PlayerAction.player.ammoSlot[0] = ammo;
                GUIController.controller.SetFireColdTip(fireColddown, fireColddown);
                GUIController.controller.SetAmmoText();
            }
        }        
    }

    public void Reload(){
        if(ammo == weapon.weaponAmmoCapa)
            return;
        if(ItemController.controller.ItemNumber(weapon.weaponAmmoIndex, gameObject.GetComponent<Invent>()) > 0){
            GUIController.controller.SetReloadTip(weapon.weaponReload, gameObject, true);
            fireAble = false;
            reloadTime = weapon.weaponReload;
        }
        else{
            Debug.Log("No Ammo");
        }
    }

    public void OnHit(int damage){
        
    }
}