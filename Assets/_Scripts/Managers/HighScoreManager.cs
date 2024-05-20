using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : Singleton<HighScoreManager>
{

    private int _highScore;       


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    public int GetHighScore(int _score)
    {       
        return _highScore;
    }
      
    public void SetHighScore(int _score)
    {
        if (_score > _highScore)
        {
            _highScore = _score;
        }
    }
}
