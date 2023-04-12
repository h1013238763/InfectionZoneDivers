using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadTipUI : MonoBehaviour
{
    public float time;
    public float timeMax;
    // Update is called once per frame
    void Start(){
        time = timeMax;
    }

    void FixedUpdate()
    {
        time -= Time.deltaTime;

        transform.localScale = new Vector3(time / timeMax, 0.1f, 0);

        if(time <= 0)
            Destroy(gameObject);
    }
}

