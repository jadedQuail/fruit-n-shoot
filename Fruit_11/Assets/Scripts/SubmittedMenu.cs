using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmittedMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static SubmittedMenu instance;

    public bool menuActive = false;

    // UI button text
    public Text tryAgainText;
    public Text mainMenuText;
    public Text quitText;

    public GameObject tryAgainPS4;
    public GameObject mainMenuPS4;
    public GameObject quitPS4;

    // Booleans to allow use of buttons for PS4
    public bool tryAgainAllowed = false;
    public bool mainMenuAllowed = false;
    public bool quitAllowed = false;

    // CHEAP HACK
    public int xButtonCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // PC vs. PS4
        tryAgainPS4.SetActive(Master.instance.PS4active);
        mainMenuPS4.SetActive(Master.instance.PS4active);
        quitPS4.SetActive(Master.instance.PS4active);

        tryAgainText.gameObject.SetActive(!Master.instance.PS4active);
        mainMenuText.gameObject.SetActive(!Master.instance.PS4active);
        quitText.gameObject.SetActive(!Master.instance.PS4active);

        // Giving the PS4 controller functionality
        if (Master.instance.PS4active && menuActive)
        {
            // Tapping into the GameOver menu's functionality because this menu doesn't have it
            if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Return)) && tryAgainAllowed)
            {
                Debug.Log("Accessed!");
                // Since the press of "X" on the previous menu triggers this one two, I make sure this is pressed "twice"
                // before it responds. It's a hack, but it works.
                xButtonCount -= 1;

                if (xButtonCount <= 0)
                {
                    // Re-enable menu options from victory menu
                    VictoryMenu.instance.tryAgainAllowed = true;
                    VictoryMenu.instance.mainMenuAllowed = true;
                    VictoryMenu.instance.quitAllowed = true;
                    VictoryMenu.instance.recordAllowed = true;

                    // Disable these buttons
                    EntryMenu.instance.submitAllowed = false;
                    EntryMenu.instance.cancelAllowed = false;

                    // Reset the text field
                    CustomSubmit.instance.field.text = "";

                    // Reset the reminder text
                    EntryMenu.instance.reminderText.gameObject.SetActive(false);

                    // Reset this button count
                    xButtonCount = 2;

                    GameOverMenu.instance.RestartGame();
                }
            }
            if (Input.GetButtonDown("Triangle") && mainMenuAllowed)
            {
                GameOverMenu.instance.MainMenu();
            }
            if (Input.GetButtonDown("PSButton") && quitAllowed)
            {
                GameOverMenu.instance.QuitGame();
            }
        }
    }

    //Function to open this menu
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

    //Function to close this menu
    public void CloseMenu()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        theMenu.SetActive(false);
        menuActive = false;

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
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
}
