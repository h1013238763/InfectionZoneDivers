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
    public int weaponAmmoCapa;      // weapon ammo capacity
    public float weaponRange;       // weapon shoot range
    public float weaponScope;
    public float weaponAccuracy;    // weapon shoot accuracy, 1 means shoot in line
    public float weaponSpeed;       // weapon attack speed, 0.8 means 0.8 attack / sec
    public float weaponReload;      // weapon reload time, in secs
    public int weaponAmmoIndex;     // weapon ammo type
    public Vector2 weaponMuzzle;    // weapon muzzle position
}
