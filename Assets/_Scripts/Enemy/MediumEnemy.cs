using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static EnemyPathfinding;

public class MediumEnemy : Enemy
{
    public EnemyPathfinding enemyPathfindingState;

    [SerializeField] AudioClip enemyHit;
    AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        enemyPathfindingState = GetComponent<EnemyPathfinding>();

        audioSource = GetComponent<AudioSource>();
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

        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(1);
            if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Entrance)
            {
                EnemyDeath(80);
            }
            else if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Hover)
            {
                EnemyDeath(80);
            }
            else if (enemyPathfindingState.State == EnemyPathfinding.PathfindingState.Dive)
            {
                EnemyDeath(160);
            }
        }
        audioSource.PlayOneShot(enemyHit);
    }
}
