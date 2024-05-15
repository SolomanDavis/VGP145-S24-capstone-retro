using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame


    /*
     OnTriggerEnter(Collider other): Calls the TakeDamage()
     when the enemy has a collision with the player's prjectile.
     Also calls HeavyEnemyHealth() that will in turn call the ChangeColor() to
     change the enemy's color when it has taken its 1st hit.
    */

    // When colliding with the Player projectile, Heavy enemy takes "1" damage. 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            TakeDamage(1);
            UpdateHealthStatus();
        }
    }

    // When EnemyHealt == 1, the Heavy enemy changes color to purple.
    private void UpdateHealthStatus()
    {
        if (EnemyHealth == 1)
        {
            anim.SetInteger("EnemyHealth", EnemyHealth);
        }

    }
    

}


