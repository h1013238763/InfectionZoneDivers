using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public Weapon currentWeapon;
    public int health;
    private float fireColddown;
    private float reloadTime;
    private bool fireAble;
    private string tag_1;
    private string tag_2;
    private int currAmmo;


    void Awake(){
        fireAble = true;
    }

    void FixedUpdate(){

        /// fire colddown control
        if(fireColddown > -0.1){
            fireColddown -= Time.deltaTime;
            if(fireColddown <= 0 && reloadTime <= 0){
                fireAble = true;
            }
        }
        /// reload time control
        if(reloadTime > -0.1){
            reloadTime -= Time.deltaTime;
            if(fireColddown <= 0 && reloadTime <= 0){
                currAmmo = currentWeapon.weaponAmmoCapa;
                fireAble = true;
            }
        }
    }

    public void Fire(double radius){
        if(currAmmo == 0 && fireAble){
            fireAble = false;
            reloadTime = currentWeapon.weaponReload;
        }

        if(fireAble){
            fireAble = false;
            fireColddown = currentWeapon.weaponSpeed;
            currAmmo --;
            CombatController.combatController.Attack(transform.position, currentWeapon, radius, tag_1, tag_2);
            Debug.Log(currAmmo + "/" + currentWeapon.weaponAmmoCapa);
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