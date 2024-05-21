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
    private AudioSource audioSource;

    //added rb to set player RigidBody position == to areaBoundaries
    Rigidbody2D rb;
    GameObject areaBoundaryOne;
    GameObject areaBoundaryTwo;
       
    [SerializeField] private float moveSpeed = 5f; // Speed of player movement
    [SerializeField] AudioClip playerDeath;

    private bool _isPaused = false;

    private void Awake()
    {
        CanvasManager.Instance.GamePaused += () => _isPaused = true;
        CanvasManager.Instance.GameUnpaused += () => _isPaused = false;

        areaBoundaryOne = GameObject.FindWithTag("PlayerAreaBoundaryOne");
        areaBoundaryTwo = GameObject.FindWithTag("PlayerAreaBoundaryTwo");
    }

    private void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isPaused)
            return;

        // Player movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);

        // if the player's transform.x is == to areaBoundaryOne then ensure that the player does not move past it, left off scren.
        if (gameObject.transform.position.x <= areaBoundaryOne.transform.position.x)
        {
            Vector2 newPosition = rb.transform.position;
            newPosition.x = areaBoundaryOne.transform.position.x;
            rb.transform.position = newPosition;
        }

        // if the player's transform.x is == to areaBoundaryTwo then ensure that the player does not move past it, right off scren.
        if (gameObject.transform.position.x >= areaBoundaryTwo.transform.position.x)
        {
            Vector2 newPosition = rb.transform.position;
            newPosition.x = areaBoundaryTwo.transform.position.x;
            rb.transform.position = newPosition;
        }
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
            audioSource.PlayOneShot(playerDeath);

            // Disable shooting
            Shoot shootController = GetComponent<Shoot>();
            if (shootController) shootController.enabled = false;
        }
    }

    // Coroutine to destroy the GameObject after the death animation finishes
    public void PlayerDeath()
    {
        // Destroy the GameObject
        Destroy(gameObject);
    }
}
