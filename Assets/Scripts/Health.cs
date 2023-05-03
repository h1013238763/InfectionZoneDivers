using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int health = 100;

    public int currentHealth;

    private void Start()
    {
        currentHealth = health;
    }

    public int TakeDamage(int damage)
    {
        currentHealth -= damage;
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
