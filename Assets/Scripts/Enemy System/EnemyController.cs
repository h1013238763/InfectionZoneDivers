using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController controller;

    public List<GameObject> enemyPool;
    public int enemyActiveCount;

    public GameObject walker;
    public GameObject runner;
    public GameObject tanker;

    public Vector2 spawnPoint;

    public int worldScale;

    public float dropChance = 0.05f;

    float[] enemyPercent;

    void Start(){
        controller = this;

        enemyPool = new List<GameObject>();

        Reset();
    }

    public void Reset(){

        Transform enemySpritePool = transform.GetChild(1);

        foreach(Transform enemy in enemySpritePool){
            GameObject.Destroy(enemy.gameObject);
        }
        enemyPool.Clear();

        switch(worldScale){
            case 0:
                enemyPercent = new float[]{0.65f, 0.25f, 0.1f};
                break;
            case 1:
                enemyPercent = new float[]{0.5f, 0.3f, 0.2f};
                break;
            default:
                break;
        }
        
        for(int i = 0; i < 10; i ++){
            AddEnemyToPool();
        }
    }
    
    /// Add bullet to bulletPool
    private float AddEnemyToPool(){
        float tempFloat = Random.Range(0, 1f);
        GameObject tempEnemy;
        float tempNum;

        if(tempFloat <= enemyPercent[0]){
            tempEnemy = Instantiate(walker);
            tempNum = 1f;
        }
        else if(tempFloat <= enemyPercent[1] + enemyPercent[0]){
            tempEnemy = Instantiate(runner);
            tempNum = 1.25f;
        }
        else{
            tempEnemy = Instantiate(tanker);
            tempNum = 1.5f;
        }

        enemyPool.Add(tempEnemy);
        tempEnemy.transform.SetParent(transform.GetChild(1));
        tempEnemy.SetActive(false);

        return tempNum;

    }
    /// Get an inactive object from the bullet pool
    /// If there are not enough inactive bullets, create more bullets into the bullet pool
    private GameObject GetEnemy(){
        foreach( GameObject enemy in enemyPool ){
            if(!enemy.activeInHierarchy)
                return enemy;
        }
        AddEnemyToPool();
        return enemyPool[enemyPool.Count];
    }

    public void CombatStart(int difficulty){
        float enemyCoeff = difficulty * (worldScale+1) * 2;

        while(enemyCoeff > 0){
            enemyCoeff -= AddEnemyToPool();
        }

        float temp = Random.Range(0, 1f);
        string side = "";

        if(temp < 0.5f){
            // left
            if(temp < 0.25f){
                spawnPoint.x = 33;
                side = "west";
            }
            // right
            else{
                spawnPoint.x = 167;
                side = "east";
            }
            spawnPoint.y = Random.Range(33f, 167f);
        }
        else{
            //down
            if(temp < 0.75f){
                spawnPoint.y = 33f;
                side = "south";
            }
            //top
            else{
                spawnPoint.y = 167f;
                side = "north";
            }
            spawnPoint.x = Random.Range(33f, 167f);
        }
        GUIController.controller.SetTextTip("A group of zombies is coming from the " + side + "!");

        
        foreach(GameObject enemy in enemyPool){
            enemy.transform.position = new Vector2( spawnPoint.x + Random.Range(-3f, 3f), spawnPoint.y + Random.Range(-3f, 3f) );
            enemy.GetComponent<Enemy>().moveSpeed *= 1.1f;
            enemy.GetComponent<Health>().health += 5;
            enemy.GetComponent<Health>().currentHealth = enemy.GetComponent<Health>().health; 
            enemy.SetActive(true);
        }

        enemyActiveCount = enemyPool.Count;
    }

    public void EnemyDies(int id){
        enemyActiveCount --;
        WorldController.controller.zombieKills[id] ++;
        if(enemyActiveCount <= 0){
            foreach(GameObject enemy in enemyPool)
                enemy.SetActive(false);

            WorldController.controller.IntoPeaceStage();
        }
    }
}
