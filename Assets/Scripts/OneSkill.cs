using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneSkill : BaseSkills
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy entity = other.gameObject.GetComponent<Enemy>();
            if (entity)
            {
                entity.TakeDamage(1.0f);
            }
        }
    }
}
