using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour{
    
    [SerializeField]private int zombieHealthMax;
    private float zombieMoveSpeed;

    public void OnHit(int hit){

    }

    private void OnEnable(){
        GetComponent<CombatUnit>().health = zombieHealthMax;
    }

    private void FixedUpdate(){

    }

    private void OnDied(){
        gameObject.SetActive(false);
    }
}
