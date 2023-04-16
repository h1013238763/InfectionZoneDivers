using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    /// Component setting
    private Rigidbody2D rigidBody;      // rigidbody component
    private PlayerInput playerInput;    // player input component
    public PlayerInputActions playerInputActions;       // player input script import

    /// GUI System Setting
    [SerializeField]private GameObject buttonTipPrefab;

    // player state setting
    [SerializeField]private int playerActionStage;      // player current action stage
    private float moveSpeed = 8f;            // player move speed
    
    public List<GameObject> buildingAssign = new List<GameObject>();
    private float buildTime;
    
    /// Combat System Setting
    //  combat slots
    public ShortItem armorSlot;
    public Weapon[] weaponSlot = new Weapon[2];
    public int[] ammoSlot = new int[2];
    public ShortItem[] quickSlot = new ShortItem[4];
    private int currentWeapon = 0;
    // combat variables
    private int health;

    private void Awake(){
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
         
        playerInputActions.GUI.Invent.performed += Invent;          // open inventory
        playerInputActions.GUI.Interact.performed += Interact;      // interact

        playerInputActions.Blueprint.Place.performed += Place;
    }
    
    private void Start(){
        weaponSlot[0] = (Weapon)ItemController.controller.database.itemDict[1];
        currentWeapon = 0;
    }

    private void FixedUpdate(){

        // player & weapon rotate
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float angle = (float)(Math.Atan2(( mousePos.y - transform.position.y ),( mousePos.x - transform.position.x )) * 180 / Math.PI);

        // Aim
        transform.GetChild(0).rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 180, -(angle+180));
        transform.rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

        // Run
        if(playerInputActions.General.Run.ReadValue<float>() > 0.5f){
            rigidBody.velocity = moveSpeed * 1.5f * playerInputActions.General.Move.ReadValue<Vector2>();
        }   
        // Focus
        else if(playerInputActions.General.Focus.ReadValue<float>() > 0.5f){
            rigidBody.velocity = moveSpeed / 3f * playerInputActions.General.Move.ReadValue<Vector2>();

        }
        // Move   
        else{ 
            rigidBody.velocity = moveSpeed * playerInputActions.General.Move.ReadValue<Vector2>();
        }
        // Fire
        if(playerInputActions.General.Fire.ReadValue<float>() > 0.5f && playerInputActions.General.Run.ReadValue<float>() < 0.5f){
            GetComponent<CombatUnit>().Fire(angle * Math.PI / 180);
        }
        // Construct
        if(playerInputActions.General.Construct.ReadValue<float>() > 0.5f){
            if(buildingAssign[0] != null){
                Building temp = buildingAssign[0].GetComponent<Building>();
                if(!temp.buildComplete && true){
                    buildTime += Time.deltaTime;
                    if(temp.buildTime <= buildTime){
                        temp.Construct();
                    }
                }
            }
        }

    }
    
    /// Mode Setting
    private void Fire(double radius){
        
    }

    private void ChangeActionStage(int stage){
        playerActionStage = stage;
        if(playerActionStage == 0){         // normal walking stage
            playerInputActions.General.Enable();
            playerInputActions.GUI.Enable();
            playerInputActions.Blueprint.Disable();
        }
        if(playerActionStage == 1){         // gui open stage
            playerInputActions.General.Disable();
            playerInputActions.GUI.Enable();
            playerInputActions.Blueprint.Disable();
        }
        if(playerActionStage == 2){
            playerInputActions.General.Disable();
            playerInputActions.GUI.Disable();
            playerInputActions.Blueprint.Enable();
        }
    }

    /// General Mode
    private void Reload(InputAction.CallbackContext context){
        GetComponent<CombatUnit>().Reload();
    }

    
    private void Interact(InputAction.CallbackContext context){
        if(buildingAssign.Count > 0){
            if(buildingAssign[0].GetComponent<Building>().buildInterAble)
                switch (buildingAssign[0].GetComponent<Building>().buildType)
                {
                    case "Chest":
                        buildingAssign[0].GetComponent<Chest>().Interact();
                        break;
                    default:
                        break;
                }
        }
    }

    /// GUI Mode
    


    /// Blueprint Mode

    /// Building Interact

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

    // change weapon
    private void Change(InputAction.CallbackContext context){
        if(currentWeapon == 0 && weaponSlot[1] != null)
            currentWeapon = 1;
        if(currentWeapon == 1 && weaponSlot[0] != null)
            currentWeapon = 0;
        GUIController.controller.currentAmmoID = weaponSlot[currentWeapon].weaponAmmoIndex;
        GUIController.controller.SetGUI(GetComponent<Invent>(), gameObject.tag);
        //transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = database.itemDict[weaponSlot[currentWeapon].itemID].itemSprite;
    }

    

    // open player bag
    private void Invent(InputAction.CallbackContext context){
        GUIController.controller.ActiveInventory("Player");
        GUIController.controller.SetInventory(GetComponent<Invent>(), "Player");
    }

    private void Place(InputAction.CallbackContext context){
        BuildController.controller.PlaceBuilding();
    }

}
