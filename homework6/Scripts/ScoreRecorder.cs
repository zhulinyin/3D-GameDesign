using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder: MonoBehaviour {

    private int score;
    public ScoreRecorder()
    {
        score = 0;
    }
    public int GetScore()
    {
        return score;
    }
    public void AddScore()
    {
        score++;
    }    
    public void Reset()
    {
        score = 0;
    }
}
