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
    public GameObject pausePanel;
    public GameObject timePanel;
    public GameObject survivorPanel;
    [Space(10)]

    // Action Tip
    [SerializeField]private GameObject buildTip;
    [SerializeField]private GameObject interactTip;
    [SerializeField]private GameObject reloadTip;
    [Space(10)]

    // Inventory
    public GameObject playerInvent;
    public GameObject publicInvent;
    public GameObject quickInvent;
    [SerializeField]private GameObject inventSlot;
    [SerializeField]private GameObject itemDetail;
    public Invent currentPublicInvent;
    [Space(10)]

    // Crafting
    [SerializeField]private GameObject resourcePanel;
    [SerializeField]private GameObject benchPanel;
    [SerializeField]private GameObject benchSlot;
    [Space(10)]

    // Combat GUI
    [SerializeField]private GameObject[] WeaponIcon = new GameObject[2];
    [SerializeField]private GameObject quickSlot;
    [SerializeField]private GameObject ammoIcon;
    [SerializeField]private GameObject ammoText;
    [SerializeField]private GameObject ammoInventText;
    [SerializeField]private GameObject fireColdCover;
    private float fireCold;
    private float fireMax;
    [Space(10)]

    // Blueprint GUI
    public GameObject blueprintPanel;

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

        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // SetBuildTipPosition((int)mousePos.x, (int)mousePos.y);
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

        if(tag == "Chest" || tag == "Player" || tag == "Turret"){
            SetInventory(target.GetComponent<Invent>(), tag);
            EnterInventory(tag);
        }
        else if(tag == "Bench"){
            SetBench(target.GetComponent<Build_Bench>());
            EnterBench();
        }
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
        quickInvent.SetActive(false);
        benchPanel.SetActive(false);
        survivorPanel.SetActive(false);
        WorldController.controller.tempScale = 0;
        HideItemDetail();
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Enable();
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
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Disable();
    }

    /// <summary>
    /// Set slots of bench panel to interacting bench
    /// </summary>
    /// <param name="bench"> The bench interacting with</param>
    public void SetBench(Build_Bench bench){
        Debug.Log(bench.recipts.Count + " : " + benchPanel.transform.GetChild(0).childCount);
        // add slots
        while(benchPanel.transform.childCount < bench.recipts.Count){
            Instantiate(inventSlot, benchPanel.transform.GetChild(0));
        }

        // set each slot
        for(int i = 0; i < bench.recipts.Count; i ++){
            SlotUI_Bench tempSlot = benchPanel.transform.GetChild(0).GetChild(i).GetComponent<SlotUI_Bench>();
            tempSlot.Reset(bench.recipts[i], bench.nums[i], bench.recipts[i].itemSprite);
            tempSlot.gameObject.SetActive(true);
        }
        for(int i = bench.recipts.Count; i < benchPanel.transform.GetChild(0).childCount; i++){
            
            benchPanel.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }
    
    // Support Functions

    /// <summary>
    /// show and hide datail panel of item
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

        if(item is Weapon){
            Weapon weapon = (Weapon)item;
            Transform grid = itemDetail.transform.GetChild(4);
            grid.GetChild(0).GetComponent<Text>().text = weapon.weaponDamage.ToString();
            grid.GetChild(1).GetComponent<Text>().text = weapon.weaponAmmoCapa.ToString();
            grid.GetChild(2).GetComponent<Text>().text = weapon.weaponSpeed.ToString();
            grid.GetChild(3).GetComponent<Text>().text = weapon.weaponAccuracy.ToString();
            grid.GetChild(4).GetComponent<Text>().text = weapon.weaponRange.ToString();
            grid.GetChild(5).GetComponent<Text>().text = weapon.weaponReload.ToString();
            grid.GetChild(6).GetComponent<Text>().text = weapon.weaponAmmoIndex.ToString();
            grid.gameObject.SetActive(true);
        }
        if(item is Consumable){
            Consumable consume = (Consumable)item;
            itemDetail.transform.GetChild(5).GetComponent<Text>().text = "    " + consume.consumableDescribe;
            itemDetail.transform.GetChild(5).gameObject.SetActive(true);
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
        if(survivorPanel.activeSelf)
            GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Disable();
        else
            ExitPanels();
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
        for(int i = 0; i < 4; i ++){
            resourcePanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = ItemController.controller.resource[i].ToString();
        }
    }

    /// Building Mode
    public void EnterBlueprint(){
        SetBuildTipSprite(null);
        buildTip.SetActive(true);
    }

    public void ExitBlueprint(){
        buildTip.SetActive(false);
    }

    public void SetBuildTipColor(bool overlap){
        if(overlap)
            buildTip.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        else
            buildTip.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 0.5f);
    }
    
    public void SetBuildTipSprite(Sprite sprite){
        buildTip.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void SetBuildTipPosition(int x, int y){
        if(buildTip.activeSelf)
            buildTip.transform.position = new Vector2((float)x, (float)y);
    }
}
