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

    public void Attack(Vector2 from, Weapon weapon, double radius){
        // set fire direction
        radius += Random.Range(-(1-weapon.weaponAccuracy), (1-weapon.weaponAccuracy));
        Vector2 direct = new Vector2((float)(Cos(radius)), (float)(Sin(radius)));
        
        GameObject bullet = GetBullet();
        bullet.SetActive(true);
        bullet.transform.GetComponent<Bullet>().Fire(from, direct, radius, weapon.weaponDamage, weapon.weaponRange);
        // create raycast
        

    }

    /// It draw the trajectory of the bullet type attack
    private void DrawBullet(Vector2 attackPos, Vector2 hitPos, GameObject bulletPrefab){

        LineRenderer line = bulletPrefab.GetComponent<LineRenderer>();
        bulletPrefab.SetActive(true);
            
        line.SetPosition(0, attackPos);
        line.SetPosition(1, hitPos);
    }
}
