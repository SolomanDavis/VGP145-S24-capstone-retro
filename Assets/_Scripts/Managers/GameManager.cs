using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonInScene<GameManager>
{
    // Player spawn location and Player prefab
    [SerializeField] public Transform PlayerSpawnLocation;
    [SerializeField] public GameObject PlayerPrefab;

    [SerializeField] private float _spawnPlayerWaitTime = 2.0f;
    [SerializeField] private float _gameOverWaitTime = 2.0f;

    private int _highScore;

    private int _score;
    public int Score => _score;

    private int _lives = 2;
    public int Lives
    {
        get
        {
            return _lives;
        }

        set
        {
            //lost a life
            if (value >= 0 && value < _lives)
            { 
                StartCoroutine(SpawnPlayer(PlayerSpawnLocation));
                CanvasManager.Instance.UpdateLifeImage(value);
            }
            else if (value < 0)
            {
                StartCoroutine(GameOver());
            }

            _lives = value;
        }
    }

    // Add score to be called when enemies are destroyed.
    public int AddToScore(int amountToAdd)
    {
        _score += amountToAdd;
        return _score;
    }

    // Spawns a new player object at given location.
    private IEnumerator SpawnPlayer(Transform location)
    {
        yield return new WaitForSeconds(_spawnPlayerWaitTime);
        Instantiate(PlayerPrefab, location.transform.position, Quaternion.identity);
    }

    // GameOver() called when lives are < 0.
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(_gameOverWaitTime);
        CanvasManager.Instance.GameOver();
    }

    //if score is > high-score sets high-score text with score and replaces previous high-score. 
    public int HighScore(int _score)
    {
        if(_score > _highScore) 
        {
            _highScore = _score;
        }

        return _highScore;
    }

    protected override void Awake()
    {
        base.Awake();

        // Register OnAllEnemiesKilled event handler
        EnemyManager.Instance.AllEnemiesKilled += OnAllEnemiesKilled;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RestartGame();
    }

    // When called in canvas manager it changes the scene. 
    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    //Resets score and lives to starting default.
    public void RestartGame()
    {
        _score = 0;
        Lives = 2;

        CanvasManager.Instance.UpdateLifeImage(Lives);
        EnemyManager.Instance.Restart();
        DestroyPlayer();
        StartCoroutine(SpawnPlayer(PlayerSpawnLocation));
    }

    private void DestroyPlayer()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        for (int i = 0; i < players.Length; ++i)
        {
            Destroy(players[i].gameObject);
        }
    }

    //Added to test game over menu. 
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Lives -= 1;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {

            AddToScore(10);
        }
    }

    // Event handler for when all enemies are killed
    private void OnAllEnemiesKilled()
    {        
        CanvasManager.Instance.GameWon();
    }
}
