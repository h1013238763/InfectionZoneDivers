using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int buildID;
    public string buildName;
    [TextArea]
    public string buildDescribe;
    public bool buildInterAble;
    public string buildType;

    public bool buildComplete;
    public int[] buildRequireItem = new int[4];
    public int[] buildRequireNum = new int[4];
    public float buildTime;

    public int buildMaxHealth;
    public int buildCurrHealth;

    void Start(){
        OnDestory();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().buildingAssign.Add(gameObject);
        }
        if(collision.gameObject.tag == "GameController"){
            GUIController.controller.SetBuildTipColor(true);
            BuildController.controller.overlap = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            if(buildComplete)
                OnComplete();
            collision.gameObject.GetComponent<PlayerAction>().buildingAssign.Remove(gameObject);
        }
        if(collision.gameObject.tag == "GameController"){
            GUIController.controller.SetBuildTipColor(false);
            BuildController.controller.overlap = false;
        }
    }

    public void OnComplete(){
        transform.GetComponent<Collider2D>().isTrigger = false;
        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        buildCurrHealth = buildMaxHealth;
        for(int i = 0; i < transform.childCount; i ++){
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnDestory(){
        transform.GetComponent<Collider2D>().isTrigger = true;
        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        for(int i = 0; i < transform.childCount; i ++){
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Construct(){
        Debug.Log("Construct");
        buildComplete = true;
    }

    public void OnHit(int damage){
        buildCurrHealth -= damage;
        buildComplete = false;
        if(buildCurrHealth <= 0){
            OnDestory();
        }
    }
}
