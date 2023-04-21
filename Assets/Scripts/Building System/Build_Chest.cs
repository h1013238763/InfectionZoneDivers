using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Chest : MonoBehaviour
{
    public void Interact(){
        GUIController.controller.ActivePanel("Chest", gameObject);
    }
}
