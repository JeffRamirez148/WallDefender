using UnityEngine;
using System.Collections;
using DigitalRubyShared;
using TMPro;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DefaultEnemy : Enemy
{
    public int damageDealth = 3;
    public float waveYForce = 300.0f;
    public float enableCollisionYPos;
    public BoxCollider2D collider;

    //public TextMeshPro healthMesh;
    [HideInInspector]
    public bool debuff = false;
    [HideInInspector]
    public Action<GameObject> OnDeath;
    [HideInInspector]
    public bool shielded = false;
    [HideInInspector]
    public List<Collider2D> waves = new List<Collider2D>();

    // Use this for initialization
    protected virtual void Start()
    {
        numOfHits = maxNumOfHits;
    }

    private static bool IsObjNull(Collider2D col)
    {
        return col == null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(transform.localPosition.y <= enableCollisionYPos)
        {
            collider.enabled = true;
        }

        waves.RemoveAll(IsObjNull);
        if (numOfHits <= 0)
        {
            Death();
        }
    }

    public virtual void Shield()
    {
        shielded = true;
    }

    public virtual void Unshield()
    {
        shielded = false;
    }


    public override void TakeDamage(float multiplier)
    {
        if (!shielded)
        {
            int damage = ((int)(((float)tapDamage) * multiplier));

            if (debuff)
            {
                damage *= 2;
            }

            numOfHits -= damage;
        }
        if(numOfHits < 0)
        {
            numOfHits = 0;
        }

        //called after setting numofhits to 0 so can get index properly
        UpdateHitColor();
    }

    protected void UpdateHitColor()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = hitColors[numOfHits];
    }

    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (!shielded)
        {
            if (coll.gameObject.tag == "DebuffArea")
            {
                debuff = true;
            }
        }

        if (coll.gameObject.tag == "Wall")
        {
            this.GetComponent<Rigidbody2D>().simulated = false;
            this.GetComponent<Collider2D>().enabled = false;
            WallBehaviour wall = coll.gameObject.GetComponent<WallBehaviour>();
            if (!wall.oob)
            {
                StartCoroutine(WaitToDestroy(wall));
            }
            else
            {
                Death();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KnockBack" && !waves.Contains(collision))
        {
            waves.Add(collision);
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0.0f, waveYForce, 0.0f));
        }
        if (!shielded)
        {
            if (collision.gameObject.tag == "DebuffArea")
            {
                debuff = true;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DebuffArea")
        {
            debuff = false;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "DebuffArea")
        {
            debuff = false;
        }

        //if (coll.gameObject.tag == "Enemy")
        //{
        //    EnemyShield enemy = coll.gameObject.GetComponent<EnemyShield>();
        //    if (enemy)
        //    {
        //        Debug.Log("Unshielded!!!");
        //        this.Unshield();
        //    }
        //}
    }

    public override void Death()
    {
        if (OnDeath != null)
        {
            OnDeath(this.gameObject);
        }
        Destroy(this.gameObject);
    }

    protected virtual IEnumerator WaitToDestroy(WallBehaviour wall)
    {
        yield return new WaitForSeconds(0.1f);
        wall.health -= damageDealth;
        Death();
    }

}
