using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float attackSpeed = 0.5f;
    public int damage = 33;

    private GameObject player;
    private float timeSinceAttack = 0;
    private bool attacking = false;
    [SerializeField]private GameObject target;
    private Health targetHealth;
    private Rigidbody2D rigidBody;
    private Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if(!attacking)
        {
            if(transform.position.x - player.transform.position.x > 0.2){
                dir.x = -1;
            }
            else if(transform.position.x - player.transform.position.x < -0.2){
                dir.x = 1;
            }
            else{
                dir.x = 0;
            }

            if(transform.position.y - player.transform.position.y > 0.3){
                dir.y = -1;
            }
            else if(transform.position.y - player.transform.position.y < -0.3){
                dir.y = 1;
            }
            else{
                dir.y = 0;
            }
            rigidBody.velocity = moveSpeed * dir;
        }
        else
        {
            if(!target.activeSelf)
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
        timeSinceAttack = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Building" || collision.tag == "Player"){
            target = collision.gameObject;
            targetHealth = target.GetComponent<Health>();
            attacking = true;
            rigidBody.velocity = new Vector2(0, 0);
            
        }
        Debug.Log("Attacking " + target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attacking = false;
    }


}
