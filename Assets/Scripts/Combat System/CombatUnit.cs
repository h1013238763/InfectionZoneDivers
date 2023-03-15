using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public Weapon currentWeapon;
    public GameObject ui;
    public int health;
    private float fireColddown;
    private float reloadTime;
    private bool fireAble;
    private string tag_1;
    private string tag_2;


    void Awake(){
        
    }

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
            ui.transform.localScale = new Vector3(reloadTime/currentWeapon.weaponReload, 0.1f, 0);
            if(fireColddown <= 0 && reloadTime <= 0){
                reloadTime = -0.1f;
                currentWeapon.weaponAmmoCurr += GetComponent<Inventory>().UseItem(currentWeapon.weaponAmmoIndex, currentWeapon.weaponAmmoCapa);
                fireAble = true;
                ui.SetActive(false);
            }
        }
    }

    public void SetWeapon(Weapon weapon){
        currentWeapon = weapon;
    }

    public void Fire(double radius){
        if(currentWeapon.weaponAmmoCurr <= 0 && fireAble){
            Reload();
        }

        if(fireAble && currentWeapon.weaponAmmoCurr > 0){
            
            for(int i = 0; i < currentWeapon.weaponBulletNum; i ++){
                radius += Random.Range(-(1-currentWeapon.weaponAccuracy), (1-currentWeapon.weaponAccuracy));
                CombatController.combatController.Attack(transform.position, currentWeapon, radius, tag_1, tag_2);
            }
            currentWeapon.weaponAmmoCurr --;
            fireColddown = 1 / currentWeapon.weaponSpeed;
            fireAble = false;
            Debug.Log(currentWeapon.weaponAmmoCurr + "/" + currentWeapon.weaponAmmoCapa);
        }
    }

    public void Reload(){
        
        if(GetComponent<Inventory>().FindItem(currentWeapon.weaponAmmoIndex)){
            fireAble = false;
            ui.SetActive(true);
            reloadTime = currentWeapon.weaponReload;
        }
        else{
            Debug.Log("No Ammo");
        }
        
    }


    public void OnHit(int damage){
        health -= damage;
    }

    public void TagInitial(string tag_1, string tag_2){
        this.tag_1 = tag_1;
        this.tag_2 = tag_2;
    }
}