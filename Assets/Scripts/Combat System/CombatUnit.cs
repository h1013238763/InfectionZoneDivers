using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public Weapon currentWeapon;
    public int health;
    private float fireColddown;
    private bool fireAble;
    private string tag_1;
    private string tag_2;

    void Awake(){
        fireAble = true;
    }

    void FixedUpdate(){

        /// fire colddown control
        if(fireColddown > -0.2){
            fireColddown -= Time.deltaTime;
            if(fireColddown <= 0){
                fireAble = true;
            }
        }
    }

    public void Fire(double radius){
        if(fireAble){
            fireAble = false;
            fireColddown = currentWeapon.weaponSpeed;
            CombatController.combatController.Attack(transform.position, currentWeapon, radius, tag_1, tag_2);
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