using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public void Interact(){
        GUIController.controller.SetInventory(gameObject.GetComponent<Invent>(), "Chest");
        GUIController.controller.ActiveInventory("Chest");
    }
}
