using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public Weapon weapon;
    public int ammo = 0;
    private float fireColddown;
    private float reloadTime;
    private bool fireAble;

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
                ammo += ItemController.controller.ItemUse( weapon.weaponAmmoIndex, weapon.weaponAmmoCapa-ammo, gameObject.GetComponent<Invent>().inventList);
                fireAble = true;
                if(gameObject.tag == "Player"){
                    GUIController.controller.SetAmmoText(ammo, weapon.weaponAmmoCapa);
                    GUIController.controller.SetAmmoInventText(weapon.weaponAmmoIndex);
                }
            }
        }
    }

    public void SetWeapon(Weapon weapon, int ammo){
        this.weapon = weapon;
        this.ammo = ammo;
    }

    public void Fire(double radius){
        if(ammo <= 0 && fireAble){
            Reload();
        }

        if(fireAble && ammo > 0){
            CombatController.controller.Attack(transform.position, weapon, radius, gameObject.tag);

            ammo --;
            fireColddown = 1 / weapon.weaponSpeed;
            fireAble = false;
            if(gameObject.tag == "Player"){
                GUIController.controller.SetAmmoText(ammo, weapon.weaponAmmoCapa);
            }
        }
    }

    public void Reload(){

        if(ItemController.controller.ItemNumber(weapon.weaponAmmoIndex, gameObject.GetComponent<Invent>().inventList) > 0){
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