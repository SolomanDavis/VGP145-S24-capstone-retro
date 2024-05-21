using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Projectile bulletPrefab; // Prefab of the bullet GameObject
    [SerializeField] private Transform firePoint; // Point where the bullet will be spawned
    [SerializeField] private AudioClip shootClip;

    private bool _isPaused = false;

    private void Awake()
    {
        CanvasManager.Instance.GamePaused += () => _isPaused = true;
        CanvasManager.Instance.GameUnpaused += () => _isPaused = false;
    }

       private void Update()
    {
        if (_isPaused)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Instantiate a bullet at the firePoint position
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (shootClip) GetComponent<AudioSource>().PlayOneShot(shootClip);
    }
}
