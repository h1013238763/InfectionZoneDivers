using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]private Weapon turretWeapon;
    public List<GameObject> enemyList;
    [SerializeField]private float idleTime;
    [SerializeField]private Quaternion idleTarget;
    public List<ShortItem> invent = new List<ShortItem>();

    void Update(){
        if(enemyList.Count > 0)
            OnCombat();
        else{
            OnIdle();
            idleTime -= Time.deltaTime;
        }
            
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
        idleTarget = transform.GetChild(0).rotation;
        idleTime = 3;
    }

    private void OnCombat(){
        if(!enemyList[0].activeSelf){
            enemyList.RemoveAt(0);
            return;
        }
        GameObject enemy = enemyList[0];
        float angle = (float)(Atan2(( enemy.transform.position.y - (transform.position.y+1) ),( enemy.transform.position.x - (transform.position.x+0.5) )) * 180 / PI);
        transform.GetChild(0).rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 180, -(angle+180));
        GetComponent<CombatUnit>().Fire(angle * PI / 180);
        idleTarget = transform.GetChild(0).rotation;
    }

    private void OnIdle(){
        if(idleTime <= 0){
            idleTime = Random.Range(5,15);
            if((int)idleTarget.z == (int)transform.GetChild(0).rotation.z){
                idleTarget = Quaternion.Euler(0, transform.GetChild(0).rotation.y, Random.Range(-90f, 90f));   
            }
        }
        transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, idleTarget, 0.3f); 
    }
}
