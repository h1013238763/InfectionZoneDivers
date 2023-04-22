using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Core : MonoBehaviour
{

    public void Interact(){
        GUIController.controller.ActivePanel("Core", gameObject);
    }
}
