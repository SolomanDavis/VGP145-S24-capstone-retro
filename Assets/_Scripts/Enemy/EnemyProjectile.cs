using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;

    [HideInInspector] public int speed;
    [HideInInspector] public int offset;
    [SerializeField] public int lifeTime;
    [SerializeField] private float bulletSpeed = 10f;
    // Once PlayerController is pushed, uncomment this out ---------------------------
    //[SerializeField] PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PlayerController player = FindObjectOfType<PlayerController>();

        // Once PlayerController is pushed, uncomment this out --------------------------
        rb.velocity = -Vector2.up * bulletSpeed;

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
                if (collision.gameObject.CompareTag("Player"))
            {
                // collision.gameObject.GetComponent<Animator>().SetTrigger("IsDead");
                Destroy(gameObject);
            }
    }

}
