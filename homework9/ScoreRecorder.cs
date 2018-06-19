using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder: MonoBehaviour {

    private int score;
    public ScoreRecorder()
    {
        score = 5;
    }
    public int GetScore()
    {
        return score;
    }
    public void AddScore()
    {
        score+=2;
    } 
    public void SubScore()
    {
        score--;
    }
    public void Reset()
    {
        score = 0;
    }
}
