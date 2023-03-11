using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Describe:
        It is the prototype of all weapons
*/
[CreateAssetMenu(fileName = "Weapon", menuName = "Infection Zone Divers/Weapon", order = 1)]
public class Weapon : Item
{
    public int weaponDamage;        // weapon damage
    public int weaponAmmoCurr;      // weapon current ammo number
    public int weaponAmmoCapa;      // weapon ammo capacity
    public float weaponRange;       // weapon shoot range
    public int weaponAccuracy;      // weapon shoot accuracy, 1 means shoot in line
    public float weaponSpeed;       // weapon attack speed, 0.8 means 0.8 attack / sec
    public float weaponReload;      // weapon reload time, in secs
    public AmmoType weaponAmmoType; // weapon ammo type
    public Vector2 weaponMuzzle;    // weapon muzzle position from the center of sprite

    public enum AmmoType{
        Small,
        Medium,
        Large,
        Shotgun,
        Explosive,
    }

    // define how it use as item in inventory
    public void Use(){
        
    }

    public Vector2 MuzzlePosition(Vector2 player){
        return player;
    }
}
