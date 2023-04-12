using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GUIController : MonoBehaviour
{
    public static GUIController controller;

    // 
    [SerializeField]private GameObject buildTip;
    [SerializeField]private GameObject interactTip;
    [SerializeField]private GameObject reloadTip;

    //
    [SerializeField]private GameObject playerInvent;
    [SerializeField]private GameObject publicInvent;
    [SerializeField]private GameObject inventSlot;

    // In Game Fixed GUI
    [SerializeField]private GameObject priWeaponIcon;
    [SerializeField]private GameObject secWeaponIcon;
    [SerializeField]private GameObject ammoIcon;
    [SerializeField]private GameObject ammoText;
    [SerializeField]private GameObject ammoInventText;


    public GameObject blueprintPanel;

    // Start is called before the first frame update
    void Start(){
        controller = this;
    }

    void Awake(){

    }

    // Update is called once per frame
    void Update(){

        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // SetBuildTipPosition((int)mousePos.x, (int)mousePos.y);
    }

    /// Inventory Methods
    private void InitialInventory(){
        
        
    } 

    public void EnterInventory(string tag){
        GameObject targetInvent;
        if(tag == "Player")
            targetInvent = playerInvent;
        else
            targetInvent = publicInvent;
        
        targetInvent.SetActive(true);
    }

    public void ExitInventory(){
        playerInvent.SetActive(false);
        publicInvent.SetActive(false);
    }

    public void SetInventory(List<ShortItem> invent){
        
    }

    /// In Game World GUI Setting
    public void SetReloadTip(float time, GameObject from, bool player){
        GameObject temp = Instantiate(reloadTip);
        temp.GetComponent<ReloadTipUI>().timeMax = time;
        temp.transform.SetParent(from.transform);
        if(player)
            temp.transform.position = new Vector3(from.transform.position.x, from.transform.position.y+1.5f, 0f);
        else
            temp.transform.position = new Vector3(from.transform.position.x+from.GetComponent<Collider>().bounds.size.x/2, from.transform.position.y+1f, 0f);
        temp.SetActive(true);
    }

    /// In Game Canvus Fixed GUI Setting
    
    public void SetAmmoText(int ammo, int capa){
        ammoText.GetComponent<Text>().text = ammo.ToString() + " / " + capa.ToString();
    }

    public void SetAmmoInventText(int ammoID){
        ammoInventText.GetComponent<Text>().text = ItemController.controller.ItemNumber(ammoID, GameObject.Find("Player").GetComponent<Invent>().inventList).ToString();
    }

    /// Building Mode
    public void EnterBlueprint(){
        SetBuildTipSprite(null);
        buildTip.SetActive(true);
    }

    public void ExitBlueprint(){
        buildTip.SetActive(false);
    }

    public void SetBuildTipColor(bool overlap){
        if(overlap)
            buildTip.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        else
            buildTip.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f, 0.5f);
    }
    
    public void SetBuildTipSprite(Sprite sprite){
        buildTip.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void SetBuildTipPosition(int x, int y){
        if(buildTip.activeSelf)
            buildTip.transform.position = new Vector2((float)x, (float)y);
    }
}
