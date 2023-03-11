using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float bulletTime;

    private void FixedUpdate(){
        bulletTime -= Time.deltaTime;
        if(bulletTime <= 0)
            OnHit();
    }

    private void OnHit(){
        gameObject.SetActive(false);
    }
}