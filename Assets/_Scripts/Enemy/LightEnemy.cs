using UnityEngine;

public class LightEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "PlayerProjectile")
        {
            TakeDamage(2);
        }
    }
}