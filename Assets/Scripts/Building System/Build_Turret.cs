using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.UI;

public class Build_Turret : MonoBehaviour
{
    public Weapon weapon;

    public bool survivorActive;
    public int survivorRequire = 1;

    public GameObject turret;
    
    void Start(){
        survivorActive = false;

        turret = CombatController.controller.GetTurret();

        turret.GetComponent<TurretCombat>().turretWeapon = weapon;
        turret.transform.position = transform.position + new Vector3(0.5f, 1, 0);
        turret.GetComponent<TurretCombat>().Assign(GetComponent<Invent>());
    }

    void OnEnable(){
        if(survivorActive)
            turret.SetActive(true);
    }

    public void Interact(){

        GUIController.controller.currentTurret = gameObject;
        GUIController.controller.ActivePanel("Turret", gameObject);
    }

    public void Active(){
        

        if(survivorActive){
            WorldController.controller.UseSurvivor(survivorRequire);
            survivorActive = false;
            turret.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            ItemController.controller.ItemGet(weapon.weaponAmmoIndex, turret.GetComponent<CombatUnit>().ammo, GetComponent<Invent>(), gameObject.tag);
            turret.GetComponent<CombatUnit>().ammo = 0;

            GUIController.controller.activeTurret.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Activate";
            return;
        }
        if(WorldController.controller.survivorNum >= survivorRequire){
            WorldController.controller.UseSurvivor(-survivorRequire);
            transform.GetChild(0).gameObject.SetActive(false);
            survivorActive = true;
            turret.SetActive(true);
            GUIController.controller.activeTurret.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Deactivate";
        }
    }

    void OnDisable() {
        turret.SetActive(false);
    }
}
