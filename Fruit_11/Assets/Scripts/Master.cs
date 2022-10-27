using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public static Master instance;

    public int loadCount = 0;
    public bool reachedReturnFire = false;
    public bool reachedShield = false;
    public bool reachedLemon = false;
    public bool reachedCookie = false;
    public bool reachedInstructions = false;
    public bool reachedKey = false;
    public bool PS4active = false;

    // SCOREBOARD //

    // Records top scores
    public List<Score> topScores = new List<Score>();

    // Bool to check if initial scoreboard-grab has happened
    public bool scoresLoaded;

    //This script is used to hold variables that need to be alive for the whole game

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //Activate or deactivate the PS4 controller
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                PS4active = true;       
            }
            else
            {
                PS4active = false;
            }
        }

        // Secret keycode for deleting all PlayerPrefs; not meant for the user to know
        if(Input.GetKey(KeyCode.LeftBracket) && Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.Equals))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public int SortFunc(Score a, Score b)
    {
        if (a.score > b.score)
        {
            return -1;
        }
        else if (a.score < b.score)
        {
            return 1;
        }
        return 0;
    }

    // Load the PlayerPrefs into a usable list
    public void PrefsToList()
    {
        // Only the top ten scores
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey("score" + (i + 1).ToString()))
            {
                topScores.Add(new Score(PlayerPrefs.GetString("name" + (i + 1).ToString()),
                                        PlayerPrefs.GetInt("score" + (i + 1).ToString())));
            }
        }
        //Sort the list
        topScores.Sort(SortFunc);
    }

    public void ListToPrefs()
    {
        // Sort the list
        topScores.Sort(SortFunc);

        // Write the scores to the PlayerPrefs
        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetString(("name"+(i+1).ToString()), topScores[i].player);
            PlayerPrefs.SetInt(("score" + (i + 1).ToString()), topScores[i].score);
        }
    }
}
