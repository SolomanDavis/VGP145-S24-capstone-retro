using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;

    [HideInInspector] public int speed;
    [HideInInspector] public int offset;
    [SerializeField] public int lifeTime;
    [SerializeField] private float bulletSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PlayerController player = FindObjectOfType<PlayerController>();

        // Once PlayerController is pushed, uncomment this out --------------------------
        rb.velocity = -Vector2.up * bulletSpeed;

        CanvasManager.Instance.GamePaused += OnPause;
        CanvasManager.Instance.GameUnpaused += OnUnpause;

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

    private void OnPause()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnUnpause()
    {
        rb.velocity = -Vector2.up * bulletSpeed;
    }
}
