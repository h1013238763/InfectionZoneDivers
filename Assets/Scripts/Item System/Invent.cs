using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invent : MonoBehaviour
{
    public List<ShortItem> inventList = new List<ShortItem>();
    public int inventCap;

    void Start(){
        inventList.Add(new ShortItem(4,400));
    }
}
