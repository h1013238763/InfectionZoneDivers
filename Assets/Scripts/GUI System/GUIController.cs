using System.Collections;
using System.Collections.Generic;
using System;
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
    [Space(10)]

    // Inventory
    public GameObject playerInvent;
    public GameObject publicInvent;
    public GameObject quickInvent;
    [SerializeField]private GameObject resourcePanel;
    [SerializeField]private GameObject inventSlot;
    [SerializeField]private int inventCap;
    [SerializeField]private GameObject itemDetail;
    public Invent currentPublicInvent;
    [Space(10)]


    // Combat GUI
    [SerializeField]private GameObject[] WeaponIcon = new GameObject[2];
    [SerializeField]private GameObject quickSlot;
    [SerializeField]private GameObject ammoIcon;
    [SerializeField]private GameObject ammoText;
    [SerializeField]private GameObject ammoInventText;
    [SerializeField]private GameObject fireColdCover;
    private float fireCold;
    private float fireMax;
    [Space(10)]

    // Blueprint GUI
    public GameObject blueprintPanel;

    // Start is called before the first frame update
    void Start(){
        controller = this;
        InitialInventory();
        
        SetGUI(PlayerAction.player.invent, "Player");
    }

    // Update is called once per frame
    void Update(){

        if(fireCold > 0){
            fireCold -= Time.deltaTime;
            SetFireColdTip( fireCold / fireMax );
        }

        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // SetBuildTipPosition((int)mousePos.x, (int)mousePos.y);
    }

    public void SetGUI(Invent invent, string tag){
        SetInventory(invent, tag);
        SetAmmoInventText();
        SetAmmoIcon();
        SetQuickInvent();
        SetAmmoText();
    }


    // Inventory Methods

    /// <summary>
    /// Initial slots to inventory
    /// </summary>
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

    /// <summary>
    /// open inventory
    /// </summary>
    /// <param name="tag"> open which inventory</param>
    public void EnterInventory(string tag){
        if( tag != "Player")
            publicInvent.SetActive(true);
        else
            quickInvent.SetActive(true);
        playerInvent.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Disable();
    }

    /// <summary>
    /// close inventory
    /// </summary>
    public void ExitInventory(){
        playerInvent.SetActive(false);
        publicInvent.SetActive(false);
        quickInvent.SetActive(false);
        HideItemDetail();
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Enable();
    }

    /// <summary>
    /// reset all ui of inventory panel
    /// </summary>
    /// <param name="invent"> the invent data </param>
    /// <param name="tag"> set which inventory </param>
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

    /// <summary>
    /// trigger inventory button
    /// </summary>
    /// <param name="tag"></param>
    public void ActiveInventory(string tag){
        if(playerInvent.activeSelf)
            ExitInventory();
        else
            EnterInventory(tag);
    }

    /// <summary>
    /// show and hide datail panel of item
    /// </summary>
    /// <param name="si"> show which item </param>
    public void ShowItemDetail(ShortItem si){
        Item item = ItemController.controller.database.itemDict[si.itemID];

        itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        //itemDetail.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
        itemDetail.transform.GetChild(1).GetComponent<Text>().text = item.itemName;
        itemDetail.transform.GetChild(2).GetComponent<Text>().text = "    " + item.itemDescribe;
        itemDetail.transform.GetChild(3).GetComponent<Text>().text = " [ " + item.GetType().ToString() + " ]";
        itemDetail.transform.GetChild(4).gameObject.SetActive(false);
        itemDetail.transform.GetChild(5).gameObject.SetActive(false);

        if(item is Weapon){
            Weapon weapon = (Weapon)item;
            Transform grid = itemDetail.transform.GetChild(4);
            grid.GetChild(0).GetComponent<Text>().text = weapon.weaponDamage.ToString();
            grid.GetChild(1).GetComponent<Text>().text = weapon.weaponAmmoCapa.ToString();
            grid.GetChild(2).GetComponent<Text>().text = weapon.weaponSpeed.ToString();
            grid.GetChild(3).GetComponent<Text>().text = weapon.weaponAccuracy.ToString();
            grid.GetChild(4).GetComponent<Text>().text = weapon.weaponRange.ToString();
            grid.GetChild(5).GetComponent<Text>().text = weapon.weaponReload.ToString();
            grid.GetChild(6).GetComponent<Text>().text = weapon.weaponAmmoIndex.ToString();
            grid.gameObject.SetActive(true);
        }
        if(item is Consumable){
            Consumable consume = (Consumable)item;
            string[] tokens = consume.consumableDescribe.Split('$');
            string describe = "";
            for(int i = 0; i < tokens.Length; i ++){
                if(tokens[i] == "0" || tokens[i] == "1")
                    tokens[i] = consume.consumableData[Int32.Parse(tokens[i])].ToString();
                describe += tokens[i];
            }
            itemDetail.transform.GetChild(5).GetComponent<Text>().text = "    " + describe;
            itemDetail.transform.GetChild(5).gameObject.SetActive(true);
        }    
        itemDetail.SetActive(true);
    }
    public void ShowItemDetail(int index){
        if(index < 2){
            Weapon weapon = PlayerAction.player.weaponSlot[index];
            if( weapon!= null)
                ShowItemDetail( new ShortItem( weapon.itemID, 1 ) );
        }else{
            if(PlayerAction.player.quickSlot[index-2] != null)
                ShowItemDetail( PlayerAction.player.quickSlot[index-2] );
        }
    }
    public void HideItemDetail(){
        itemDetail.SetActive(false);
    }

    // Combat GUI Setting

    /// <summary>
    /// Set quick and weapon slot ui
    /// </summary>
    public void SetQuickInvent(){
        Sprite temp;
        // weapon slots
        for(int i = 0; i < 2; i ++)
        {
            if(PlayerAction.player.weaponSlot[i] != null){
                temp = PlayerAction.player.weaponSlot[i].itemSprite;
                quickInvent.transform.GetChild(i).GetComponent<QuickSlotUI>().Reset(temp, i, 1);
                WeaponIcon[i].transform.GetChild(0).GetComponent<Image>().sprite = temp;
                WeaponIcon[i].transform.GetChild(0).gameObject.SetActive(true);
            }else{
                quickInvent.transform.GetChild(i).GetComponent<QuickSlotUI>().Hide();
                WeaponIcon[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        // item slots
        for(int i = 0; i < 4; i ++){
            if(PlayerAction.player.quickSlot[i] != null){
                temp = ItemController.controller.ItemFind(PlayerAction.player.quickSlot[i]).itemSprite;
                quickInvent.transform.GetChild(i+2).GetComponent<QuickSlotUI>().Reset(temp, i+2, PlayerAction.player.quickSlot[i].itemNum);
                quickSlot.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = temp;
                quickSlot.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = PlayerAction.player.quickSlot[i].itemNum.ToString();
                quickSlot.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                quickSlot.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }else{
                quickInvent.transform.GetChild(i+2).GetComponent<QuickSlotUI>().Hide();
                quickSlot.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                quickSlot.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set text of current ammo number 
    /// </summary>
    /// <param name="ammo"> the number of ammo </param>
    /// <param name="capa"> the capacity of ammo </param>
    public void SetAmmoText(){
        ammoText.GetComponent<Text>().text = PlayerAction.player.ammoSlot[0].ToString() + " / " + PlayerAction.player.weaponSlot[0].weaponAmmoCapa.ToString();
    }

    /// <summary>
    /// Set ammo icon to current weapon's ammo icon
    /// </summary>
    public void SetAmmoIcon(){
        ammoIcon.SetActive(true);
        PlayerAction player = GameObject.Find("Player").GetComponent<PlayerAction>();
        if(player.weaponSlot[0] != null)
            ammoIcon.GetComponent<Image>().sprite = ItemController.controller.database.itemDict[player.weaponSlot[0].weaponAmmoIndex].itemSprite;
        else
            ammoIcon.SetActive(false);
    }

    /// <summary>
    /// Set text of how many ammo in inventory
    /// </summary>
    public void SetAmmoInventText(){
        int currentAmmoID = PlayerAction.player.weaponSlot[0].weaponAmmoIndex;
        ammoInventText.GetComponent<Text>().text = ItemController.controller.ItemNumber(currentAmmoID, GameObject.Find("Player").GetComponent<Invent>()).ToString();
    }

    /// <summary>
    /// Set the reload ui on reloading object
    /// </summary>
    /// <param name="time"> how long the object reload </param>
    /// <param name="from"> which object need to reload </param>
    /// <param name="player"> is player reload? </param>
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

    /// <summary>
    /// Set the fire colddown tip ui
    /// </summary>
    /// <param name="time"></param>
    /// <param name="timeMax"></param>
    public void SetFireColdTip(float time, float timeMax){
        fireCold = time;
        fireMax = timeMax;

    }
    public void SetFireColdTip(float length){
        if(length <= 0)
            length = 0;
        fireColdCover.transform.localScale = new Vector3(1, length, 0);
    }

    /// <summary>
    /// Set top-right resource panel
    /// </summary>
    public void SetResourcePanel(){
        for(int i = 0; i < 4; i ++){
            resourcePanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = ItemController.controller.resource[i].ToString();
        }
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
