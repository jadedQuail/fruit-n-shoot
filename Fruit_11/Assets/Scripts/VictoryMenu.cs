using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour
{
    public static VictoryMenu instance;

    // Indication of whether the menu is open or not
    public bool menuActive;

    //Text
    public Text completionFinal;
    public Text coinFinal;
    public Text heartsFinal;
    public Text penaltyFinal;
    public Text scoreFinal;

    public Text submitScoreText;

    //Timer Text Transfer
    public Text finalTimeText;
    public Text inGameTimeText;

    //Numbers
    public int completionScore;
    public int coinScore;
    public int heartsScore;
    public int penaltyScore;
    public int scoreTotal;

    // UI button text
    public Text tryAgainText;
    public Text mainMenuText;
    public Text quitText;
    public Text recordScoreText;

    public GameObject tryAgainPS4;
    public GameObject mainMenuPS4;
    public GameObject quitPS4;
    public GameObject recordScorePS4;

    // Booleans for enabling/disabling button presses on PS4
    public bool tryAgainAllowed = true;
    public bool mainMenuAllowed = true;
    public bool quitAllowed = true;
    public bool recordAllowed = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        tryAgainPS4.SetActive(Master.instance.PS4active);
        mainMenuPS4.SetActive(Master.instance.PS4active);
        quitPS4.SetActive(Master.instance.PS4active);
        recordScorePS4.SetActive(Master.instance.PS4active);

        tryAgainText.gameObject.SetActive(!Master.instance.PS4active);
        mainMenuText.gameObject.SetActive(!Master.instance.PS4active);
        quitText.gameObject.SetActive(!Master.instance.PS4active);
        recordScoreText.gameObject.SetActive(!Master.instance.PS4active);

        // Giving the PS4 controller functionality
        if (Master.instance.PS4active && menuActive)
        {
            // Tapping into the GameOver menu's functionality because this menu doesn't have it
            if(Input.GetButtonDown("Fire2") && tryAgainAllowed)
            {
                GameOverMenu.instance.RestartGame();
            }
            if(Input.GetButtonDown("Triangle") && mainMenuAllowed)
            {
                GameOverMenu.instance.MainMenu();
            }
            if(Input.GetButtonDown("PSButton") && quitAllowed)
            {
                GameOverMenu.instance.QuitGame();
            }
            if(Input.GetButtonDown("Square") && recordAllowed)
            {
                // Enable the EntryMenu buttons
                EntryMenu.instance.submitAllowed = true;
                EntryMenu.instance.cancelAllowed = true;

                // Disable these buttons
                tryAgainAllowed = false;
                mainMenuAllowed = false;
                quitAllowed = false;
                recordAllowed = false;

                EntryMenu.instance.OpenMenu();
            }
        }

        //Completion Bonus
        completionScore = 500;
        completionFinal.text = completionScore.ToString();

        //Coins
        coinScore = GameManager.instance.coinCount * 5;
        coinFinal.text = coinScore.ToString();

        //Hearts
        heartsScore = GameManager.instance.heartsSaved * 10;

        //Can't be negative
        if(heartsScore < 0)
        {
            heartsScore = 0;
        }

        heartsFinal.text = heartsScore.ToString();

        //Timer
        penaltyScore = (int)Mathf.Floor(HUD.instance.finalTime);
        penaltyFinal.text = "-" + penaltyScore.ToString();

        //Final Score
        scoreTotal = completionScore + coinScore + heartsScore - penaltyScore;
        scoreFinal.text = scoreTotal.ToString();
        //Final score also added to entry menu
        submitScoreText.text = scoreTotal.ToString();

        //Show the final time
        finalTimeText.text = inGameTimeText.text;
    }
}
