using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static GameOverMenu instance;

    public bool menuActive = false;

    public string[] quotes;
    public string[] quoteAuthor;

    public Text inspirationText;
    public Text authorText;

    public bool newGame = false;

    //UI texts
    public Text tryAgainText;
    public Text mainMenuText;
    public Text quitText;

    //PS4 texts
    public GameObject tryAgainPS4;
    public GameObject mainMenuPS4;
    public GameObject quitPS4;

    // NOTE: For the Game Over Menu, being "active" just means that it was pulled down. It should ALWAYS be in the scene!

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // PS4 vs. PC
        tryAgainPS4.SetActive(Master.instance.PS4active);
        mainMenuPS4.SetActive(Master.instance.PS4active);
        quitPS4.SetActive(Master.instance.PS4active);

        tryAgainText.gameObject.SetActive(!Master.instance.PS4active);
        mainMenuText.gameObject.SetActive(!Master.instance.PS4active);
        quitText.gameObject.SetActive(!Master.instance.PS4active);

        // Giving the PS4 controller functionality
        if (Master.instance.PS4active && menuActive)
        {
            if(Input.GetButtonDown("Fire2"))
            {
                RestartGame();
            }
            if(Input.GetButtonDown("Triangle"))
            {
                MainMenu();
            }
            if(Input.GetButtonDown("PSButton"))
            {
                QuitGame();
            }
        }
    }

    public void SelectQuote()
    {
        //Selects the quotation to use randomly
        int selection = Random.Range(0, quotes.Length);

        //Populates the text fields with the quote and its author (based on random selection)
        inspirationText.text = quotes[selection];
        authorText.text = quoteAuthor[selection];
    }

    //Close the menu
    public void CloseMenu()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        menuActive = false;
        theMenu.SetActive(false);
    }

    //Open the menu                 //The victory menu shares a lot of these functions because I am lazy and this game is almost complete :)
    public void OpenMenu()
    {
        //Menu opened; player can't move, menu is active, player cannot shoot
        PlayerController.instance.canMove = false;
        PlayerController.instance.canShoot = false;

        menuActive = true;
        theMenu.SetActive(true);

        //Make it so the pause menu cannot be opened
        PauseMenu.instance.canOpenPause = false;
    }

    public void RestartGame()
    {
        //Load up the first level
        SceneManager.LoadScene("Floor0");

        //Restart the timer
        HUD.instance.currentTime = 0f;

        //Clear the coins and arrows
        GameManager.instance.coinCount = 0;
        GameManager.instance.arrowCount = 0;
        HUD.instance.UpdateCoins();
        HUD.instance.UpdateArrows();

        //Prevent the timer from running until the player moves for the first time
        newGame = true;

        //The game is reset, so gameEnded is false
        GameManager.instance.gameEnded = false;

        //Get rid of all the player's hearts; reset the heartsSaved count
        GameManager.instance.hearts = 0;
        GameManager.instance.heartsSaved = 30;

        //Close all the menus (which in turn allows for player movement and shooting again)
        PauseMenu.instance.CloseMenuNoSound();
        SubmittedMenu.instance.CloseMenuNoSound();
        EntryMenu.instance.CloseMenuNoSound();
        //Ensure the victory menu is closed
        VictoryMenu.instance.menuActive = false;

        //Allow the pause menu to be opened again
        PauseMenu.instance.canOpenPause = true;

        //This is the game over menu
        CloseMenu();

        //Start the main music again
        AudioManager.instance.PlayBGM(1);

        //Move the Game Over Menu back to its original position (via animation)
        HUD.instance.myAnim.SetBool("moveToClose", true);
        //Same for victory menu
        HUD.instance.myAnim.SetBool("v_moveToClose", true);

        //These must be set to false so as to not keep re-opening the menu
        HUD.instance.myAnim.SetBool("moveToOpen", false);
        HUD.instance.myAnim.SetBool("isOpen", false);
        //Same for the victory menu
        HUD.instance.myAnim.SetBool("v_moveToOpen", false);
        HUD.instance.myAnim.SetBool("v_isOpen", false);

        //Restart the button count thing for the menu
        SubmittedMenu.instance.xButtonCount = 2;

        //Put the player in the correct spot
        PlayerController.instance.transform.position = new Vector3(0, 0, 0);

        //Advance the load count on the Master
        Master.instance.loadCount++;

        //Restart the button count thing

        //Play a UI sound
        AudioManager.instance.PlaySFX(5);
    }

    //Go to the mainMenu
    public void MainMenu()
    {
        //Play UI sound     //This doesn't play; figure out how to fix it
        AudioManager.instance.PlaySFX(4);

        //Destroy all instances (except audio)
        Destroy(PlayerController.instance.gameObject);
        Destroy(UIFade.instance.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(HUD.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);

        //Load the main menu
        SceneManager.LoadScene("Main Menu");
    }

    //Quits  the game
    public void QuitGame()
    {
        Application.Quit();

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }
}
