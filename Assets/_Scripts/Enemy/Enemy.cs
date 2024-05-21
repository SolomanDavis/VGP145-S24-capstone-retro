using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor.ShaderKeywordFilter;

public abstract class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected Animator anim;
    AudioSource audioSource;

    [SerializeField] protected int EnemyHealth;
    [SerializeField] private EnemyProjectile enemyProjectile;
    public Transform enemyProjectileSpawn;
    [SerializeField] private int projectileSpeed;
    public float TimeToDestroy = 1;
    [SerializeField] AudioClip enemydeath;
    [SerializeField] private float shootCooldown = 1f;
    public event UnityAction EnemyKilled;

    public float maxAngle = 45f;

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
            
            Shoot(0, 0);
        }
    }

    // TriggerOnAnimationEvent
    public void Shoot(int min, int max)
    {
        //This offset will allow the enemy script to choose to fire the projectile
        //at the player with an offset to the left and right (we think....)
        int RandomNumberOffset = Random.Range(min, max);

        EnemyProjectile currentProjectile = Instantiate(enemyProjectile, enemyProjectileSpawn.position, Quaternion.identity);

        currentProjectile.bulletSpeed = projectileSpeed;

        currentProjectile.offset = RandomNumberOffset;

        StartCoroutine(ShootCooldown());
    }

    public virtual void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth <= 0)
        {
            anim.SetTrigger("IsDead");

            audioSource.PlayOneShot(enemydeath);

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
      
        //Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);

       // Debug.DrawLine(Vector3.zero, Vector3.up, Color.green);

        float angle = Vector3.Angle(transform.up, downVector);

        //Debug.Log("Angle: " + angle);

        return angle <= maxAngle;
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
