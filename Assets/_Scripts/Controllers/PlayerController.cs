
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ensures that these components are attached to the gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of player movement
    public GameObject bulletPrefab; // Prefab of the bullet GameObject
    public Transform firePoint; // Point where the bullet will be spawned
    public float bulletSpeed = 10f; // Speed of the bullet


    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);

        // Shooting
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate a bullet at the firePoint position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply velocity to the bullet (upward)
        rb.velocity = Vector2.up * bulletSpeed;
    }
}
