using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;

    [HideInInspector] public int speed;
    [HideInInspector] public int offset;
    [SerializeField] public int lifeTime;

    // Once PlayerController is pushed, uncomment this out ---------------------------
    //[SerializeField] PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Once PlayerController is pushed, uncomment this out --------------------------
        //rb.velocity = Vector2.MoveTowards(transform.position, player.transform + offset, speed * Time.deltaTime);

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
