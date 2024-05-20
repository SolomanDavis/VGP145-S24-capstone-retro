using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ensures that these components are attached to the gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D bc;

    [SerializeField] private float moveSpeed = 5f; // Speed of player movement

    private bool _isPaused = false;

    private void Awake()
    {
        CanvasManager.Instance.GamePaused += () => _isPaused = true;
        CanvasManager.Instance.GameUnpaused += () => _isPaused = false;
    }

    private void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_isPaused)
            return;

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);

        // Check if the player is moving horizontally and set animation parameter accordingly
        // animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }


    //Player Destruction
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile") || collision.CompareTag("Enemy"))
        {
            GameManager.Instance.Lives--;

            // rb.simulated = false;
            bc.enabled = false;

            animator.SetTrigger("Die");
        }
    }

    // Coroutine to destroy the GameObject after the death animation finishes
    public void PlayerDeath()
    {
        // Destroy the GameObject
        Destroy(gameObject);
    }
}
