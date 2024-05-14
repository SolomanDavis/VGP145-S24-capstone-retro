using System.Collections;
using UnityEngine;

public class MediumEnemy : Enemy
{

    public float fireRate = 1f; // Rate of fire (bullets per second)
    public float strafeDistance = 5.0f;
    public float strafeDuration = 10f;
    public float currentHealth = 2f;
    public int damageTaken;
    public float moveSpeed;

    private float nextFireTime; // Time of the next fire
    private bool movingRight = true; //Moving to the right
    private float strafeTimer = 10f;

    protected override void Start()
    {
        base.Start();
        damageTaken = 0;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() //Strafe Timer for Enemies
    {
        while (strafeTimer > 0f)
        {
            yield return new WaitForSeconds(1f);
            strafeTimer--;
            Debug.Log("Time remaining until direction change:" + strafeTimer);
        }
    }

    //Enemy animations, damage and animations, aim in the direction the enemy is facing, enemy spawning, score

    //Placeholder Script for idle animation and change direction
    private void strafe() //Used to change direction for Strafing
    {
        if (strafeTimer <= 0)
        {
            moveSpeed *= -1; //will change direction of the unit when timer elapses
            strafeTimer = 10f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            TakeDamage(1);
        }
    }

    private void DeathAnimation()
    {
        if (damageTaken == currentHealth)
        {
            anim.SetInteger("NumberOfHits", damageTaken);
            EnemyDeath(2);
        }
    }
}
