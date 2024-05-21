using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;

    [HideInInspector] public int speed;
    [HideInInspector] public int offset;
    [SerializeField] public int lifeTime;
    [SerializeField] public float bulletSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = -Vector2.up * bulletSpeed;

        CanvasManager.Instance.GamePaused += OnPause;
        CanvasManager.Instance.GameUnpaused += OnUnpause;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
