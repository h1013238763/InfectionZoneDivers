using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float bulletTime = 0;
    public float bulletSpeed;
    public int bulletDamage;

    [SerializeField]private Rigidbody2D rigidBody;
    
    void FixedUpdate(){

        bulletTime -= Time.deltaTime;
        if(bulletTime <= 0)
            gameObject.SetActive(false);
    }

    public void Fire(Vector2 from, Vector2 direction, double radius,int damage, float range){
        int degree = (int)(radius * 180 / 3.1415);
        transform.position = new Vector3(from.x, from.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, degree);

        rigidBody.velocity = bulletSpeed * direction;
        bulletDamage = damage;

        bulletTime = range * 0.01f;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Enemy"){
            collision.transform.GetComponent<Health>().TakeDamage(bulletDamage);
            gameObject.SetActive(false);
        }
    }
    
}
