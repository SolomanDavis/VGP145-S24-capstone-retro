using UnityEngine;

public class MediumEnemy : Enemy
{
    public EnemyPathfinding enemyPathfindingState;

    protected override void Start()
    {
        base.Start();

        enemyPathfindingState = GetComponent<EnemyPathfinding>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "PlayerProjectile")
        {
            switch (enemyPathfindingState.State)
            {
                case EnemyPathfinding.PathfindingState.Entrance:
                case EnemyPathfinding.PathfindingState.Hover:
                    EnemyDeath(50);
                    break;
                case EnemyPathfinding.PathfindingState.Dive:
                    EnemyDeath(100);
                    break;
            }
        }
    }
}
