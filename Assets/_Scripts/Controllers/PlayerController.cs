using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ensures that these components are attached to the gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Speed of player movement
    [SerializeField] private float deathAnimationDuration = 1.5f; // Duration of death animation in seconds
    private Animator animator;

    private void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);

        // Check if the player is moving horizontally and set animation parameter accordingly
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            //GameManager.Instance.Lives--;

            // Play death animation
            animator.SetTrigger("Die");

            // Destroy the GameObject after the death animation finishes
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //GameManager.Instance.Lives--;

            animator.SetTrigger("Die");

            // Destroy the GameObject after the death animation finishes
            StartCoroutine(DestroyAfterAnimation());
        }
    }


    // Coroutine to destroy the GameObject after the death animation finishes
    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the specified duration or the length of the death animation, whichever is greater
        yield return new WaitForSeconds(Mathf.Max(deathAnimationDuration, animator.GetCurrentAnimatorStateInfo(0).length));

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
