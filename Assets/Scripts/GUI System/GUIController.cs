using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GUIController : MonoBehaviour
{
    public static GUIController controller;

    // Action Tip
    [SerializeField]private GameObject buildTip;
    [SerializeField]private GameObject interactTip;
    [SerializeField]private GameObject reloadTip;
    [SerializeField]private GameObject itemDragTip;
    [Space(10)]

    // Inventory
    public GameObject playerInvent;
    public GameObject publicInvent;
    [SerializeField]private GameObject inventSlot;
    [SerializeField]private int inventCap;
    [SerializeField]private GameObject itemDetail;
    public Invent currentPublicInvent;
    [Space(10)]

    // Combat GUI
    [SerializeField]private GameObject priWeaponIcon;
    [SerializeField]private GameObject secWeaponIcon;
    [SerializeField]private GameObject ammoIcon;
    [SerializeField]private GameObject ammoText;
    [SerializeField]private GameObject ammoInventText;
    public int currentAmmoID;
    [Space(10)]

    // Blueprint GUI
    public GameObject blueprintPanel;

    // Start is called before the first frame update
    void Start(){
        controller = this;
        InitialInventory();
        
        currentAmmoID = GameObject.Find("Player").GetComponent<PlayerAction>().weaponSlot[0].weaponAmmoIndex;
        SetAmmoInventText();
    }

    // Update is called once per frame
    void Update(){
        
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // SetBuildTipPosition((int)mousePos.x, (int)mousePos.y);
    }

    public void SetGUI(Invent invent, string tag){
        SetInventory(invent, tag);
        SetAmmoInventText();
    }


    // Inventory Methods
    private void InitialInventory(){
        while(playerInvent.transform.GetChild(0).childCount < inventCap){
            Instantiate(inventSlot, playerInvent.transform.GetChild(0));
        }
        playerInvent.SetActive(false);
        while(publicInvent.transform.GetChild(0).childCount < inventCap){
            Instantiate(inventSlot, publicInvent.transform.GetChild(0));
        }
        publicInvent.SetActive(false);
    } 
    public void EnterInventory(string tag){
        if( tag != "Player")
            publicInvent.SetActive(true);
        playerInvent.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Disable();
    }
    public void ExitInventory(){
        playerInvent.SetActive(false);
        publicInvent.SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Enable();
    }
    public void SetInventory(Invent invent, string tag){
        // which invent gui need to be set
        Transform tempInvent;
        if(tag == "Player")
            tempInvent = playerInvent.transform.GetChild(0);
        else{
            tempInvent = publicInvent.transform.GetChild(0);
            currentPublicInvent = invent;
        }
        // set slot active
        for(int i = 0; i < invent.inventCap; i ++){
            InventSlotUI tempSlot = tempInvent.GetChild(i).GetComponent<InventSlotUI>();
            if(invent.inventList[i] == null){
                tempSlot.Hide();
            }else{
                tempSlot.Reset(invent.inventList[i], ItemController.controller.database.itemDict[invent.inventList[i].itemID].itemSprite);
            }
            tempSlot.gameObject.SetActive(true);
        }
        for(int i = invent.inventCap; i < tempInvent.transform.childCount; i++){
            tempInvent.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void ActiveInventory(string tag){
        if(playerInvent.activeSelf)
            ExitInventory();
        else
            EnterInventory(tag);
    }

    // inventory slot mouse events
    public void ShowItemDetail(ShortItem si){
        Item item = ItemController.controller.database.itemDict[si.itemID];

        itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        //itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
        itemDetail.transform.GetChild(1).GetComponent<Text>().text = item.itemName;
        itemDetail.transform.GetChild(2).GetComponent<Text>().text = item.itemDescribe;
        itemDetail.transform.GetChild(3).gameObject.SetActive(false);

        if(item is Weapon){
            Weapon weapon = (Weapon)item;
            Transform grid = itemDetail.transform.GetChild(3);
            grid.GetChild(0).GetComponent<Text>().text = weapon.weaponDamage.ToString();
            grid.GetChild(1).GetComponent<Text>().text = weapon.weaponAmmoCapa.ToString();
            grid.GetChild(2).GetComponent<Text>().text = weapon.weaponSpeed.ToString();
            grid.GetChild(3).GetComponent<Text>().text = weapon.weaponAccuracy.ToString();
            grid.GetChild(4).GetComponent<Text>().text = weapon.weaponRange.ToString();
            grid.GetChild(5).GetComponent<Text>().text = weapon.weaponReload.ToString();
            grid.GetChild(6).GetComponent<Text>().text = weapon.weaponAmmoIndex.ToString();
            grid.gameObject.SetActive(true);
        }        
        itemDetail.SetActive(true);
    }
    public void HideItemDetail(){
        itemDetail.SetActive(false);
    }

    // In Game World GUI Setting
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

    public void SetAmmoInventText(){
        ammoInventText.GetComponent<Text>().text = ItemController.controller.ItemNumber(currentAmmoID, GameObject.Find("Player").GetComponent<Invent>()).ToString();
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
