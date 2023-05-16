using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class CombatController : MonoBehaviour{

    public static CombatController controller;
    public List<GameObject> bulletPool;
    public List<GameObject> explosivePool;
    public List<GameObject> effectPool;
    public List<GameObject> turretPool;

    public GameObject bulletSpritePool;
    public GameObject bulletPrefab;
    public int bulletCapacity;

    public GameObject explosiveSpritePool;
    public GameObject explosivePrefab;

    public GameObject effectSpritePool;
    public GameObject effectPrefab;

    public GameObject turretSpritePrefab;
    public GameObject turretPrefab; 

    void Awake(){
        controller = this;
        bulletCapacity = 20;
    }

    void Start(){
        bulletPool = new List<GameObject>();
        for(int i = 0; i < bulletCapacity; i ++)
            AddBulletToPool();

        InitialExplosivePool();
        InitialEffectPool();
    }
    
    private void InitialExplosivePool(){
        for(int i = 0; i < 10; i ++){
            AddExplosive();
        }
    }

    private void AddExplosive(){
        GameObject temp = Instantiate(explosivePrefab);
        explosivePool.Add(temp);
        temp.transform.SetParent(explosiveSpritePool.transform);
        temp.SetActive(false);
    }

    public GameObject GetExplosiveFromPool(){
        foreach(GameObject explosive in explosivePool){
            if(!explosive.activeSelf){
                return explosive;
            }
        }
        AddExplosive();
        return explosivePool[explosivePool.Count-1];
    }

    private void InitialEffectPool(){
        for(int i = 0; i < 10; i ++){
            AddEffect();
        }
    }

    private void AddEffect(){
        GameObject temp = Instantiate(effectPrefab);
        effectPool.Add(temp);
        temp.transform.SetParent(effectSpritePool.transform);
        temp.SetActive(false);
    }

    public GameObject GetEffectFromPool(){
        foreach(GameObject effect in effectPool){
            if(!effect.activeSelf){
                return effect;
            }
        }
        AddEffect();
        return effectPool[effectPool.Count-1];
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

    private void AddTurret(){
        GameObject temp = Instantiate(turretPrefab);
        turretPool.Add(temp);
        temp.transform.SetParent(turretSpritePrefab.transform);
        temp.SetActive(false);
    } 

    public GameObject GetTurret(){
        AddTurret();
        return turretPool[turretPool.Count-1];
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

    public void Reset(){
        foreach(Transform sprite in bulletSpritePool.transform){
            GameObject.Destroy(sprite.gameObject);
        }
        foreach(Transform sprite in explosiveSpritePool.transform){
            GameObject.Destroy(sprite.gameObject);
        }
        foreach(Transform sprite in effectSpritePool.transform){
            GameObject.Destroy(sprite.gameObject);
        }
        foreach(Transform sprite in turretSpritePrefab.transform){
            GameObject.Destroy(sprite.gameObject);
        }

        bulletPool.Clear();
        explosivePool.Clear();
        effectPool.Clear();
        turretPool.Clear();

        for(int i = 0; i < bulletCapacity; i ++)
            AddBulletToPool();
        InitialExplosivePool();
        InitialEffectPool();
    }
}
