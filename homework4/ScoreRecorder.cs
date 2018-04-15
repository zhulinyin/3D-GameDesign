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
    public void AddScore(Color color)//不同颜色的飞碟得分不同
    {
        if (color == Color.red)
        {
            score += 1;
        }
        else if (color == Color.yellow)
        {
            score += 2;
        }
        else if (color == Color.gray)
        {
            score += 5;
        }
    }
    public void SubScore()
    {
        score -= 2;
    }
    public void Reset()
    {
        score = 0;
    }
}
