using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFire : MonoBehaviour
{
    public float timeMax;
    private float time;

    void FixedUpdate(){
        
        if(time > 0 )
            time -= Time.deltaTime;
        
        if(time <= 0 && time > -0.1f){
            transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
            time = -0.1f;
        }
    }

    public void Fire(){
        time = timeMax;
        transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
