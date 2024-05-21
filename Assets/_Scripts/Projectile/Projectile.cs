using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float lifeTime;
    [SerializeField] private float bulletSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = -Vector2.up * bulletSpeed;


        if (lifeTime <= 0)
        {
            lifeTime = 2.0f;
        }

        CanvasManager.Instance.GamePaused += OnPause;
        CanvasManager.Instance.GameUnpaused += OnUnpause;

        Destroy(gameObject, lifeTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    { }

    private void OnPause()
    {
        if (rb) rb.velocity = Vector2.zero;
    }

    private void OnUnpause()
    {
        if (rb) rb.velocity = -Vector2.up * bulletSpeed;
    }
}

