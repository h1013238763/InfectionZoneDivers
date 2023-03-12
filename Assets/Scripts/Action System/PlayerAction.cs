using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    // component setting
    private Rigidbody2D rigidBody;      // rigidbody component
    private PlayerInput playerInput;    // player input component
    private PlayerInputActions playerInputActions;       // player input script import
    private CombatUnit combatUnit;
    private DatabaseController itemDict;

    // GUI Setting

    // player state setting
    [SerializeField]private float moveSpeed;             // player move speed

    // weapon setting
    public ShortItem[] weaponSlot = new ShortItem[2];
    public ShortItem[] quickSlot = new ShortItem[4];
    private int currentWeapon = 0;


    private void Awake(){
        // initial variables
        rigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        combatUnit = GetComponent<CombatUnit>();

        // initial player Player actions
        playerInputActions.Player.Enable();                         // enable Action Map "Player"
        playerInputActions.Mouse.Enable();                          // enable Action Map "Mouse"
        playerInputActions.GUI.Enable();
        playerInputActions.Player.Interact.performed += Interact;   // interact
        playerInputActions.Player.Reload.performed += Reload;       // reload
        playerInputActions.Player.Change.performed += Change;       // change weapon

        playerInputActions.GUI.Bag.performed += Bag;

        combatUnit.TagInitial("Enemy", "Neutral");
    }

    private void FixedUpdate(){

        // player & weapon rotate
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float angle = (float)(Math.Atan2(( mousePos.y - transform.position.y ),( mousePos.x - transform.position.x )) * 180 / Math.PI);

        transform.rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        transform.GetChild(0).rotation = (angle < 90 && angle > -90) ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 180, -(angle+180));

        // player move
        rigidBody.velocity = moveSpeed * playerInputActions.Player.Move.ReadValue<Vector2>();

        // fire control
        if(playerInputActions.Mouse.Fire.ReadValue<float>() > 0.5f){
            combatUnit.Fire(angle * Math.PI / 180);
        }
    }

    private void Aim(InputAction.CallbackContext context){
        Debug.Log("aim");
    }

    // define how player interact works
    private void Interact(InputAction.CallbackContext context){
        Debug.Log("interact");
    }

    private void Reload(InputAction.CallbackContext context){
        combatUnit.Reload();
    }

    // change weapon
    private void Change(InputAction.CallbackContext context){
        if(currentWeapon == 0 && weaponSlot[1] != null)
            currentWeapon = 1;
        if(currentWeapon == 1 && weaponSlot[0] != null)
            currentWeapon = 0;
    }

    private void Bag(InputAction.CallbackContext context){
        playerInputActions.Mouse.Disable();
        GetComponent<Inventory>().OpenInventory();
    }


}
