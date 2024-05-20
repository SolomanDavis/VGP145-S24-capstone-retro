using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected Animator anim;
    
    [SerializeField] protected int EnemyHealth;
    [SerializeField] private EnemyProjectile enemyProjectile;
    public Transform enemyProjectileSpawn;
    [SerializeField] private int projectileSpeed;
    public float TimeToDestroy = 1;
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
        
    }

    // Update is called once per frame
    void Update()
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
        //This offset will allow the enemy script to choose to fire the projectile
        //at the player with an offset to the left and right (we think....)
        //int RandomNumberOffset = Random.Range(min, max);

        EnemyProjectile currentProjectile = Instantiate(enemyProjectile, enemyProjectileSpawn.position, Quaternion.identity);
        
        currentProjectile.bulletSpeed = projectileSpeed;

        _canShoot = false;

        StartCoroutine(ShootCooldown());
    }

    /* public void Shoot(int min, int max)
    {
        //This offset will allow the enemy script to choose to fire the projectile
        //at the player with an offset to the left and right (we think....)
        int RandomNumberOffset = Random.Range(min, max);

        EnemyProjectile currentProjectile = Instantiate(enemyProjectile, enemyProjectileSpawn.position, enemyProjectileSpawn.rotation);
        currentProjectile.speed = projectileSpeed;
        currentProjectile.offset = RandomNumberOffset;
    }*/

    public virtual void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth <= 0)
        {
            anim.SetTrigger("IsDead");

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
        //Vector3 upVector = transform.position - Vector3.up;
        //upVector.Normalize();

        //Debug.DrawLine(transform.position, upVector, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.red);

        Debug.DrawLine(Vector3.zero, Vector3.up, Color.green);

        float angle = Vector3.Angle(transform.up, downVector);

        //float angle = Vector3.Angle(transform.up, upVector);

        //Debug.Log("Angle: " + angle);

        return angle <= maxAngle;
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        _canShoot = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyDeathCollider"))
        {
            EnemyDeath(0); 
            Destroy(gameObject);
        }
    }


}
