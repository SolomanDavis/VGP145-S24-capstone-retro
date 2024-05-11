using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected Animator anim;
    
    [SerializeField] private int EnemyHealth;
    [SerializeField] private EnemyProjectile enemyProjectile;
    public Transform enemyProjectileSpawn;
    [SerializeField] private int projectileSpeed;
    public float TimeToDestroy = 1;


    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();                
    }



    // Update is called once per frame
    void Update()
    {
        
    }



    // TriggerOnAnimationEvent
    public void Shoot(int min, int max)
    {
        //This offset will allow the enemy script to choose to fire the projectile
        //at the player with an offset to the left and right (we think....)
        int RandomNumberOffset = Random.Range(min, max);

        EnemyProjectile currentProjectile = Instantiate(enemyProjectile, enemyProjectileSpawn.position, enemyProjectileSpawn.rotation);
        
        currentProjectile.speed = projectileSpeed;

        currentProjectile.offset = RandomNumberOffset;
    }



    public virtual void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth <= 0)
        {
            anim.SetTrigger("IsDead");
        }
    }



        // This should called right at the end of the animation event on enemy death
    public virtual void EnemyDeath(int score)
    {
        GameManager.Instance.AddToScore(score);
        Destroy(gameObject);
        // EnemiesOnScreen --;
        // TotalNumberOfEnemiesKilled ++;
    }



    //public void Pathfinding()
    //{

    //}

}
