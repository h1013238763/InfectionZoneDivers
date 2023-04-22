using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor
{
    public int[] resourceList = new int[4];
    public int survivorNum;

    public Survivor(int point, int tend){
        point = (int)(point * (1 + 0.25f*(tend-1)));

        resourceList[2] = (int)(point * Random.Range(0.05f, 0.15f));
        resourceList[1] = (int)(point * Random.Range(0.15f, 0.25f));
        resourceList[3] = (int)(point * Random.Range(0.25f, 0.35f));
        resourceList[0] = point - resourceList[1] - resourceList[2] - resourceList[3];

        survivorNum = tend;
    }
}
