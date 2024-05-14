using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected Animator anim;
    
    [SerializeField] private int EnemyHealth;
    [SerializeField] private EnemyProjectile enemyProjectile;
    public Transform enemyProjectileSpawn;
    [SerializeField] private int projectileSpeed;
    public float TimeToDestroy = 1;

    public event UnityAction EnemyKilled;

    public float maxAngle = 45f;

    private bool _isPaused = false;

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

        if (IsLookingDown())
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

        EnemyProjectile currentProjectile = Instantiate(enemyProjectile, enemyProjectileSpawn.position, enemyProjectileSpawn.rotation);
        
        currentProjectile.speed = projectileSpeed;

        currentProjectile.offset = 0;
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
            Debug.Log("Anim triggered");
            anim.SetTrigger("IsDead");
            
        }
    }

    // This should called right at the end of the animation event on enemy death
    public virtual void EnemyDeath(int score)
    {
        GameManager.Instance.AddToScore(score);
        EnemyKilled?.Invoke();
        Destroy(gameObject);
        // EnemiesOnScreen --;
        // TotalNumberOfEnemiesKilled ++;
    }
    
    public bool IsLookingDown()
    {
        Vector3 upVector = transform.position - Vector3.up;
        //upVector.Normalize();

        Debug.DrawLine(transform.position, upVector, Color.red);

        Debug.DrawLine(Vector3.zero, Vector3.up, Color.green);

        float angle = Vector3.Angle(transform.up, upVector);
        //Debug.Log("Angle: " + angle);

        return angle <= maxAngle;
    }

}
