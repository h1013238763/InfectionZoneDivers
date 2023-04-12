using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]private List<Sprite> gateSprite;
    // Start is called before the first frame update

    void Start(){
        transform.GetComponent<Collider2D>().isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("In");
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player In");
            transform.GetComponent<SpriteRenderer>().sprite = gateSprite[0];
            transform.GetComponent<Collider2D>().isTrigger = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D other){
        Debug.Log("Out");
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player Out");
            transform.GetComponent<SpriteRenderer>().sprite = gateSprite[1];
            transform.GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
