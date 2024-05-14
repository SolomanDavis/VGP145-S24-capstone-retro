using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRankGridSlot : MonoBehaviour
{
    public Enemy enemy;

    public void AssignEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemy.EnemyKilled += UnassignEnemy;
    }

    public void UnassignEnemy()
    {
        enemy.EnemyKilled -= UnassignEnemy;
        enemy = null;
    }
}
