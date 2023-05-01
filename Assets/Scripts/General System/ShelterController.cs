using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShelterController : MonoBehaviour
{
    public static ShelterController controller;
    public int[] resource = new int[4];
    public int survivor;
    
    public GameObject shelterResourcePanel;
    public GameObject shelterUpgradeDescribe;

    public GameObject shelterGUI;
    public GameObject loadingGUI;

    public List<Upgrade> upgrades = new List<Upgrade>();
    public Upgrade currUpgrade;

    private const string SAVEDATA_NAME = "save.data";
    void Start(){

        controller = this;

    }

    public void ActiveUpgrade(){
        if(survivor < currUpgrade.survivor){
            Debug.Log(" Shelter Lack of Survivors");
            return;
        }
        for(int i = 0; i < 4; i ++){
            if(resource[i] < currUpgrade.resource[i]){
                Debug.Log("Shelter Lack of Resource");
                return;
            }
        }
        foreach(Upgrade i in currUpgrade.upgradePre ){
            if(!upgrades.Contains(i)){
                Debug.Log("Requires Prereqyusute Research");
                return;
            }
        }
        if(upgrades.Contains(currUpgrade)){
            Debug.Log("Research Has Been Done");
            return;
        }
        for(int i = 0; i < 4; i ++){
            resource[i] -= currUpgrade.resource[i];
        }
        currUpgrade.Active();
        upgrades.Add(currUpgrade);
    }

    public void GameStart(){

        shelterGUI.SetActive(false);

        foreach(Upgrade i in upgrades){
            i.Effect();
        }

        loadingGUI.SetActive(false);
    }

    public void SetResourcePanel(){
        for(int i = 0; i < 4; i ++){
            shelterResourcePanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = resource[i].ToString();
        }
        shelterResourcePanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = survivor.ToString();
    }

    public void SaveGame(){
        // create path and file
        if(!Directory.Exists(SAVEDATA_NAME))
            Directory.CreateDirectory(SAVEDATA_NAME);
        StreamWriter sw = new StreamWriter(SAVEDATA_NAME);
        sw.Write("yes");
    }

    public void LoadGame(){
        
    }

    public void CreateMap(){
        
    }
}
