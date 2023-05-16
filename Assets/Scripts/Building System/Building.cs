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
    public Vector2 buildSize;

    public int[] buildRequire = new int[4];

    public int buildMaxHealth;

    void Start(){
        GetComponent<Health>().health = buildMaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().AddToBuildList(gameObject);
        }
        if(collision.gameObject.tag == "GameController"){
            if(buildType == "Core"){
                return;
            }
            else if(BuildController.controller.deleteMode){
                GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 1f);
                BuildController.controller.deleteBuild = gameObject;
            }
            else{
                BuildController.controller.AddOverlapList(gameObject);
            } 
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<PlayerAction>().RemoveFromBuildList(gameObject);
        }
        if(collision.gameObject.tag == "GameController"){
            if(buildType == "Core"){
                return;
            }
            else if(BuildController.controller.deleteMode){
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                BuildController.controller.deleteBuild = null;
            }
            else{
                BuildController.controller.RemoveOverlapList(gameObject);
            }
        }
    }

    public void OnDestroy(){
        if(GetComponent<Invent>() != null){
            foreach(ShortItem item in GetComponent<Invent>().inventList){
                if(item != null){
                    GUIController.controller.SetTextTip("Storage buildings containing items cannot be demolished");
                    return;
                }
            }
            
        }

        WorldController.controller.ResourceGet(buildRequire);

        if(buildType == "Gather"){
            if(GetComponent<Build_Gather>().survivorActive)
                WorldController.controller.UseSurvivor(GetComponent<Build_Gather>().survivorRequire);
        }else if(buildType == "Turret"){
            Build_Turret turret = GetComponent<Build_Turret>();
            if(turret.survivorActive){
                WorldController.controller.UseSurvivor(turret.survivorRequire);
            }
        }
        BuildController.controller.buildInWorld.Remove(gameObject);
        Destroy(gameObject);
    }

    public void ForceDestroy(){
        BuildController.controller.buildInWorld.Remove(gameObject);
        Destroy(gameObject);
    }
}
