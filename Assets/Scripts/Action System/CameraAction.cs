using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAction : MonoBehaviour
{
    [SerializeField]private Transform player;
    public int cameraMode = 0;
    public float cameraSize;
    public float scope;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (cameraMode){
            case 0:
                OnNormal();
                break;
            case 1:
                OnAim();
                break;
            default:
                break;
        }
        
    }

    private void OnNormal(){
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }

    private void OnAim(){
        // get mouse pos from center point
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos.x = (scope-1) * (mousePos.x/Screen.width - 0.5f) * cameraSize * 1.8f + player.position.x;
        mousePos.y = (scope-1) * (mousePos.y/Screen.height - 0.5f) * cameraSize* 1.8f + player.position.y;

        transform.position = new Vector3(mousePos.x, mousePos.y, -10 );
    }
}
