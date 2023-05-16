using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Consumable : Item{
    [TextArea]public string consumableDescribe;
    public string consumeType;
    public float consumableTime;

    public void Use(){
        
        GameObject explosive = CombatController.controller.GetExplosiveFromPool();
        double radius = PlayerAction.player.angle * PI / 180;
        Vector2 direct = new Vector2((float)(Cos(radius)), (float)(Sin(radius)));

        switch(consumeType){
            case "ArmorPack":
                PlayerAction.player.ArmorChange(40);
                break;
            case "Doping":
                PlayerAction.player.dopingBonus = 1.5f;
                PlayerAction.player.dopingTime = 11f;
                break;
            case "Grenade":
                explosive.SetActive(true);
                explosive.transform.GetComponent<SpriteRenderer>().sprite = ItemController.controller.database.itemDict[17].itemSprite;
                explosive.transform.GetComponent<Bullet_Explosive>().SetExplosive(0.5f, 0.1f);
                explosive.transform.GetComponent<Bullet_Explosive>().Fire(PlayerAction.player.transform.position, direct, radius, 50, 10);
                break;
            case "Molotov":
                explosive.SetActive(true);
                explosive.transform.GetComponent<SpriteRenderer>().sprite = ItemController.controller.database.itemDict[18].itemSprite;
                explosive.transform.GetComponent<Bullet_Explosive>().SetExplosive(0.5f, 5f);
                explosive.transform.GetComponent<Bullet_Explosive>().Fire(PlayerAction.player.transform.position, direct, radius, 15, 10);
                break;
            case "ResourcePack":
                int[] resource = new int[4];
                resource[(int)Random.Range(0f, 4f)] = 1000;
                WorldController.controller.ResourceGet(resource);
                break;
            default:
                break;
        }
    }
}
