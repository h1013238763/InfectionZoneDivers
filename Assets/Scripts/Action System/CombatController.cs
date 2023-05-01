using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class CombatController : MonoBehaviour{

    public static CombatController controller;
    public List<GameObject> bulletPool;
    public List<GameObject> projectilePool;
    public GameObject bulletSpritePool;
    public GameObject bulletPrefab;
    public int bulletCapacity;

    void Awake(){
        controller = this;
        bulletCapacity = 20;
    }

    void Start(){
        bulletPool = new List<GameObject>();
        for(int i = 0; i < bulletCapacity; i ++)
            AddBulletToPool();

    }
    
    /// Add bullet to bulletPool
    private void AddBulletToPool(){
        GameObject temp;
        temp = Instantiate(bulletPrefab);
        temp.SetActive(false);
        temp.transform.parent = bulletSpritePool.transform;
        bulletPool.Add(temp);
    }
    /// Get an inactive object from the bullet pool
    /// If there are not enough inactive bullets, create more bullets into the bullet pool
    private GameObject GetBullet(){
        foreach( GameObject bullet in bulletPool ){
            if(!bullet.activeInHierarchy)
                return bullet;
        }
        bulletCapacity ++;
        AddBulletToPool();
        return bulletPool[bulletCapacity];
    }

    public void Attack(Vector2 from, Weapon weapon, double radius, string tag){
        // set fire direction
        radius += Random.Range(-(1-weapon.weaponAccuracy), (1-weapon.weaponAccuracy));
        Vector2 direct = new Vector2((float)(weapon.weaponRange*Cos(radius))+from.x, (float)(weapon.weaponRange*Sin(radius))+from.y);
        
        // create raycast
        RaycastHit2D hit = Physics2D.Raycast(from, direct, weapon.weaponRange, LayerMask.GetMask("tag"));

        DrawBullet(from, direct, GetBullet());
        // hit event
        if(hit.collider != null){
            if(hit.collider.tag == tag){
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

    /// It draw the trajectory of the bullet type attack
    private void DrawBullet(Vector2 attackPos, Vector2 hitPos, GameObject bulletPrefab){

        LineRenderer line = bulletPrefab.GetComponent<LineRenderer>();
        bulletPrefab.SetActive(true);
            
        line.SetPosition(0, attackPos);
        line.SetPosition(1, hitPos);
    }
}
