using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected Animator anim;
    
    protected int EnemyHealth;
    public Transform ProjectileSpawn;
    public float TimeToDestroy = 2;

    //Need to know how enemies will be animated.
    //If the image only rotates as opposed to the object then we would need an animation event
    protected bool isFacingDown = false; 


    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(ShootCoroutine());

        EnemyHealth = 2;

    }


    protected IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (isFacingDown)
            {
                Shoot();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void Shoot()
    {
        // If an animation event is created then
        // isFacingDown = True
    }


    public virtual void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject, TimeToDestroy);
        }
    }


    //public void Pathfinding()
    //{

    //}


}
