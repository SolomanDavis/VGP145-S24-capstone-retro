using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static EnemyPathfinding;

public class HeavyEnemy : Enemy
{

    private EnemyPathfinding enemyPathfindingState;
  
    [SerializeField] AudioClip enemyHit;

    AudioSource audioSource;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemyPathfindingState = GetComponent<EnemyPathfinding>();

        audioSource = GetComponent<AudioSource>();
    }
     
    /*
     OnTriggerEnter(Collider other): Calls the TakeDamage()
     when the enemy has a collision with the player's prjectile.
     Also calls HeavyEnemyHealth() that will in turn call the ChangeColor() to
     change the enemy's color when it has taken its 1st hit.
    */

    //TBC if player projectile is trigger _ TODO (Estelle)
    //TBC player projectile tage name _ TODO (Estelle)
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.CompareTag("PlayerProjectile"))
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
            audioSource.PlayOneShot(enemyHit);
        }

    }

    // Calls EnemyDeath with relevant state score
    public void callDeathWithScore()
    {

        switch (enemyPathfindingState.State)
        {
            case EnemyPathfinding.PathfindingState.Entrance:
            case EnemyPathfinding.PathfindingState.Hover:
                EnemyDeath(150);
                break;
            case EnemyPathfinding.PathfindingState.Dive:
                EnemyDeath(400);
                break;
        }
        
    }

}


