using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // Player spawn location and Player prefab
    [SerializeField] public Transform PlayerSpawnLocation;
    [SerializeField] public GameObject PlayerPrefab;

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
            Debug.Log("lives value and current lives "  + value.ToString() + " " + _lives.ToString());
            //lost a life
            if (value >= 0 && value < _lives)
            { 
                //SpawnPlayer(PlayerSpawnLocation);
                CanvasManager.Instance.UpdateLifeImage(value);

            }

            else if (value < 0)
            { GameOver(); }

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
    public void SpawnPlayer(Transform location)
    {
        Instantiate(PlayerPrefab, location.transform.position, Quaternion.identity);
    }

    // GameOver() called when lives are < 0.
    public void GameOver()
    {
        // Function to be completed
        
        Debug.Log("GameOver");
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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (PlayerSpawnLocation != null && PlayerPrefab != null)
        {
            SpawnPlayer(PlayerSpawnLocation);
        }
        else
        {
            Debug.LogWarning("No player spawn location set in GameManager");
        }

        Debug.Log("Start current number of lives" + Lives.ToString());
        //CanvasManager.Instance.UpdateLifeImage(Lives);
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
        // will also need to restart the enemy script. 
    }


    // Update is called once per frame
    //protected override void Update()
    //{        
    //}
    // Not sure if we need it.



    //Added to test game over menu. 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Lives -= 1;
           // Debug.Log(_lives.ToString());
        }

        if (Input.GetKeyDown(KeyCode.N))
        {

            AddToScore(10);
            Debug.Log(_lives.ToString());
        }

    }



}
