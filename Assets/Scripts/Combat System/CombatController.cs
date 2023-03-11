using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CombatController : MonoBehaviour{

    public static CombatController combatController;
    public List<GameObject> bulletPool;
    public List<GameObject> projectilePool;
    public GameObject bulletPrefab;
    public int bulletCapacity;

    void Awake(){
        combatController = this;
        bulletCapacity = 20;
    }

    void Start(){
        bulletPool = new List<GameObject>();
        for(int i = 0; i < bulletCapacity; i ++)
            AddBulletToPool();

    }

    public void Attack(Vector2 from, Weapon weapon, double radius, string tag_1, string tag_2){
        // set fire direction
        Vector2 direct = new Vector2((float)(weapon.weaponRange*Math.Cos(radius)), (float)(weapon.weaponRange*Math.Sin(radius)));
        // create raycast
        RaycastHit2D hit = Physics2D.Raycast(from, direct, weapon.weaponRange, LayerMask.GetMask("Hitbox"));
        // hit event
        //Debug.DrawRay(transform.GetChild(0).position, direct, Color.green);
        if(hit.collider != null){
            if(hit.collider.tag == tag_1 || hit.collider.tag == tag_2){
                // do damage
                hit.collider.gameObject.GetComponent<CombatUnit>().OnHit(weapon.weaponDamage);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce( -hit.normal * 20);
            }
            Vector2 hitPos = hit.point;
            DrawBullet(from, hitPos, GetBullet());
        }
        else{
            DrawBullet(from, direct, GetBullet());
        }
    }

    /// <summary>
    /// Add bullet to bulletPool
    /// </summary>
    private void AddBulletToPool(){
        GameObject temp;
        temp = Instantiate(bulletPrefab);
        temp.SetActive(false);
        temp.transform.parent = transform;
        bulletPool.Add(temp);
    }

    /// <summary>
    /// Get an inactive object from the bullet pool
    /// If there are not enough inactive bullets
    /// create more bullets into the bullet pool
    /// </summary>
    /// <returns>inactive bullet object</returns>
    private GameObject GetBullet(){
        foreach( GameObject bullet in bulletPool ){
            if(!bullet.activeInHierarchy)
                return bullet;
        }
        bulletCapacity ++;
        AddBulletToPool();
        return bulletPool[bulletCapacity];
    }

    /// <summary>
    /// It draw the trajectory of the bullet type attack
    /// </summary>
    /// <param name="attackPos">attack source position</param>
    /// <param name="hitPos">hit target position</param>
    /// <param name="bulletPrefab">bullet for drawing</param>
    private void DrawBullet(Vector2 attackPos, Vector2 hitPos, GameObject bulletPrefab){

        LineRenderer line = bulletPrefab.GetComponent<LineRenderer>();
        bulletPrefab.SetActive(true);
            
        line.SetPosition(0, attackPos);
        line.SetPosition(1, hitPos);
    }

    private void DrawProjectile(){

    }
}
