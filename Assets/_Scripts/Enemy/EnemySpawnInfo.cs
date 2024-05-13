using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnInfo", menuName = "Enemy/EnemySpawnInfo")]
public class EnemySpawnInfo : ScriptableObject
{
    // TODO: ZA - replace type with abstract enemy class type : Completed (Estelle) to add EnemyKilled event
    // Prefab of the enemy to spawn
    [SerializeField] public Enemy Prefab;

    // The sum of all distribution values of all enemy spawn infos must equal the total number of enemies to spawn
    [SerializeField] public int DistributionValue;
}
