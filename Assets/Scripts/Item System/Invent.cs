using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invent : MonoBehaviour
{
    public List<ShortItem> inventList = new List<ShortItem>();
    public int inventCap;

    void Start(){
        for(int i = 0; i < inventCap; i ++){
            inventList.Add(null);
        }
    }
}
