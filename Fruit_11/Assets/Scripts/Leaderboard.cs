using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    // Text objects for score, position, and name
    public Text[] scores;
    public Text[] positions;
    public Text[] names;

    // Bool for loading PlayerPrefs in
    public bool loaded = false;

    // This function does not work for this particular object on the first go around
    void Start()
    {
        // Second go-around; write to the scoreboard again
        WriteToBoard();
    }

    void Update()
    {
        // Load prefs; write to board
        if (Master.instance.scoresLoaded == false)
        {
            // Load prefs
            Master.instance.PrefsToList();

            // Initial write to board
            WriteToBoard();

            // Close off logic branch
            Master.instance.scoresLoaded = true;
        }
    }

    // Write from list to board
    public void WriteToBoard()
    {
        if (Master.instance != null)
        {
            // Sort the list
            Master.instance.topScores.Sort(Master.instance.SortFunc);

            for (int i = 0; i < Master.instance.topScores.Count; i++)
            {
                scores[i].text = Master.instance.topScores[i].score.ToString();
                names[i].text = Master.instance.topScores[i].player;
                positions[i].text = (i + 1).ToString();
            }
        }
    }
}
