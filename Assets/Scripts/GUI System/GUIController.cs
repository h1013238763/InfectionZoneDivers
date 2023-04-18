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
    [Space(10)]

    // Inventory
    public GameObject playerInvent;
    public GameObject publicInvent;
    public GameObject quickInvent;
    [SerializeField]private GameObject[] materialPanel = new GameObject[4];
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
        else
            quickInvent.SetActive(true);
        playerInvent.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerAction>().playerInputActions.General.Disable();
    }
    public void ExitInventory(){
        playerInvent.SetActive(false);
        publicInvent.SetActive(false);
        quickInvent.SetActive(false);
        HideItemDetail();
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

    // Quick Inventory Method
    public void SetQuickInvent(){
        Sprite temp;
        // weapon slots
        for(int i = 0; i < 2; i ++)
        {
            if(PlayerAction.player.weaponSlot[(PlayerAction.player.currentWeapon+i)%2] != null){
                temp = PlayerAction.player.weaponSlot[(PlayerAction.player.currentWeapon+i)%2].itemSprite;
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

    // Combat GUI Setting
    // ammo setting
    public void SetAmmoText(int ammo, int capa){
        ammoText.GetComponent<Text>().text = ammo.ToString() + " / " + capa.ToString();
    }
    public void SetAmmoIcon(){
        ammoIcon.SetActive(true);
        PlayerAction player = GameObject.Find("Player").GetComponent<PlayerAction>();
        if(player.weaponSlot[player.currentWeapon] != null)
            ammoIcon.GetComponent<Image>().sprite = ItemController.controller.database.itemDict[player.weaponSlot[player.currentWeapon].weaponAmmoIndex].itemSprite;
        else
            ammoIcon.SetActive(false);
    }
    public void SetAmmoInventText(){
        ammoInventText.GetComponent<Text>().text = ItemController.controller.ItemNumber(currentAmmoID, GameObject.Find("Player").GetComponent<Invent>()).ToString();
    }
    // reload tip
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
    // fire colddown tip
    public void SetFireColdTip(float time, float timeMax){
        fireCold = time;
        fireMax = timeMax;
    }
    public void SetFireColdTip(float length){
        fireColdCover.transform.localScale = new Vector3(1, length, 0);
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
