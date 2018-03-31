using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShield : Enemy
{
    // Use this for initialization
    protected virtual void Start()
    {
        //health = 3;
        //tapDamage = 1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if (health <= 0)
        //{
        //    Death();
        //}
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }

    public override void TakeDamage(float multiplier)
    {
        //int damage = ((int)(((float)tapDamage) * multiplier));
        //health -= damage;

        //if (health < 0)
        //{
        //    health = 0;
        //}
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            DefaultEnemy enemy = collision.GetComponent<DefaultEnemy>();
            if (enemy)
            {
                enemy.Shield();
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DefaultEnemy enemy = collision.GetComponent<DefaultEnemy>();
            if (enemy)
            {
                enemy.Unshield();
            }
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DefaultEnemy enemy = collision.gameObject.GetComponent<DefaultEnemy>();
            if (enemy)
            {
                enemy.Shield();
            }
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DefaultEnemy enemy = collision.gameObject.GetComponent<DefaultEnemy>();
            if (enemy)
            {
                Debug.Log("Unshielded!!!");

                enemy.Unshield();
            }
        }
    }
}
