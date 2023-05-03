using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int health = 100;

    public int currentHealth;

    public float hitTimecurr;
    public float hitTimeMax = 0.5f;

    private void Start()
    {
        currentHealth = health;
    }

    private void FixedUpdate(){
        
        if(hitTimecurr >= hitTimeMax){
            transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        else{
            hitTimecurr += Time.deltaTime;
        }
    }

    public int TakeDamage(int damage)
    {
        currentHealth -= damage;

        hitTimecurr = 0;
        transform.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0, 1f);

        if(currentHealth <= 0)
        {
            Die();
        }
        return currentHealth;
    }

    public int Heal(int heal)
    {
        currentHealth += Mathf.Abs(heal);
        return currentHealth;
    }

    private void Die()
    {
        if(gameObject.tag == "Player")
        {
            WorldController.controller.GameEnd(1);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
