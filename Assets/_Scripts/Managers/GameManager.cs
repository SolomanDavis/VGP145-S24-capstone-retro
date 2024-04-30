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
            if (value >= 0 && value < _lives)
            { SpawnPlayer(PlayerSpawnLocation); }

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
    }


    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
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

            _lives -= 1;
            Debug.Log(_lives.ToString());
        }
    }



}
