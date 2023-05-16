using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextTipUI : MonoBehaviour
{
    public float hideTime = 5f;
    public float currTime = 0f;
    public float disTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currTime <= hideTime + disTime * 3){
            if(currTime <= disTime){
                gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, currTime);
            }
            else if(currTime < hideTime){
                gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
            }
            else if(currTime < hideTime+disTime){
                gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, hideTime+disTime-currTime);
            }
            else{
                gameObject.SetActive(false);
            }
            currTime += Time.deltaTime;
        }
    }
}
