using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int[] resource = new int[4];
    public int survivor;

    public int upgradeID;
    public string upgradeName;
    public Sprite upgradeSprite;
    [TextArea]
    public string upgradeDescribe;
    public Upgrade[] upgradePre;

    void Start(){
        SetGUI();
    }

    public void SetGUI(){
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = upgradeSprite;
        transform.GetChild(1).GetComponent<Text>().text = upgradeName;
        transform.GetChild(2).GetComponent<Text>().text = "s: " + survivor;
        for(int i = 0; i < 4; i ++){
            transform.GetChild(4).GetChild(i).GetComponent<Text>().text = resource[i].ToString();
        }
    }

    public void PrintDescribe(){
        ShelterController.controller.shelterUpgradeDescribe.GetComponent<Text>().text = "[ " + upgradeName + " ]\n" + upgradeDescribe;
        ShelterController.controller.currUpgrade = this;
    }

    public void Active(){
        
        transform.GetChild(2).GetComponent<Text>().text = "0";

        for(int i = 0; i < 4; i ++){
            transform.GetChild(4).GetChild(i).GetComponent<Text>().text = "0";
        }
    }

    public virtual void Effect(){

    }
}
