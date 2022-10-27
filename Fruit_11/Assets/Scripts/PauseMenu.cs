using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static PauseMenu instance;

    public bool menuActive = false;

    //Bool that controls when the pause menu can be opened (i.e. can't during the game over sequence)
    public bool canOpenPause = true;

    // Text
    public Text resumeText;
    public Text mainMenuText;
    public Text quitText;
    public Text xText;

    // PS4 UI groups
    public GameObject resumePS4;
    public GameObject mainMenuPS4;
    public GameObject quitPS4;
    public GameObject xPS4;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Master.instance.PS4active && menuActive)
        {
            if (Input.GetButtonDown("Circle"))
            {
                CloseMenu();
            }
            if (Input.GetButtonDown("Triangle"))
            {
                MainMenu();
            }
            if (Input.GetButtonDown("PSButton"))
            {
                QuitGame();
            }
        }

        // PS4 vs. PC
        resumePS4.SetActive(Master.instance.PS4active);
        mainMenuPS4.SetActive(Master.instance.PS4active);
        quitPS4.SetActive(Master.instance.PS4active);
        xPS4.SetActive(Master.instance.PS4active);

        resumeText.gameObject.SetActive(!Master.instance.PS4active);
        mainMenuText.gameObject.SetActive(!Master.instance.PS4active);
        quitText.gameObject.SetActive(!Master.instance.PS4active);
        xText.gameObject.SetActive(!Master.instance.PS4active);

        //Checks for the escape key
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Options"))
        {
            //If the menu is not active, activate it; make sure the player can't move.
            if(menuActive == false && canOpenPause == true)
            {
                OpenMenu();
            }
            //If the menu is active, deactivate it; make sure the player can move again.
            else if(menuActive == true)
            {
                CloseMenu();
            }
        }
    }

    //Close the menu
    public void CloseMenu()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        theMenu.SetActive(false);
        menuActive = false;

        //Play UI sound
        AudioManager.instance.PlaySFX(5);
    }

    //Close the menu without a sound effect (for the GameOver menu)
    public void CloseMenuNoSound()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        theMenu.SetActive(false);
        menuActive = false;
    }

    //Open the menu
    public void OpenMenu()
    {
        //Menu opened; player can't move, menu is active, player cannot shoot
        PlayerController.instance.canMove = false;
        PlayerController.instance.canShoot = false;

        theMenu.SetActive(true);
        menuActive = true;

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }

    //Go to the mainMenu
    public void MainMenu()
    {
        //Destroy all instances (except audio)
        Destroy(PlayerController.instance.gameObject);
        Destroy(UIFade.instance.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(HUD.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);

        //Load the main menu
        SceneManager.LoadScene("Main Menu");

        //Play UI sound     //This doesn't play; figure out how to fix it
        AudioManager.instance.PlaySFX(4);
    }

    //Quits  the game
    public void QuitGame()
    {
        Application.Quit();

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }
}
