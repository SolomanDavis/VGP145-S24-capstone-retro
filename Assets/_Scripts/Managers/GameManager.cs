using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int _score;
    public int Score => _score;

    private int _lives;
    public int Lives
    {
        get { return _lives; }

        set { _lives = value; }
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

    }

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    //protected override void Update()
    //{        
    //}
    // Not sure if we need it.
}
