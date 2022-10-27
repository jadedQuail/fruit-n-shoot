using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    public int score;
    public string player;

    //Constructor
    public Score(string player, int score)
    {
        this.player = player;
        this.score = score;  
    }
}
