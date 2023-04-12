using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float bulletTime;

    private void Start(){
        bulletTime = 0.2f;
    }

    private void FixedUpdate(){
        bulletTime -= Time.deltaTime;
        if(bulletTime <= 0)
            gameObject.SetActive(false);
    }
}
