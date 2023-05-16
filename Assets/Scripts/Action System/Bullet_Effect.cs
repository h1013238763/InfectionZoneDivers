using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Effect : MonoBehaviour
{
    [SerializeField]private float effectTime = 0;
    public float repeatTime;
    public float repeatCurr;
    public int effectDamage;

    public Transform enemyPool;

    [SerializeField]private Rigidbody2D rigidBody;
    
    void FixedUpdate(){

        if(effectTime > 0){
            effectTime -= Time.deltaTime;
            if(repeatCurr > 0){
                repeatCurr -= Time.deltaTime;
            }
            else{
                
                HitEnemyInRange();

                repeatCurr = repeatTime;
            }
        }
        else{
            gameObject.SetActive(false);
        }
    }

    public void SetEffect(int damage, float repeat, float effect){
        effectDamage = damage;
        repeatTime = repeat;
        repeatCurr = 0;
        effectTime = effect;
    }

    public void HitEnemyInRange(){
        enemyPool = GameObject.Find("Combat Controller").transform.GetChild(1);
        GameObject enemy;
        float x;
        float y;
        float distance;
        for(int i = 0; i < enemyPool.childCount; i ++){
            if(enemyPool.GetChild(i).gameObject.activeSelf){
                
                enemy = enemyPool.transform.GetChild(i).gameObject;

                x = enemy.transform.position.x - transform.position.x;
                y = enemy.transform.position.y - transform.position.y;

                distance = (x * x) + (y * y);

                if(distance < 9){
                    enemy.GetComponent<Health>().TakeDamage(effectDamage);
                }
            }
        }
    }
}
