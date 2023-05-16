using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTipUI : MonoBehaviour
{
    Vector2 pos;

    void Update() {
        pos = GameObject.Find("Player").transform.position;
        transform.position = new Vector2( pos.x + 2, pos.y + 5.5f );
    }
}
