using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<ShortItem> invent;
    public float moveSpeed = 10f;
    public float attackSpeed = 0.5f;
    public int damage = 33;

    private GameObject player;
    private float timeSinceAttack = 0;
    private bool attacking = false;
    private GameObject target;
    private Health targetHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if(!attacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if(!target.activeInHierarchy)
            {
                attacking = false;
            }
            else if(timeSinceAttack >= attackSpeed)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        targetHealth.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
        targetHealth = target.GetComponent<Health>();
        attacking = true;

        Debug.Log("Attacking " + target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attacking = false;
    }


}
