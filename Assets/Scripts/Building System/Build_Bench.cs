using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Bench : MonoBehaviour
{
    public List<Item> recipts;
    public List<int> nums;

    public void Interact(){
        GUIController.controller.ActivePanel("Bench", gameObject);
    }
}
