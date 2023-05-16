using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GUIController : MonoBehaviour
{
    public static GUIController controller;

    // Overall
    public GameObject inGameStatus;
    public GameObject pausePanel;
    public GameObject survivorPanel;
    public GameObject textTip;
    [Space(10)]

    // Status
    public GameObject timePanel;
    public GameObject resourcePanel;
    [Space(10)]

    // Action Tip
    public GameObject buildTip;
    public GameObject interactTip;
    public GameObject reloadTip;
    public GameObject activeTurret;
    public GameObject currentTurret;
    [Space(10)]

    // Inventory
    public GameObject playerInvent;
    public GameObject publicInvent;
    public GameObject quickInvent;
    public GameObject itemDetail;
    public Invent currentPublicInvent;
    [Space(10)]

    // Crafting
    public GameObject benchPanel;
    [Space(10)]

    // Combat GUI
    public GameObject[] WeaponIcon = new GameObject[2];
    public GameObject quickSlot;
    public GameObject ammoIcon;
    public GameObject ammoText;
    public GameObject ammoInventText;
    public GameObject fireColdCover;
    private float fireCold;
    private float fireMax;
    [Space(10)]

    // Blueprint GUI
    public GameObject blueprintPanel;
    public GameObject buildSlotGrid;
    public GameObject buildSlot;
    public int rotate = 0;

    // Start is called before the first frame update
    void Start(){
        controller = this;
        SetGUI(PlayerAction.player.invent, "Player");
        SetResourcePanel();
        InitialPanels();
    }

    // Update is called once per frame
    void Update(){

        if(fireCold > 0){
            fireCold -= Time.deltaTime;
            SetFireColdTip( fireCold / fireMax );
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        SetBuildTipPosition((int)mousePos.x, (int)mousePos.y);
    }

    public void SetGUI(Invent invent, string tag){
        SetInventory(invent, tag);
        SetAmmoInventText();
        SetAmmoIcon();
        SetQuickInvent();
        SetAmmoText();
    }

    // panel controller

    /// <summary>
    /// active different GUI panels 
    /// </summary>
    /// <param name="tag"> active which panel</param>
    public void ActivePanel(string tag, GameObject target){
        if(playerInvent.activeSelf){
            ExitPanels();
            return;
        }else if(tag == "Core"){
            SetSurvivorPanel();
            EnterSurvivorPanel();
        }
        else{
            SetInventory(GameObject.Find("Player").GetComponent<Invent>(), "Player");
            EnterInventory("Player");
        }

        if(tag == "Chest" || tag == "Player"){
            SetInventory(target.GetComponent<Invent>(), tag);
            EnterInventory(tag);
        }
        else if(tag == "Turret"){
            SetInventory(target.GetComponent<Invent>(), tag);
            EnterInventory(tag);

            if(currentTurret.GetComponent<Build_Turret>().survivorActive){
                activeTurret.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Deactivate";
            }else{
                activeTurret.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Activate";
            }
            
            activeTurret.SetActive(true);
        }
        else if(tag == "Bench"){
            SetBench(target.GetComponent<Build_Bench>());
            EnterBench();
        }

        GameObject.Find("Player").GetComponent<PlayerAction>().SetActionStage(1);
    }

    /// <summary>
    /// Initial Panels
    /// </summary>
    private void InitialPanels(){
        playerInvent.SetActive(false);
        publicInvent.SetActive(false);
        benchPanel.SetActive(false);
    } 

    /// <summary>
    /// close panels
    /// </summary>
    public void ExitPanels(){
        playerInvent.SetActive(false);
        publicInvent.SetActive(false);
        activeTurret.SetActive(false);
        quickInvent.SetActive(false);
        benchPanel.SetActive(false);
        survivorPanel.SetActive(false);
        WorldController.controller.tempDifficulty = 0;
        ExitBlueprintPanel();
        HideItemDetail();
        GameObject.Find("Player").GetComponent<PlayerAction>().SetActionStage(0);
    }

    // Inventory Methods

    /// <summary>
    /// open inventory
    /// </summary>
    /// <param name="tag"> open which inventory</param>
    public void EnterInventory(string tag){
        if( tag != "Player")
            publicInvent.SetActive(true);
        else
            quickInvent.SetActive(true);
        playerInvent.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Disable();
    }

    /// <summary>
    /// reset all ui of inventory panel
    /// </summary>
    /// <param name="invent"> the invent data </param>
    /// <param name="tag"> set which inventory </param>
    public void SetInventory(Invent invent, string tag){
        // which invent gui need to be set
        Transform tempInvent;
        if(tag == "Player")
            tempInvent = playerInvent.transform.GetChild(0);
        else{
            tempInvent = publicInvent.transform.GetChild(0);
            currentPublicInvent = invent;
        }
        // set slot active
        for(int i = 0; i < invent.inventCap; i ++){
            SlotUI_Invent tempSlot = tempInvent.GetChild(i).GetComponent<SlotUI_Invent>();
            if(invent.inventList[i] == null){
                tempSlot.Hide();
            }else{
                tempSlot.Reset(invent.inventList[i], ItemController.controller.ItemFind(invent.inventList[i]).itemSprite);
            }
            tempSlot.gameObject.SetActive(true);
        }
        for(int i = invent.inventCap; i < tempInvent.transform.childCount; i++){
            tempInvent.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Bench Methods

    /// <summary>
    /// open Bench Panel
    /// </summary>
    public void EnterBench(){
        benchPanel.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerAction>().SetActionStage(1);
    }

    /// <summary>
    /// Set slots of bench panel to interacting bench
    /// </summary>
    /// <param name="bench"> The bench interacting with</param>
    public void SetBench(Build_Bench bench){
        // set grid size
        benchPanel.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(400, bench.recipes.Count * 125);
        // set each slot
        for(int i = 0; i < bench.recipes.Count; i ++){
            SlotUI_Bench tempSlot = benchPanel.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<SlotUI_Bench>();
            tempSlot.Reset(bench.recipes[i], bench.nums[i], bench.recipes[i].itemSprite);
            tempSlot.gameObject.SetActive(true);
        }
        for(int i = bench.recipes.Count; i < benchPanel.transform.GetChild(0).GetChild(0).childCount; i++){
            
            benchPanel.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }
    
    // Support Functions

    /// <summary>
    /// show and hide datail panel of item and buildings
    /// </summary>
    /// <param name="si"> show which item </param>
    public void ShowItemDetail(int id){
        Item item = ItemController.controller.database.itemDict[id];

        itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        //itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
        itemDetail.transform.GetChild(1).GetComponent<Text>().text = item.itemName;
        itemDetail.transform.GetChild(2).GetComponent<Text>().text = "    " + item.itemDescribe;
        itemDetail.transform.GetChild(3).GetComponent<Text>().text = " [ " + item.GetType().ToString() + " ]";
        itemDetail.transform.GetChild(4).gameObject.SetActive(false);
        itemDetail.transform.GetChild(5).gameObject.SetActive(false);
        itemDetail.transform.GetChild(6).gameObject.SetActive(false);

        if(item is Weapon){
            Weapon weapon = (Weapon)item;
            Transform grid = itemDetail.transform.GetChild(4);
            grid.GetChild(0).GetComponent<Text>().text = weapon.weaponDamage.ToString();
            grid.GetChild(1).GetComponent<Text>().text = weapon.weaponAmmoCapa.ToString();
            grid.GetChild(2).GetComponent<Text>().text = weapon.weaponSpeed.ToString();
            grid.GetChild(3).GetComponent<Text>().text = weapon.weaponAccuracy.ToString();
            grid.GetChild(4).GetComponent<Text>().text = weapon.weaponRange.ToString();
            grid.GetChild(5).GetComponent<Text>().text = weapon.weaponReload.ToString();
            grid.GetChild(6).GetComponent<Text>().text = ItemController.controller.database.itemDict[weapon.weaponAmmoIndex].itemName;
            grid.gameObject.SetActive(true);
            itemDetail.transform.GetChild(6).gameObject.SetActive(true);
        }
        if(item is Consumable){
            Consumable consume = (Consumable)item;
            itemDetail.transform.GetChild(5).GetComponent<Text>().text = "    " + consume.consumableDescribe;
            itemDetail.transform.GetChild(5).gameObject.SetActive(true);
        }    
        itemDetail.SetActive(true);
    }
    public void ShowBuildDetail(int id){
        GameObject build = BuildController.controller.currBuildList[id];
        Debug.Log(build);
        if(build.GetComponent<Building>().buildType == "Turret")
            itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = build.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        else
            itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = build.GetComponent<SpriteRenderer>().sprite;
        //itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
        itemDetail.transform.GetChild(1).GetComponent<Text>().text = build.GetComponent<Building>().buildName;
        itemDetail.transform.GetChild(2).GetComponent<Text>().text = "    " + build.GetComponent<Building>().buildDescribe;
        itemDetail.transform.GetChild(3).GetComponent<Text>().text = " [ " + build.GetComponent<Building>().buildType + " ]";
        itemDetail.transform.GetChild(4).gameObject.SetActive(false);
        itemDetail.transform.GetChild(5).gameObject.SetActive(false);
        itemDetail.transform.GetChild(6).gameObject.SetActive(false);
        // turret status
        if(build.GetComponent<Building>().buildType == "Turret"){

            Transform grid = itemDetail.transform.GetChild(4);
            Weapon weapon = build.GetComponent<Build_Turret>().weapon;
            grid.GetChild(0).GetComponent<Text>().text = weapon.weaponDamage.ToString();
            grid.GetChild(1).GetComponent<Text>().text = weapon.weaponAmmoCapa.ToString();
            grid.GetChild(2).GetComponent<Text>().text = weapon.weaponSpeed.ToString();
            grid.GetChild(3).GetComponent<Text>().text = weapon.weaponAccuracy.ToString();
            grid.GetChild(4).GetComponent<Text>().text = weapon.weaponRange.ToString();
            grid.GetChild(5).GetComponent<Text>().text = weapon.weaponReload.ToString();
            grid.GetChild(6).GetComponent<Text>().text = ItemController.controller.database.itemDict[weapon.weaponAmmoIndex].itemName;
            grid.gameObject.SetActive(true);
            itemDetail.transform.GetChild(6).gameObject.SetActive(true);
        }
        itemDetail.SetActive(true);
    }
    public void HideItemDetail(){
        itemDetail.SetActive(false);
    }

    // Time Panel Setting

    public void SetTimePanel(int day){
        Debug.Log("Set");
        timePanel.transform.GetChild(0).GetComponent<Text>().text = "Day " + day.ToString();
    }

    // Survivor Panel

    public void SetSurvivorPanel(){
        // set retreat button
        GameObject retreat = survivorPanel.transform.GetChild(1).GetChild(2).gameObject;
        int day = WorldController.controller.dayNum;
        retreat.GetComponent<Button>().interactable = ((day - 7) % 3 == 0 && day > 6);
    }
    public void EnterSurvivorPanel(){
        survivorPanel.SetActive( !survivorPanel.activeSelf );

        if(survivorPanel.activeSelf){
            GameObject.Find("Player").GetComponent<PlayerAction>().SetActionStage(1);
        }      
        else{
            ExitPanels();
        }      
    }

    // Combat GUI Setting

    /// <summary>
    /// Set quick and weapon slot ui
    /// </summary>
    public void SetQuickInvent(){
        Sprite temp;
        // weapon slots
        for(int i = 0; i < 2; i ++)
        {
            if(PlayerAction.player.weaponSlot[i] != null){
                temp = PlayerAction.player.weaponSlot[i].itemSprite;
                quickInvent.transform.GetChild(i).GetComponent<SlotUI_Quick>().Reset(temp, i, PlayerAction.player.weaponSlot[i].itemID, 1);
                WeaponIcon[i].transform.GetChild(0).GetComponent<Image>().sprite = temp;
                WeaponIcon[i].transform.GetChild(0).gameObject.SetActive(true);
            }else{
                quickInvent.transform.GetChild(i).GetComponent<SlotUI_Quick>().Hide();
                WeaponIcon[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        // item slots
        for(int i = 0; i < 4; i ++){
            if(PlayerAction.player.quickSlot[i] != null){
                temp = ItemController.controller.ItemFind(PlayerAction.player.quickSlot[i]).itemSprite;
                quickInvent.transform.GetChild(i+2).GetComponent<SlotUI_Quick>().Reset(temp, i+2, PlayerAction.player.quickSlot[i].itemID, PlayerAction.player.quickSlot[i].itemNum);
                quickSlot.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = temp;
                quickSlot.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = PlayerAction.player.quickSlot[i].itemNum.ToString();
                quickSlot.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                quickSlot.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }else{
                quickInvent.transform.GetChild(i+2).GetComponent<SlotUI_Quick>().Hide();
                quickSlot.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                quickSlot.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set text of current ammo number 
    /// </summary>
    /// <param name="ammo"> the number of ammo </param>
    /// <param name="capa"> the capacity of ammo </param>
    public void SetAmmoText(){
        ammoText.GetComponent<Text>().text = PlayerAction.player.ammoSlot[0].ToString() + " / " + PlayerAction.player.weaponSlot[0].weaponAmmoCapa.ToString();
    }

    /// <summary>
    /// Set ammo icon to current weapon's ammo icon
    /// </summary>
    public void SetAmmoIcon(){
        ammoIcon.SetActive(true);
        PlayerAction player = GameObject.Find("Player").GetComponent<PlayerAction>();
        if(player.weaponSlot[0] != null)
            ammoIcon.GetComponent<Image>().sprite = ItemController.controller.database.itemDict[player.weaponSlot[0].weaponAmmoIndex].itemSprite;
        else
            ammoIcon.SetActive(false);
    }

    /// <summary>
    /// Set text of how many ammo in inventory
    /// </summary>
    public void SetAmmoInventText(){
        int currentAmmoID = PlayerAction.player.weaponSlot[0].weaponAmmoIndex;
        ammoInventText.GetComponent<Text>().text = ItemController.controller.ItemNumber(currentAmmoID, GameObject.Find("Player").GetComponent<Invent>()).ToString();
    }

    /// <summary>
    /// Set the reload ui on reloading object
    /// </summary>
    /// <param name="time"> how long the object reload </param>
    /// <param name="from"> which object need to reload </param>
    /// <param name="player"> is player reload? </param>
    public void SetReloadTip(float time, GameObject from, bool player){
        GameObject temp = Instantiate(reloadTip);
        temp.GetComponent<ReloadTipUI>().timeMax = time;
        temp.transform.SetParent(from.transform);
        if(player)
            temp.transform.position = new Vector3(from.transform.position.x, from.transform.position.y+1.5f, 0f);
        else
            temp.transform.position = new Vector3(from.transform.position.x+from.GetComponent<Collider>().bounds.size.x/2, from.transform.position.y+1f, 0f);
        temp.SetActive(true);
    }

    /// <summary>
    /// Set the fire colddown tip ui
    /// </summary>
    /// <param name="time"></param>
    /// <param name="timeMax"></param>
    public void SetFireColdTip(float time, float timeMax){
        fireCold = time;
        fireMax = timeMax;

    }
    public void SetFireColdTip(float length){
        if(length <= 0)
            length = 0;
        fireColdCover.transform.localScale = new Vector3(1, length, 0);
    }

    /// <summary>
    /// Set top-right resource panel
    /// </summary>
    public void SetResourcePanel(){
        Transform resourceGrid = resourcePanel.transform.GetChild(1);
        for(int i = 0; i < 4; i ++){
            resourceGrid.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = WorldController.controller.resource[i].ToString();
        }
    }

    /// Building Mode
    public void EnterBlueprintPanel(){
        if(blueprintPanel.activeSelf){
            ExitBlueprintPanel();
            return;
        }
        // hide other gui
        inGameStatus.SetActive(false);

        // active blueprintPanel
        blueprintPanel.SetActive(true);

        // disable player action
        GameObject.Find("Player").GetComponent<PlayerAction>().SetActionStage(2);
        GameObject.Find("Main Camera").GetComponent<CameraAction>().cameraMode = 2;
    }

    public void ExitBlueprintPanel(){
        // active status gui
        inGameStatus.SetActive(true);

        buildTip.SetActive(false);
        blueprintPanel.SetActive(false);
        buildSlotGrid.SetActive(false);

        BuildController.controller.currentBuilding = null;
        BuildController.controller.deleteMode = false;
        
        GameObject.Find("Player").GetComponent<PlayerAction>().SetActionStage(0);
        GameObject.Find("Main Camera").GetComponent<CameraAction>().cameraMode = 0;
    }

    public void SetBlueprintPanel(){
        List<GameObject> buildList = BuildController.controller.currBuildList;

        buildSlotGrid.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(400, buildList.Count * 125);

        // set each slot
        if(buildSlotGrid.transform.GetChild(0).GetChild(0).childCount < buildList.Count){
            for(int i = buildSlotGrid.transform.GetChild(0).GetChild(0).childCount; i < buildList.Count; i ++){
                GameObject temp = Instantiate(buildSlot);
                temp.transform.SetParent(buildSlotGrid.transform.GetChild(0).GetChild(0).transform);
                temp.SetActive(false);
            }
        }
        for(int i = 0; i < buildList.Count; i ++){
            SlotUI_Build tempSlot = buildSlotGrid.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<SlotUI_Build>();
            tempSlot.Reset(i, buildList[i].GetComponent<SpriteRenderer>().sprite);
            tempSlot.gameObject.SetActive(true);
        }
        for(int i = buildList.Count; i < buildSlotGrid.transform.GetChild(0).GetChild(0).childCount; i++){
            buildSlotGrid.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
        }

        buildSlotGrid.SetActive(true);
    }

    public void SetBuildTipColor(bool overlap){
        if(overlap)
            buildTip.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        else
            buildTip.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 0.5f);
    }
    
    public void SetBuildTipSprite(GameObject build){
        
        buildTip.GetComponent<SpriteRenderer>().sprite = build.transform.GetComponent<SpriteRenderer>().sprite;
        buildTip.transform.rotation = Quaternion.Euler(0, 0, 0);
        buildTip.GetComponent<BoxCollider2D>().size = build.transform.GetComponent<Building>().buildSize - new Vector2(0.2f, 0.2f);
        buildTip.GetComponent<BoxCollider2D>().offset = build.transform.GetComponent<Building>().buildSize / 2;

        buildTip.SetActive(true);
    }
    public void SetBuildTipSprite(int degree){
        buildTip.transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    public void SetBuildTipRotation(){
        Building tempBuild = BuildController.controller.currentBuilding.GetComponent<Building>();
        rotate += 90;
        if(rotate >= 360)
            rotate -= 360;

        if(tempBuild.buildType == "Gate" || tempBuild.buildType == "Wall" || tempBuild.buildType == "Turret" || tempBuild.buildType == "Gather")
            rotate = 0;

        SetBuildTipSprite(rotate);
    }

    private void SetBuildTipPosition(int x, int y){
        float xp = 0f;
        float yp = 0f;
        if(buildTip.activeSelf){
            if(rotate >= 90 && rotate < 270)
                xp = 1f;
            if(rotate >= 180)
                yp = 1f;
        }
        buildTip.transform.position = new Vector2((float)x + xp, (float)y + yp);
    }

    public void SetTextTip(string text){
        textTip.GetComponent<Text>().text = text;
        textTip.SetActive(false);
        textTip.SetActive(true);
        textTip.GetComponent<TextTipUI>().currTime = 0f;
    }

    public void SetTurret(){
        currentTurret.GetComponent<Build_Turret>().Active();
    }

    public void Reset(){
        SetGUI(PlayerAction.player.invent, "Player");
        SetResourcePanel();
        ExitPanels();
    }
}
