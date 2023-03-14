using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private bool complete;
    private 

    private void OnTriggerStay2D(Collider2D collision){
        int show = (complete) ? 0 : 1;
        transform.GetChild(show).gameObject.SetActive(true);
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().buildingAssign = this.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        int show = (complete) ? 0 : 1;
        transform.GetChild(show).gameObject.SetActive(false);
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().buildingAssign = null;
        }
    }

    public void Interact(){
        Debug.Log("interact");
    }
}
