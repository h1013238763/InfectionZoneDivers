using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Gate : MonoBehaviour
{
    [SerializeField]private List<Sprite> gateSprite;
    [SerializeField]private Collider2D coll;
    // Start is called before the first frame update

    void Start(){
        transform.GetComponent<Collider2D>().isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player In");
            transform.GetComponent<SpriteRenderer>().sprite = gateSprite[0];
            coll.isTrigger = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player Out");
            transform.GetComponent<SpriteRenderer>().sprite = gateSprite[1];
            coll.isTrigger = false;
        }
    }
}
