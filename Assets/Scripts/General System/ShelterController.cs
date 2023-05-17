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

    // Complete upgrade list
    public List<Upgrade> upgrades = new List<Upgrade>();
    
    public Upgrade currUpgrade;

    public GameObject mainPanel;
    public GameObject tutorialPanel;

    private const string SAVEDATA_NAME = "save.data";

    void Start(){

        controller = this;

        ActiveShelterGUI();
    }

    public void ActiveUpgrade(){
        if(currUpgrade == null){
            return;
        }
        if(survivor < currUpgrade.survivor){
            GUIController.controller.SetTextTip(" Shelter Lack of Survivors");
            return;
        }
        for(int i = 0; i < 4; i ++){
            if(resource[i] < currUpgrade.resource[i]){
                GUIController.controller.SetTextTip("Shelter Lack of Resource");
                return;
            }
        }
        foreach(Upgrade pre in currUpgrade.upgradePre ){
            bool has = false;
            for(int i = 0; i < upgrades.Count; i ++){
                if(pre.upgradeID == upgrades[i].upgradeID){
                    has = true;
                }
            }
            if(!has){
                GUIController.controller.SetTextTip("Requires Prerequisite Research");
                return;
            }
        }
        foreach(Upgrade up in upgrades){
            if(up.upgradeID == currUpgrade.upgradeID){
                GUIController.controller.SetTextTip("Research Has Been Done");
                return;
            }
        }
        for(int i = 0; i < 4; i ++){
            resource[i] -= currUpgrade.resource[i];
        }
        currUpgrade.Active();
        upgrades.Add(currUpgrade);
        SetResourcePanel();
        currUpgrade = null;
    }

    public void GameStart(){

        shelterGUI.SetActive(false);

        ResetGame();

        for(int i = 0; i < upgrades.Count; i ++){
            upgrades[i].Effect();
        }

        PlayerAction.player.Restart();

        loadingGUI.SetActive(false);
    }

    public void ActiveShelterGUI(){
        SetResourcePanel();
        shelterGUI.SetActive(true);
        loadingGUI.SetActive(true);
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

    public void ResetGame(){
        GUIController.controller.Reset();
        PlayerAction.player.Reset();
        EnemyController.controller.Reset();
        BuildController.controller.Reset();
        WorldController.controller.Reset();
        ItemController.controller.Reset();
        CombatController.controller.Reset();
    }

    public void QuitToMainMenu(){
        mainPanel.SetActive(true);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
