using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Math;

public class TurretCombat : MonoBehaviour
{
    public Weapon turretWeapon;
    public List<GameObject> enemyList;

    public Transform weaponSprite;

    public CombatUnit combat;

    public void Assign(Invent invent){
        combat = GetComponent<CombatUnit>();
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = turretWeapon.itemSprite;
        combat.SetWeapon(turretWeapon, 0);
        combat.invent = invent;
        weaponSprite = transform.GetChild(0);
    }
        
    void FixedUpdate(){
        if(enemyList.Count > 0)
            OnCombat();           
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Enemy"){
            enemyList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Enemy"){
            if(enemyList.Contains(collision.gameObject))
                enemyList.Remove(collision.gameObject);
        }
    }

    private void OnCombat(){
        if(!enemyList[0].activeSelf){
            enemyList.RemoveAt(0);
            return;
        }
        GameObject enemy = enemyList[0];
        float angle = (float)(Atan2(( enemy.transform.position.y - (combat.muzzle.transform.position.y) ),( enemy.transform.position.x - (combat.muzzle.transform.position.x) )) * 180 / PI);
        weaponSprite.rotation = Quaternion.Euler(0, 0, angle);
        weaponSprite.GetComponent<SpriteRenderer>().flipY = (angle > 90 || angle < -90);
        GetComponent<CombatUnit>().Fire(angle * PI / 180, false);
        GetComponent<SpriteRenderer>().flipX = (angle > 90 || angle < -90);
        combat.flip = GetComponent<SpriteRenderer>().flipX;
    }
}
