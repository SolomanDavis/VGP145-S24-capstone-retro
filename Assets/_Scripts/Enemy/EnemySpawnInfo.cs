using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnInfo", menuName = "Enemy/EnemySpawnInfo")]
public class EnemySpawnInfo : ScriptableObject
{
    // Prefab of the enemy to spawn
    [SerializeField] public Enemy Prefab;

    // Rank of the enemy to spawn
    [SerializeField] public EnemyRank Rank;

    // The sum of all distribution values of all enemy spawn infos must equal the total number of enemies to spawn
    [SerializeField] public int DistributionValue;
}
