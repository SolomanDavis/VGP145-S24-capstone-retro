using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public event UnityAction EnemyKilled;

    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected Animator anim;
    protected AudioSource audioSource;

    [SerializeField] private EnemyProjectile enemyProjectile;
    public Transform enemyProjectileSpawn;

    [SerializeField] protected int enemyHealth;
    [SerializeField] private int projectileSpeed;
    [SerializeField] private float shootCooldown = 1f;

    [SerializeField] AudioClip EnemyDeathClip;

    public float isLookingDownMaxAngle = 45f;

    private bool _isPaused = false;
    private bool _canShoot = true;

    private void Awake()
    {
        CanvasManager.Instance.GamePaused += () => _isPaused = true;
        CanvasManager.Instance.GameUnpaused += () => _isPaused = false;

    }

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_isPaused)
            return;

        if (IsLookingDown() && _canShoot)
        {
            
            Shoot();
        }
    }

    // TriggerOnAnimationEvent
    public void Shoot()
    {
        EnemyProjectile currentProjectile = Instantiate(enemyProjectile, enemyProjectileSpawn.position, Quaternion.identity);
        currentProjectile.bulletSpeed = projectileSpeed;

        // Initiate cooldown
        StartCoroutine(ShootCooldown());
    }

    public virtual void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            anim.SetTrigger("IsDead");
            audioSource.PlayOneShot(EnemyDeathClip);

            bc.enabled = false; // Turn off box collider to prevent further damage
        }
    }

    // This should called right at the end of the animation event on enemy death
    public virtual void EnemyDeath(int score)
    {
        GameManager.Instance.AddToScore(score);
        EnemyKilled?.Invoke();
        Destroy(gameObject);
    }

    public bool IsLookingDown()
    {
       Vector3 downVector = Vector3.down;
       float angle = Vector3.Angle(transform.up, downVector);

       return angle <= isLookingDownMaxAngle;
    }

    private IEnumerator ShootCooldown()
    {
        _canShoot = false;

        yield return new WaitForSeconds(shootCooldown);

        _canShoot = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyDeathCollider"))
        {
            EnemyDeath(0); 
        }
    }
}
