using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Explosive : MonoBehaviour
{
    [SerializeField]private float bulletTime;
    public float bulletSpeed;
    public int bulletDamage;

    [SerializeField]private Rigidbody2D rigidBody;

    public float repeat;
    public float duration;
    
    void FixedUpdate(){

        bulletTime -= Time.deltaTime;
        if(bulletTime <= 0)
            Explose();
    }

    public void Fire(Vector2 from, Vector2 direction, double radius,int damage, float range){

        int degree = (int)(radius * 180 / 3.1415);
        transform.position = new Vector3(from.x, from.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, degree);

        rigidBody.velocity = bulletSpeed * direction;

        bulletDamage = damage;

        bulletTime = range * 0.05f;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Enemy"){
            Explose();
        }
    }

    public void SetExplosive(float repeatTime, float durTime){
        repeat = repeatTime;
        duration = durTime;
    }

    public void Explose(){
        GameObject effect = CombatController.controller.GetEffectFromPool();

        effect.GetComponent<Bullet_Effect>().SetEffect(bulletDamage, repeat, duration);
        effect.transform.position = transform.position;
        effect.SetActive(true);
        gameObject.SetActive(false);
    }
}
