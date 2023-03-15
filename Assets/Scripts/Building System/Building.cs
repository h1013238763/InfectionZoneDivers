using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private bool complete;
    public string buildingName;
    public Sprite buildingSprite;
    public string buildingDescribe;
    public int buildingID;
    public float buildTime;
    public BuildType buildingType;
    public int[] buildingRequireItem = new int[4];
    public int[] buildingRequireNum = new int[4];

    public enum BuildType{
        Structure,
        Defence,
        Storage,
        Manufactory,
        Power,
        Special
    }

    void Start(){
        complete = false;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().buildingAssign.Add(this.gameObject);
            collision.gameObject.GetComponent<PlayerAction>().InteractTip();
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().buildingAssign.Remove(this.gameObject);
            collision.gameObject.GetComponent<PlayerAction>().InteractTip();
        }
    }

    public void Interact(){
        if(complete){
            Debug.Log("interact");
        }
        else{
            Debug.Log("Incomplete");
        }
    }
}
