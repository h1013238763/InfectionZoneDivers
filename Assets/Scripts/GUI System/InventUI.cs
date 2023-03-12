using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventUI : MonoBehaviour
{
    [SerializeField]private List<GameObject> slots = new List<GameObject>();
    [SerializeField]private DatabaseController database;
    [SerializeField]private int capacity;
    [SerializeField]private GameObject slotPrefab;

    public void ResetInvent(List<ShortItem> list, int capacity){
        for(int i = 0; i < list.Count; i ++){
            if(list[i] != null)
                slots[i].GetComponent<SlotUI>().Reset(list[i], database.itemDict[list[i].itemID].itemSprite);
            else
                slots[i].GetComponent<SlotUI>().Hide();
        }
        for(int i = list.Count; i < capacity; i ++)
            slots[i].GetComponent<SlotUI>().Hide();
    }

    public void OpenInventory(int capacity){
        while(slots.Count < capacity){
            GameObject temp;
            temp = Instantiate(slotPrefab);
            temp.transform.SetParent(transform.GetChild(0));
            slots.Add(temp);
        }

        if(!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void PrintInvent(){
        foreach(GameObject slot in slots)
            slot.GetComponent<SlotUI>().Print();
    }

}
