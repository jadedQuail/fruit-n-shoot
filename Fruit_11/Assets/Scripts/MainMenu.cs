using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public GameObject topButtons;
    public GameObject botButtons;
    public GameObject backButton;
    public GameObject creditsGroup;

    public GameObject leaderMenu;

    //Flag for credits being enabled
    public bool creditsEnabled;

    //Flag for slider being enabled
    public bool sliderRunning = true;

    //Flag for leaderboard being enabled
    public bool leaderboardEnabled;

    // Text on buttons
    public Text playText;
    public Text quitText;
    public Text creditsText;
    public Text leaderboardText;

    public Text creditsBackText;
    public Text leaderboardBackText;

    // PS4 counterparts
    public GameObject playGroupPS4;
    public GameObject quitGroupPS4;
    public GameObject creditsGroupPS4;
    public GameObject leaderboardGroupPS4;

    public GameObject creditsBackGroupPS4;
    public GameObject leaderboardBackGroupPS4;

    // Enabling/disabling buttons for PS4 controller
    private bool playAllowed = true;
    private bool quitAllowed = true;
    private bool creditsAllowed = true;
    private bool leaderboardAllowed = true;
    private bool back_credits = false;
    private bool back_leaderboard = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        // Make the game run as fast as possible
        Application.targetFrameRate = 300;

        // Force the resolution to be 1920x1080
        //int width = 1920; // or something else
        //int height = 1080; // or something else
        //bool isFullScreen = false; // should be windowed to run in arbitrary resolution
        //int desiredFPS = 60; // or something else

        //Screen.SetResolution(width, height, isFullScreen, desiredFPS);

    }

    // Update is called once per frame
    void Update()
    {
        // Change text based on PS4 controller being active or not
        playGroupPS4.SetActive(Master.instance.PS4active);
        quitGroupPS4.SetActive(Master.instance.PS4active);
        creditsGroupPS4.SetActive(Master.instance.PS4active);
        leaderboardGroupPS4.SetActive(Master.instance.PS4active);
        creditsBackGroupPS4.SetActive(Master.instance.PS4active);
        leaderboardBackGroupPS4.SetActive(Master.instance.PS4active);

        playText.gameObject.SetActive(!Master.instance.PS4active);
        quitText.gameObject.SetActive(!Master.instance.PS4active);
        creditsText.gameObject.SetActive(!Master.instance.PS4active);
        leaderboardText.gameObject.SetActive(!Master.instance.PS4active);
        creditsBackText.gameObject.SetActive(!Master.instance.PS4active);
        leaderboardBackText.gameObject.SetActive(!Master.instance.PS4active);

        // Give functionality to the PS4 buttons
        if (Master.instance.PS4active)
        {
            if (Input.GetButtonDown("Fire2") && playAllowed)
            {
                //Reset everything to original values
                playAllowed = true;
                leaderboardAllowed = true;
                creditsAllowed = true;
                quitAllowed = true;
                back_credits = false;
                back_leaderboard = false;

                StartGame();
            }
            if (Input.GetButtonDown("Square") && creditsAllowed)
            {
                BeginCredits();

                playAllowed = false;
                leaderboardAllowed = false;
                quitAllowed = false;
                creditsAllowed = false;

                back_credits = true;

            }
            if (Input.GetButtonDown("Triangle") && leaderboardAllowed)
            {
                OpenLeaderboard();
                playAllowed = false;
                creditsAllowed = false;
                quitAllowed = false;
                leaderboardAllowed = false;

                back_leaderboard = true;
            }
            if (Input.GetButtonDown("Circle") && quitAllowed)
            {
                //Reset everything to original values
                playAllowed = true;
                leaderboardAllowed = true;
                creditsAllowed = true;
                quitAllowed = true;
                back_credits = false;
                back_leaderboard = false;

                QuitGame();
            }
            if (Input.GetButtonDown("Circle") && back_credits)
            {
                //Reset everything to original values
                playAllowed = true;
                leaderboardAllowed = true;
                creditsAllowed = true;
                quitAllowed = true;
                back_credits = false;
                back_leaderboard = false;

                ExitCredits();
            }
            if (Input.GetButtonDown("Circle") && back_leaderboard)
            {
                //Reset everything to original values
                playAllowed = true;
                leaderboardAllowed = true;
                creditsAllowed = true;
                quitAllowed = true;
                back_credits = false;
                back_leaderboard = false;

                CloseLeaderboard();
            }
        }

        //Hide the fruit if the leaderboard is enabled
        if (leaderboardEnabled)
        {
            var foundFruits = FindObjectsOfType<Target>();
            foreach(Target fruit in foundFruits)
            {
                fruit.GetComponent<Renderer>().enabled = false;
            }
        }
        else  //Re-show the fruit if the leaderboard is disabled
        {
            var foundFruits = FindObjectsOfType<Target>();
            foreach (Target fruit in foundFruits)
            {
                fruit.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void BeginCredits()
    {
        //Disable main buttons
        topButtons.SetActive(false);
        botButtons.SetActive(false);

        //Enable the back button
        backButton.SetActive(true);

        //Enable the actual credits
        creditsGroup.SetActive(true);

        //Enable credits flag
        creditsEnabled = true;

        //Play a UI sound
        AudioManager.instance.PlaySFX(4);
    }

    public void ExitCredits()
    {
        //Enable main buttons
        topButtons.SetActive(true);
        botButtons.SetActive(true);

        //Disable the back button
        backButton.SetActive(false);

        //Disable the actual credits
        creditsGroup.SetActive(false);

        //Disable credits flag
        creditsEnabled = false;

        //Play a UI sound
        AudioManager.instance.PlaySFX(4);
    }

    public void StartGame()
    {
        //Load the first scenen
        SceneManager.LoadScene("Floor0");

        //Shut off the slider
        sliderRunning = false;

        //Play a UI sound
        AudioManager.instance.PlaySFX(5);

        //Advance the load count on the master
        Master.instance.loadCount++;

        //First time load, bring up instruction
        if (InstructionMenu.instance != null)
        {
            InstructionMenu.instance.OpenMenu();
        } 
    }

    //Quits  the game
    public void QuitGame()
    {
        Application.Quit();

        //Play a UI sound
        AudioManager.instance.PlaySFX(4);
    }

    public void OpenLeaderboard()
    {
        //Disable main buttons
        topButtons.SetActive(false);
        botButtons.SetActive(false);

        //Enable the leaderboard menu
        leaderMenu.SetActive(true);
        leaderboardEnabled = true;

        //Play a UI sound
        AudioManager.instance.PlaySFX(4);
    }

    public void CloseLeaderboard()
    {
        //Enable main buttons
        topButtons.SetActive(true);
        botButtons.SetActive(true);

        //Disable the leaderboard menu
        leaderMenu.SetActive(false);
        leaderboardEnabled = false;

        //Play a UI sound
        AudioManager.instance.PlaySFX(4);
    }
}
