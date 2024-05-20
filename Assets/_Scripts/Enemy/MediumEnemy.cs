using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static EnemyPathfinding;

public class MediumEnemy : Enemy
{
    public EnemyPathfinding enemyPathfindingState;

    public float currentHealth = 2f;
    public int damageTaken;
    public float moveSpeed;

    private float nextFireTime; // Time of the next fire
    private bool movingRight = true; //Moving to the right
    //private float strafeTimer = 10f;

    protected override void Start()
    {
        base.Start();
        damageTaken = 0;
        enemyPathfindingState = GetComponent<EnemyPathfinding>();
        //StartCoroutine(Countdown());
    }

    //IEnumerator Countdown() //Strafe Timer for Enemies

    //while (strafeTimer > 0f)
    //yield return new WaitForSeconds(1f);
    //strafeTimer--;
    //Debug.Log("Time remaining until direction change:" + strafeTimer);

    //Enemy animations, damage and animations, aim in the direction the enemy is facing, enemy spawning, score

    //Placeholder Script for idle animation and change direction
    //private void strafe() //Used to change direction for Strafing

    //if (strafeTimer <= 0)

    //moveSpeed *= -1; //will change direction of the unit when timer elapses
    //strafeTimer = 10f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "PlayerProjectile")
        {
            TakeDamage(1);
            if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Entrance)
            {
                EnemyDeath(50);
            }
            else if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Hover)
            {
                EnemyDeath(50);
            }
            else if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Dive)
            {
                EnemyDeath(100);
            }
        }
    }
}
