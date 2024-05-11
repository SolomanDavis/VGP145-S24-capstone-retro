using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Prefab of the bullet GameObject
    [SerializeField] private Transform firePoint; // Point where the bullet will be spawned
    [SerializeField] private float bulletSpeed = 10f; // Speed of the bullet

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fire();
        }
    }

    void fire()
    {
        // Instantiate a bullet at the firePoint position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply velocity to the bullet (upward)
        rb.velocity = Vector2.up * bulletSpeed;
    }
}
