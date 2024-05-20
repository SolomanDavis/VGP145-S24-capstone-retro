using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static EnemyPathfinding;

public class HeavyEnemy : Enemy
{

    public EnemyPathfinding enemyPathfindingState;
    private int heavyEnemyScore;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemyPathfindingState = GetComponent<EnemyPathfinding>();
    }

    void Update()
    {
       if(enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Entrance)
        { heavyEnemyScore = 150; }
       else if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Hover)
       { heavyEnemyScore = 150; }
       else if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Dive)
       { heavyEnemyScore = 400; }
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
        }

    }

    // Calls EnemyDeath with relevant state score
    public void callDeathWithScore()
    {
        EnemyDeath(heavyEnemyScore);
    }

}


