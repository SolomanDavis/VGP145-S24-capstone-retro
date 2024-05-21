using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
   // Rigidbody2D rb;

    [HideInInspector] public int offset;
    [SerializeField] public int lifeTime;
    [SerializeField] public float bulletSpeed = 100f;
    

    private Vector2 _aim;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        
        // PlayerController player = FindObjectOfType<PlayerController>();
        _aim = new Vector2(GameManager.Instance.PlayerInstance.transform.position.x + offset, GameManager.Instance.PlayerInstance.transform.position.y-5);
        // Once PlayerController is pushed, uncomment this out --------------------------
        // rb.velocity = -Vector2.up * bulletSpeed;
        

        CanvasManager.Instance.GamePaused += OnPause;
       // CanvasManager.Instance.GameUnpaused += OnUnpause;

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _aim, bulletSpeed * Time.deltaTime);
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
       // rb.velocity = Vector2.zero;
    }

//    private void OnUnpause()
//    {
//        rb.velocity = -Vector2.up * bulletSpeed;
//    }
}
