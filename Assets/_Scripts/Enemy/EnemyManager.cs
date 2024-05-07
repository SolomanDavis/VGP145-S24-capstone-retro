using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyManager : SingletonInScene<EnemyManager>
{
    // Event signals for other game systems to listen to
    public event UnityAction AllEnemiesSpawned; // All enemies have been spawned
    public event UnityAction AllEnemiesKilled; // All have been spawned and all enemies have been killed

    // Numbers of enemies total and in squad to spawn in the game
    [SerializeField] public int TotalEnemies = 36;
    [SerializeField] public int MinEnemiesPerSquad = 4;
    [SerializeField] public int MaxEnemiesPerSquad = 8;

    // Time between spawning enemy squads
    [SerializeField] public float MinTimeBetweenSquads = 10f;
    [SerializeField] public float MaxTimeBetweenSquads = 20f;

    // Enemy spawn info including spawning distribution of enemy types in the game
    [SerializeField] public EnemySpawnInfo[] EnemySpawnInfos;
    private int[] _enemyDistributions;

    // TODO: ZA - replace with actual spawn locations
    // List of possible spawn locations for enemy squads
    [SerializeField] public Transform[] EnemySpawnLocations;

    // Time since the last squad of enemies was spawned
    private float _timeSinceLastSquadSpawned = 0f;

    // Statistics to drive scoring and game conditions
    private int _enemiesSpawned = 0;
    private int _enemiesAlive = 0;
    private int _enemiesKilled = 0;

    // Flag to indicate if the enemy manager is currently spawning enemies
    private bool _isSpawning = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (TotalEnemies == 0)
            Debug.LogError("[EnemyManager] Total number of enemies to spawn is set to 0.");

        if (MinEnemiesPerSquad == 0 || MaxEnemiesPerSquad == 0 || MaxEnemiesPerSquad > TotalEnemies)
            Debug.LogError("[EnemyManager] Min or max number of enemies per squad is set to invalid value.");

        if (EnemySpawnLocations.Length == 0)
            Debug.LogError("[EnemyManager] No enemy spawn locations set.");

        // Verify spawn info is set correctly
        int distributionSum = 0;
        _enemyDistributions = new int[EnemySpawnInfos.Length];
        for (int i = 0; i < EnemySpawnInfos.Length; i++)
        {
            if (EnemySpawnInfos[i].Prefab == null)
                Debug.LogError("[EnemyManager] Enemy prefab not set for spawn info.");

            if (EnemySpawnInfos[i].DistributionValue <= 0)
                Debug.LogError("[EnemyManager] Invalid enemy spawn distribution value set.");

            _enemyDistributions[i] = EnemySpawnInfos[i].DistributionValue; // Initialize enemy spawn distributions array
            distributionSum += _enemyDistributions[i];
        }

        if (distributionSum != TotalEnemies)
            Debug.LogError("[EnemyManager] Enemy type distribution does not match total number of enemies to spawn.");
    }

    public void SpawnEnemies()
    {
        // Choose a random spawn location for the squad
        Transform spawnLocation = EnemySpawnLocations[Random.Range(0, EnemySpawnLocations.Length)];

        // Choose a random number of enemies to spawn in the squad, accounting for the number of enemies left to spawn
        int maxEnemiesLeft = TotalEnemies - _enemiesSpawned;
        if (maxEnemiesLeft == 0)
            return;

        int numberOfEnemiesToSpawn = Mathf.Min(maxEnemiesLeft, Random.Range(MinEnemiesPerSquad, MaxEnemiesPerSquad));

        // Based on the enemy type distribution remaining, spawn the enemies in the squad
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            EnemySpawnInfo chosenInfo;
            if (!ChooseEnemyToSpawn(EnemySpawnInfos, _enemyDistributions, out chosenInfo))
            {
                // If no enemies left to spawn, stop spawning
                _isSpawning = false;
            }

            // Spawn the chosen enemy at the chosen location
            Instantiate(chosenInfo.Prefab, spawnLocation.position, Quaternion.identity);

            // TODO: ZA - debugging purposes
            Debug.Log("ZA - spawned enemy of Prefab type: " + chosenInfo.Prefab.name);

            // Update stats
            _enemiesSpawned++;
            _enemiesAlive++;
            _enemyDistributions[Array.IndexOf(EnemySpawnInfos, chosenInfo)]--; // Decrement the distribution value of the chosen enemy type

            // If all enemies have been spawned, raise the AllEnemiesSpawned event
            if (_enemiesSpawned == TotalEnemies)
            {
                AllEnemiesSpawned?.Invoke();
            }
        }
    }

    // Choose an enemy type to spawn based on the distribution of enemy types remaining
    // Returns the spawn info associated with the enemy to spawn
    private bool ChooseEnemyToSpawn(EnemySpawnInfo[] spawnInfos, int[] currentDistributions, out EnemySpawnInfo chosenInfo)
    {
        if (spawnInfos.Length != currentDistributions.Length)
            throw new ArgumentException("[EnemyManager] Enemy spawn infos and distributions arrays are not the same length.");

        // Build running sum array from enemy type distribution
        int runningSum = 0;
        int[] runningSums = new int[3];
        for (int i = 0; i < spawnInfos.Length; i++)
        {
            runningSum += currentDistributions[i];
            runningSums[i] = runningSum;
        }

        // Validate there are still enemies left to spawn
        if (runningSums[runningSums.Length - 1] == 0)
        {
            Debug.Log("[EnemyManager] No enemies left to spawn.");
            chosenInfo = spawnInfos[0];
            return false;
        }

        // Choose a random value between 1 and the total sum of distributions, which is the last element in the running sum array
        int randomValue = Random.Range(0, runningSums[runningSums.Length - 1]);

        for (int i = 0; i < runningSums.Length; i++)
        {
            if (randomValue < runningSums[i])
            {
                chosenInfo = spawnInfos[i];
                return true;
            }
        }

        // Should never happen
        Debug.LogError("[EnemyManager] Error choosing random enemy type.");
        chosenInfo = spawnInfos[0];
        return false;
    }

    public void StartSpawningEnemies()
    {
        Debug.Log("ZA - StartSpawningEnemies");
        _isSpawning = true;
    }

    public void Update()
    {
        // Trigger the coroutine to spawn a squad of enemies if we have not reached the total number of enemies to spawn
        // and an interval has passed since the last squad was spawned.
        if (IsAbleToSpawn())
        {
            _timeSinceLastSquadSpawned = 0f; // Reset time since last squad spawned
            SpawnEnemies();
        }

        // Update time since last squad was spawned
        _timeSinceLastSquadSpawned += Time.deltaTime;

        // If all enemies have been spawned and killed, raise OnAllEnemiesKilled event to signal win condition
        if (IsAllEnemiesKilled())
        {
            AllEnemiesKilled?.Invoke();
        }
    }

    private bool IsAbleToSpawn()
    {
        return _isSpawning
            && _enemiesSpawned < TotalEnemies
            && _timeSinceLastSquadSpawned > Random.Range(MinTimeBetweenSquads, MaxTimeBetweenSquads);
    }

    private bool IsAllEnemiesKilled()
    {
        return _enemiesSpawned == TotalEnemies && _enemiesKilled == TotalEnemies;
    }
}
