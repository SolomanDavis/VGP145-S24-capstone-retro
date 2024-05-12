using System.Collections;
using UnityEngine;

public class LightEnemy : Enemy
{

    public float fireRate = 1f; // Rate of fire (bullets per second)
    public float strafeDistance = 5.0f;
    public float strafeDuration = 10f;
    public float currentHealth = 1f;
    public float damageTaken;
    public float moveSpeed;

    private float nextFireTime; // Time of the next fire
    private bool movingRight = true; //Moving to the right
    private float strafeTimer = 10f;

    protected override void Start()
    {
        base.Start();
        damageTaken = 0;
        StartCoroutine(Countdown() );
    }

    IEnumerator Countdown()
    { 
        while (strafeTimer > 0f) 
        {
            yield return new WaitForSeconds(1f);
            strafeTimer--;
            Debug.Log("Time remaining until direction change:" + strafeTimer);
        }
    }

    //Enemy animations, damage and animations, aim in the direction the enemy is facing, enemy spawning, score

    void Update()
    {
        // Move towards the target position


        // Check if it's time to shoot
        //if (Time.time >= nextFireTime)
        // Shoot a bullet (Shoot script goes here)
        // 
        // Update the next allowed fire time, placeholder for shoot update function
        //nextFireTime = Time.time + 1f / fireRate;
    }
    //Placeholder Script for idle animation and change direction
    private void strafe()
    {
        if (strafeTimer <= 0)
        {
            moveSpeed *= -1; //will change direction of the unit when timer elapses
            strafeTimer = 10f;
        }
    }

    void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerProjectile"))
            {
                damageTaken++;
                if (damageTaken >= currentHealth)
                {
                Destroy(obj: LightEnemy); 
                }
            }
        }
   
}