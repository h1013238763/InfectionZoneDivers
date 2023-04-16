using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public void Interact(){
        Debug.Log("Chest Interact");
        GUIController.controller.SetInventory(GetComponent<Invent>(), "Chest");
        GUIController.controller.ActiveInventory("Chest");
    }
}
