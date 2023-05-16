using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Bench : MonoBehaviour
{
    public List<Item> recipes;
    public List<int> nums;

    public void Interact(){
        GUIController.controller.ActivePanel("Bench", gameObject);
    }

    public void AddFormula(Item item, int num){
        if(!recipes.Contains(item)){
            recipes.Add(item);
            nums.Add(num);
        }
    }
}
