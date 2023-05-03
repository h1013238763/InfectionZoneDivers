using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    // this
    public static PlayerAction player;

    /// Component setting
    private Rigidbody2D rigidBody;      // rigidbody component
    private PlayerInput playerInput;    // player input component
    public PlayerInputActions playerInputActions;       // player input script import
    public Invent invent;
    public CombatUnit combatUnit;
    public CameraAction cameraMain; 
    public GameObject healthBar;


    /// GUI System Setting
    [SerializeField]private GameObject buttonTipPrefab;

    // player state setting
    [SerializeField]private int playerActionStage;      // player current action stage
    public float moveSpeedDefault = 8f;
    private float moveSpeedCurrent = 8f;            // player move speed
    
    public List<GameObject> buildingAssign = new List<GameObject>();
    private float buildTime;
    
    /// Combat System Setting
    //  combat slots
    public ShortItem armorSlot;
    public Weapon[] weaponSlot = new Weapon[2];
    public int[] ammoSlot = new int[2];
    public ShortItem[] quickSlot = new ShortItem[4];
    // combat variables
    public int armorCurr;
    public int armorMax;
    // Special Action
    private float itemTime = -0.1f;
    private float itemEffect;
    private int itemSlot;

    private void Awake(){
        // initial static variable
        player = this;

        // initial variables
        rigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();

        // initial player Player actions
        playerInputActions.General.Enable();                        // enable Action Map "Player"
        playerInputActions.GUI.Enable();
        playerInputActions.Blueprint.Disable();

        // General
        playerInputActions.General.Reload.performed += Reload;      // reload
        playerInputActions.General.Change.performed += Change;      // change weapon
        playerInputActions.General.UseItem.performed += UseItem;
        playerInputActions.General.Build.performed += Build;

        // GUI
        playerInputActions.GUI.Invent.performed += Invent;          // open inventory
        playerInputActions.GUI.Interact.performed += Interact;      // interact
        playerInputActions.GUI.Pause.performed += Pause;
        
        // Blueprint
        playerInputActions.Blueprint.Place.performed += Place;
        playerInputActions.Blueprint.Pause.performed += Pause;
        playerInputActions.Blueprint.Build.performed += Build;
        playerInputActions.Blueprint.Rotate.performed += Rotate;
    }
    
    private void Start(){
        // bind components
        combatUnit = GetComponent<CombatUnit>();
        invent = GetComponent<Invent>();
        cameraMain = GameObject.Find("Main Camera").gameObject.GetComponent<CameraAction>();

        Restart();
    }

    private void FixedUpdate(){

        // player & weapon rotate
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float angle = (float)(Math.Atan2(( mousePos.y - transform.position.y ),( mousePos.x - transform.position.x )) * 180 / Math.PI);

        // Aim
        transform.GetChild(0).rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 180, -(angle+180));
        transform.rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

        // reset camera
        if(playerInputActions.General.Focus.ReadValue<float>() < 0.5f && cameraMain.cameraMode != 2){
            cameraMain.cameraMode = 0;
        }

        // Focus
        if(playerInputActions.General.Focus.ReadValue<float>() > 0.5f){
            moveSpeedCurrent = moveSpeedDefault / 4f;
            cameraMain.scope = weaponSlot[0].weaponScope;
            cameraMain.cameraMode = 1;
        }
        
        // Run
        else if(playerInputActions.General.Run.ReadValue<float>() > 0.5f){
            itemTime = -0.1f;
            moveSpeedCurrent = moveSpeedDefault * 1.5f;
            combatUnit.reloadTime = -0.1f;
            if(transform.childCount > 1){
                transform.GetChild(1).gameObject.GetComponent<ReloadTipUI>().SelfDestroy();
            }
        }
        // Use Armorpack
        else if(itemTime > 0 || combatUnit.reloadTime > 0){
            moveSpeedCurrent = moveSpeedDefault / 4f;
        }
        // Move   
        else{ 
            moveSpeedCurrent = moveSpeedDefault;
        }
        rigidBody.velocity = moveSpeedCurrent * playerInputActions.General.Move.ReadValue<Vector2>();

        // Fire
        if(playerInputActions.General.Fire.ReadValue<float>() > 0.5f && playerInputActions.General.Run.ReadValue<float>() < 0.5f){
            if(itemTime <= -0.1f)
                GetComponent<CombatUnit>().Fire(angle * Math.PI / 180, true);
        }

        if(itemTime > -0.1f){
            itemTime -= Time.deltaTime;
            if(itemTime <= 0){
                // recover armor
                ArmorChange((int)(armorMax * 0.01 * itemEffect));
                // remove 1 item
                quickSlot[itemSlot].itemNum --;
                // set ui
                GUIController.controller.SetQuickInvent();
                // reset action variable
                itemTime = -0.1f;
            }
        } 
    }

    public void Restart(){
        // Reset all states as game start
        combatUnit.SetWeapon(weaponSlot[0], ammoSlot[0]);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = weaponSlot[0].itemSprite;
        ArmorChange(0);
    }

    public void SetActionStage(int stage){
        switch(stage){
            case 0: // general
                playerInputActions.General.Enable();
                playerInputActions.GUI.Enable();
                playerInputActions.Blueprint.Disable();
                break;
            case 1: // in gui
                playerInputActions.General.Disable();
                playerInputActions.GUI.Enable();
                playerInputActions.Blueprint.Disable();
                break;
            case 2: // in blueprint
                playerInputActions.General.Disable();
                playerInputActions.GUI.Disable();
                playerInputActions.Blueprint.Enable();
                break;
            default:
                break;
        }
    }

    /// General Mode
    private void Reload(InputAction.CallbackContext context){
        if(itemTime <= -0.1f && ammoSlot[0] < weaponSlot[0].weaponAmmoCapa)
            GetComponent<CombatUnit>().Reload();
    }

    // use item
    private void UseItem(InputAction.CallbackContext context){
        int index = (int)(context.control.ToString()[context.control.ToString().Length-1])-49;
        if(quickSlot[index] != null){
            UseItem(index, (Consumable)ItemController.controller.ItemFind(quickSlot[index]));
        } 
    }
    public void UseItem(int index, Consumable item){
        if(playerInputActions.General.Run.ReadValue<float>() > 0.5f || GetComponent<CombatUnit>().reloadTime > 0){
            return;
        }
        switch(item.consumableType){
            case "Armorpack":
                itemTime = item.consumableData[1];
                itemEffect = item.consumableData[0];
                itemSlot = index;
                GUIController.controller.SetReloadTip(itemTime, gameObject, true);
                break;
            default:
                break;
        }
    }
    
    // change weapon
    private void Change(InputAction.CallbackContext context){

        // Availability Check
        if(weaponSlot[1] != null ){
            // Weapon Swap
            Weapon tempWeapon = weaponSlot[0];
            weaponSlot[0] = weaponSlot[1];
            weaponSlot[1] = tempWeapon;
            int tempAmmo = ammoSlot[0];
            ammoSlot[0] = ammoSlot[1];
            ammoSlot[1] = tempAmmo;
        }
        
        SetWeapon();
    }

    public void SetWeapon(){
        combatUnit.SetWeapon(weaponSlot[0], ammoSlot[0]);

        // Interrupt Reloading
        if(combatUnit.reloadTime > 0){
            combatUnit.reloadTime = -0.1f;
        }
        if(transform.childCount > 1){
            transform.GetChild(1).gameObject.GetComponent<ReloadTipUI>().SelfDestroy();
        }

        // Graphic Setting
        transform.GetChild(0).GetChild(0).position = weaponSlot[0].weaponMuzzle;
        GUIController.controller.SetGUI(invent, "Player");
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = weaponSlot[0].itemSprite;
    }

    // GUI Mode

    /// <summary>
    /// Player General.Interact action
    /// interact with building
    /// </summary>
    /// <param name="context"> return message from input system</param>
    private void Interact(InputAction.CallbackContext context){
        if(buildingAssign.Count > 0){
            if(buildingAssign[0].GetComponent<Building>().buildInterAble)
                switch (buildingAssign[0].GetComponent<Building>().buildType)
                {
                    case "Chest":
                        buildingAssign[0].GetComponent<Build_Chest>().Interact();
                        break;
                    case "Turret":
                        buildingAssign[0].transform.GetChild(0).GetComponent<Build_Turret>().Interact();
                        break;
                    case "Bench":
                        buildingAssign[0].GetComponent<Build_Bench>().Interact();
                        break;
                    case "Gather":
                        buildingAssign[0].GetComponent<Build_Gather>().Interact();
                        break;
                    case "Core":
                        buildingAssign[0].GetComponent<Build_Core>().Interact();
                        break;
                    default:
                        break;
                }
        }
    }

    private void Invent(InputAction.CallbackContext context){
        GUIController.controller.ActivePanel("Player", gameObject);
    }

    private void Pause(InputAction.CallbackContext context){
        if( GUIController.controller.playerInvent.activeSelf ||
            GUIController.controller.blueprintPanel.activeSelf ||
            GUIController.controller.survivorPanel.activeSelf){

            GUIController.controller.ExitPanels();
            return;
        }

        if(Time.timeScale != 0){
            Debug.Log("pause");
            GUIController.controller.pausePanel.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Debug.Log("continue");
        GUIController.controller.pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Build(InputAction.CallbackContext context){
        GUIController.controller.EnterBlueprintPanel();
    }

    // Blueprint Mode

    private void Place(InputAction.CallbackContext context){
        BuildController.controller.PlaceBuilding();
    }

    private void Rotate(InputAction.CallbackContext context){
        GUIController.controller.SetBuildTipRotation();
    }

    // Building Interact

    

    //  Print Interact Tip
    private void InteractTip(){
        if( buildingAssign.Count != 0 ){
            Vector3 buildingSize = buildingAssign[0].GetComponent<Renderer>().bounds.size;
            buttonTipPrefab.transform.position = buildingAssign[0].transform.position + new Vector3(buildingSize.x/2f, buildingSize.y+0.5f, 0);
            buttonTipPrefab.SetActive(true);
        }
        else{
            buttonTipPrefab.SetActive(false);
        }
    }

    public void ArmorChange(int num){
        armorCurr += num;
        if(armorCurr > armorMax)
            armorCurr = armorMax;
        if(armorCurr < 0)
            armorCurr = 0;
        healthBar.transform.localScale = new Vector2(1f * armorCurr / armorMax, 1f);
    }
}
