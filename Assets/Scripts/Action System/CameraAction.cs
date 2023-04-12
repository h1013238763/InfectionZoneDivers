using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    [SerializeField]private Transform player;
    public int cameraMode = 0;
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
                break;
            default:
                break;
        }
        
    }

    private void OnNormal(){
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }

    private void OnAim(){

    }
}
